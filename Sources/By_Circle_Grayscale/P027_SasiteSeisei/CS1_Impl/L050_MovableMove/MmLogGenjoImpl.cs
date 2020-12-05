using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L002_GraphicLog;
using Grayscale.P027_SasiteSeisei.L00025_MovableMove;
using Grayscale.P006_Syugoron;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P025_KifuLarabe.L050_Things;
using Grayscale.P025_KifuLarabe.L100_KifuIO;
using System;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P027_SasiteSeisei.L050_MovableMove
{
    public class MmLogGenjoImpl : MmLogGenjo
    {

        public GraphicalLog_Board BrdMove { get; set; }

        public bool Enable { get { return this.enable; } }
        private bool enable;

        public int YomuDeep { get { return this.yomuDeep; } }
        private int yomuDeep;//脳内読み手数

        public int Tesumi_yomiCur { get { return this.tesumi_yomiCur; } }
        private int tesumi_yomiCur;

        public ShootingStarlightable Sasite { get { return this.sasite; } }
        private ShootingStarlightable sasite;

        public LarabeLoggerable LogTag { get { return this.logTag; } }
        private LarabeLoggerable logTag;


        public MmLogGenjoImpl(
            bool enable,
            GraphicalLog_Board brdMove,
            int yomuDeep,//脳内読み手数
            int tesumi_yomiCur,
            ShootingStarlightable sasite,
            LarabeLoggerable logTag
            )
        {
            this.enable = enable;
            this.BrdMove = brdMove;
            this.yomuDeep = yomuDeep;
            this.tesumi_yomiCur = tesumi_yomiCur;
            this.sasite = sasite;
            this.logTag = logTag;
        }

        public void Log1(Playerside pside_genTeban3)
        {
            this.BrdMove.Caption = "移動可能_" + Converter04.Sasite_ToString_ForLog(this.Sasite, pside_genTeban3);
            this.BrdMove.Tesumi = this.Tesumi_yomiCur;
            this.BrdMove.NounaiYomiDeep = this.YomuDeep;
            this.BrdMove.GenTeban = pside_genTeban3;// 現手番
        }

        public void Log2(
            Playerside tebanSeme,//手番（利きを調べる側）
            Playerside tebanKurau//手番（喰らう側）
        )
        {
            if (Playerside.P1 == tebanSeme)
            {
                this.BrdMove.NounaiSeme = Gkl_NounaiSeme.Sente;
            }
            else if (Playerside.P2 == tebanSeme)
            {
                this.BrdMove.NounaiSeme = Gkl_NounaiSeme.Gote;
            }
        }


        public void Log3(
            SkyConst src_Sky,
            Playerside tebanKurau,//手番（喰らう側）
            Playerside tebanSeme,//手番（利きを調べる側）
            Fingers fingers_kurau_IKUSA,//戦駒（喰らう側）
            Fingers fingers_kurau_MOTI,// 持駒（喰らう側）
            Fingers fingers_seme_IKUSA,//戦駒（利きを調べる側）
            Fingers fingers_seme_MOTI// 持駒（利きを調べる側）
        )
        {
            // 攻め手の駒の位置
            GraphicalLog_Board boardLog_clone = new GraphicalLog_Board(this.BrdMove);
            foreach (Finger finger in fingers_seme_IKUSA.Items)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).Now);

                Gkl_KomaMasu km = new Gkl_KomaMasu(
                    Util_GraphicalLog.PsideKs14_ToString(tebanSeme, Haiyaku184Array.Syurui(koma.Haiyaku), ""),
                    Util_Masu.AsMasuNumber(koma.Masu)
                    );
                boardLog_clone.KomaMasu1.Add(km);
            }

            foreach (Finger finger in fingers_kurau_IKUSA.Items)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).Now);

                this.BrdMove.KomaMasu2.Add(new Gkl_KomaMasu(
                    Util_GraphicalLog.PsideKs14_ToString(tebanKurau, Haiyaku184Array.Syurui(koma.Haiyaku), ""),
                    Util_Masu.AsMasuNumber(koma.Masu)
                    ));
            }

            foreach (Finger finger in fingers_seme_MOTI.Items)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).Now);

                Gkl_KomaMasu km = new Gkl_KomaMasu(
                    Util_GraphicalLog.PsideKs14_ToString(tebanSeme, Haiyaku184Array.Syurui(koma.Haiyaku), ""),
                    Util_Masu.AsMasuNumber(koma.Masu)
                    );
                this.BrdMove.KomaMasu3.Add(km);
            }

            foreach (Finger finger in fingers_kurau_MOTI.Items)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).Now);

                this.BrdMove.KomaMasu4.Add(new Gkl_KomaMasu(
                    Util_GraphicalLog.PsideKs14_ToString(tebanKurau, Haiyaku184Array.Syurui(koma.Haiyaku), ""),
                    Util_Masu.AsMasuNumber(koma.Masu)
                    ));
            }
            this.BrdMove = boardLog_clone;
        }

        public void Log4(
            SkyConst src_Sky,
            Playerside tebanSeme,//手番（利きを調べる側）
            Maps_OneAndOne<Finger, SySet<SyElement>> kmMove_seme_IKUSA
        )
        {
            // 戦駒の移動可能場所
            GraphicalLog_Board boardLog_clone = new GraphicalLog_Board(this.BrdMove);
            kmMove_seme_IKUSA.Foreach_Entry((Finger key, SySet<SyElement> value, ref bool toBreak) =>
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(key).Now);

                string komaImg = Util_GraphicalLog.PsideKs14_ToString(tebanSeme, Haiyaku184Array.Syurui(koma.Haiyaku), "");

                foreach (Basho masu in value.Elements)
                {
                    boardLog_clone.Masu_theMove.Add((int)masu.MasuNumber);
                }
            });

            this.BrdMove = boardLog_clone;
        }

    }
}
