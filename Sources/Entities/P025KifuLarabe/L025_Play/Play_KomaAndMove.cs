﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Forms;
using Grayscale.P025_KifuLarabe;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L007_Random;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P025_KifuLarabe.L025_Play;


using Grayscale.P025_KifuLarabe.L100_KifuIO;

using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P006_Syugoron;
using Grayscale.Kifuwarazusa.Entities;

namespace Grayscale.P025_KifuLarabe.L025_Play
{


    public abstract class Play_KomaAndMove
    {


        /// <summary>
        /// a - b = c
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="sbGohosyu"></param>
        /// <param name="logTag"></param>
        public static Maps_OneAndOne<Finger, SySet<SyElement>> MinusMasus(
            bool enableLog,
            SkyConst src_Sky_forLog,
            Maps_OneAndOne<Finger, SySet<SyElement>> a1,
            SySet<SyElement> b,
            ILogTag logTag_orNull
            )
        {
            //GraphicalLogUtil.Log(enableLog, "Thought_KomaAndMove#MinusMasus",
            //    "["+
            //    GraphicalLogUtil.JsonKyokumens_MultiKomabetuMasus(enableLog, siteiSky_forLog, a1, "a1") +
            //    "]"
            //    );


            Maps_OneAndOne<Finger, SySet<SyElement>> c = new Maps_OneAndOne<Finger, SySet<SyElement>>(a1);

            List<Finger> list_koma = c.ToKeyList();//調べたい側の全駒


            foreach (Finger selfKoma in list_koma)
            {
                SySet<SyElement> srcMasus = c.ElementAt(selfKoma);

                SySet<SyElement> minusedMasus = srcMasus.Minus_Closed( b);

                // 差替え
                c.AddReplace(selfKoma, minusedMasus, false);//差分に差替えます。もともと無い駒なら何もしません。
            }

            return c;
        }

        /// <summary>
        /// a - b = c
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="sbGohosyu"></param>
        /// <param name="logTag"></param>
        public static Maps_OneAndOne<Finger, SySet<SyElement>> Minus_OverThereMasus(
            bool enableLog,
            SkyConst src_srcSky_forLog,
            Maps_OneAndOne<Finger, SySet<SyElement>> a,
            SySet<SyElement> b,
            ILogTag logTag_orNull
        )
        {
            Maps_OneAndOne<Finger, SySet<SyElement>> c = new Maps_OneAndOne<Finger, SySet<SyElement>>(a);

            foreach (Finger selfKoma in c.ToKeyList())//調べたい側の全駒
            {
                SySet<SyElement> srcMasus = c.ElementAt(selfKoma);

                // a -overThere b するぜ☆
                Util_GraphicalLog.Log(enableLog, "Thought_KomaAndMove Minus_OverThereMasus",
                    "[\n" +
                    "    [\n" +
                    Util_GraphicalLog.JsonElements_Masus(enableLog, srcMasus, "(1)引く前") +
                    "    ],\n"+
                    "],\n"
                    );
                SySet<SyElement> minusedMasus = srcMasus.Clone();
                minusedMasus.MinusMe_Opened( b);

                // 差替え
                c.AddReplace(selfKoma, minusedMasus, false);//差分に差替えます。もともと無い駒なら何もしません。
            }

            Util_GraphicalLog.Log(enableLog, "Thought_KomaAndMove Minus_OverThereMasus",
                "[\n"+
                "    [\n" +
                Util_GraphicalLog.JsonKyokumens_MultiKomabetuMasus(enableLog, src_srcSky_forLog, a, "(1)a") +
                Util_GraphicalLog.JsonElements_Masus(enableLog, b, "(2)-overThere_b") +
                Util_GraphicalLog.JsonKyokumens_MultiKomabetuMasus(enableLog, src_srcSky_forLog, c, "(3)＝c") +
                "    ],\n"+
                "],\n"
                );

            return c;
        }


    }


}
