using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号
using Grayscale.P025_KifuLarabe.L00012_Atom;

namespace Grayscale.P025_KifuLarabe.L00025_Struct
{

    /// <summary>
    /// ************************************************************************************************************************
    /// 天空
    /// ************************************************************************************************************************
    /// 
    /// 局面のことです。
    /// 
    /// </summary>
    public class SkyConst : Sky
    {
        #region プロパティー

        /// <summary>
        /// TODO:
        /// </summary>
        public bool PsideIsBlack { get; set; }

        /// <summary>
        /// 「置き場に置けるものの素性」リストです。駒だけとは限りませんので、４０個以上になることもあります。
        /// </summary>
        private List<Starlight> starlights;

        #endregion


        public Sky Clone()
        {
            return new SkyConst(this);
        }


        public int Count
        {
            get
            {
                return this.starlights.Count;
            }
        }


        /// <summary>
        /// 棋譜を新規作成するときに使うコンストラクター。
        /// </summary>
        public SkyConst()
        {
            this.PsideIsBlack = true;
            this.starlights = new List<Starlight>();
        }

        /// <summary>
        /// クローンを作ります。
        /// </summary>
        /// <param name="src"></param>
        public SkyConst(Sky src)
        {
            Debug.Assert(src.Count == 40, "本将棋とみなしてテスト中。sky.Starlights.Count=[" + src.Count + "]");//将棋の駒の数

            // 星々のクローン
            this.PsideIsBlack = src.PsideIsBlack;
            this.starlights = new List<Starlight>();

            src.Foreach_Starlights((Finger finger, Starlight light, ref bool toBreak) =>
            {
                this.starlights.Add(light);
            });
        }

        public Starlight StarlightIndexOf(
            Finger finger,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
        )
        {
            Starlight found;

            if (0<=(int)finger && (int)finger < this.starlights.Count)
            {
                found = this.starlights[(int)finger];
            }
            else
            {
                string message = this.GetType().Name + "#StarIndexOf：　スプライト配列の範囲を外れた添え字を指定されましたので、取得できません。スプライト番号=[" + finger + "] / スプライトの数=[" + this.starlights.Count + "]\n memberName=" + memberName + "\n sourceFilePath=" + sourceFilePath + "\n sourceLineNumber=" + sourceLineNumber;
                Debug.Fail(message);
                throw new Exception(message);
            }

            return found;
        }


        public void Foreach_Starlights(SkyBuffer.DELEGATE_Sky_Foreach delegate_Sky_Foreach)
        {
            bool toBreak = false;

            Finger finger = 0;
            foreach (Starlight light in this.starlights)
            {
                delegate_Sky_Foreach(finger, light, ref toBreak);

                finger = (int)finger + 1;
                if (toBreak)
                {
                    break;
                }
            }
        }


        /// <summary>
        /// ************************************************************************************************************************
        /// 天上のすべての星の光
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="okiba"></param>
        /// <returns></returns>
        public Fingers Fingers_All()
        {
            Fingers fingers = new Fingers();

            this.Foreach_Starlights((Finger finger, Starlight light, ref bool toBreak) =>
            {
                fingers.Add(finger);
            });

            return fingers;
        }
    }
}
