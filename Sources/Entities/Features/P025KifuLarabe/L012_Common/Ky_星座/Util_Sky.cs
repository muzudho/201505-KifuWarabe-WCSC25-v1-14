using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P006_Syugoron;
using Grayscale.P006Sfen;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L006_SfenEx;
using Grayscale.P025_KifuLarabe.L100_KifuIO;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.P025_KifuLarabe.L012_Common
{
    public abstract class Util_Sky
    {

        public static readonly ShootingStarlightable NullObjectMove = new RO_ShootingStarlight(
            //Fingers.Error_1,
            new RO_Star_Koma(Playerside.Empty, Masu_Honshogi.Error, Ks14.H00_Null),
            new RO_Star_Koma(Playerside.Empty, Masu_Honshogi.Error, Ks14.H00_Null),
            null
            );

        public static SfenStringImpl ExportSfen(SkyConst src_Sky)
        {
            Debug.Assert(src_Sky.Count == 40, "sky.Starlights.Count=[" + src_Sky.Count + "]");//将棋の駒の数

            StartposExporter se = new StartposExporter(src_Sky);
            return new SfenStringImpl("sfen "+Util_SfenStartposWriter.CreateSfenstring(se, false));
        }

        public static SfenStringImpl ExportSfen_ForDebug(SkyConst src_Sky, bool psideIsBlack)
        {
            StartposExporter se = new StartposExporter(src_Sky);
            return new SfenStringImpl("sfen "+Util_SfenStartposWriter.CreateSfenstring(se, true));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sky"></param>
        /// <param name="sfenStartpos"></param>
        public static SkyConst ImportSfen(SfenStringImpl startposString)
        {
            StartposImporter startposImporter;
            string restText;
            bool successful = StartposImporter.TryParse(
                startposString.ValueStr,
                out startposImporter,
                out restText
                );

            return startposImporter.ToSky();
        }

        /// <summary>
        /// 「グラフィカル局面ログ」出力用だぜ☆
        /// </summary>
        public static string Json_1Sky(
            SkyConst src_Sky,
            string memo,
            string hint,
            int tesumi_yomiGenTeban_forLog//読み進めている現在の手目済

            //[CallerMemberName] string memberName = "",
            //[CallerFilePath] string sourceFilePath = "",
            //[CallerLineNumber] int sourceLineNumber = 0
            )
        {
            //...(^▽^)さて、局面は☆？
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[");

            // コメント
            string comment;
            {
                StringBuilder cmt = new StringBuilder();

                // メモ
                cmt.Append(memo);

                comment = cmt.ToString();
            }

            sb.AppendLine("    { act:\"drawText\", text:\"" + comment + "\", x: 20, y:20 },");//FIXME: \記号が入ってなければいいが☆

            int hKoma = 0;
            int hMasu_sente = 81;
            int hMasu_gote = 121;

            // 全駒
            src_Sky.Foreach_Starlights((Finger finger, Starlight light, ref bool toBreak) =>
            {
                RO_MotionlessStarlight ms = (RO_MotionlessStarlight)light;

                RO_Star_Koma koma = Util_Koma.AsKoma(ms.Now);


                Ks14 ks14 = Haiyaku184Array.Syurui(koma.Haiyaku);

                if (Util_Masu.GetOkiba(koma.Masu) == Okiba.Gote_Komadai)
                {
                    // 後手持ち駒
                    sb.AppendLine("    { act:\"drawImg\", img:\"" + Util_GraphicalLog.PsideKs14_ToString(koma.Pside, ks14, "") + "\", masu: " + hMasu_gote + " },");//FIXME: \記号が入ってなければいいが☆
                    hMasu_gote++;
                }
                else if (Util_Masu.GetOkiba(koma.Masu) == Okiba.Sente_Komadai)
                {
                    // 先手持ち駒
                    sb.AppendLine("    { act:\"drawImg\", img:\"" + Util_GraphicalLog.PsideKs14_ToString(koma.Pside, ks14, "") + "\", masu: " + hMasu_sente + " },");//FIXME: \記号が入ってなければいいが☆
                    hMasu_sente++;
                }
                else if (Util_Masu.GetOkiba(koma.Masu) == Okiba.ShogiBan)
                {
                    // 盤上
                    sb.AppendLine("    { act:\"drawImg\", img:\"" + Util_GraphicalLog.PsideKs14_ToString(koma.Pside, ks14, "") + "\", masu: " + Util_Masu.AsMasuNumber(koma.Masu) + " },");//FIXME: \記号が入ってなければいいが☆
                }

                hKoma++;
            });

            sb.AppendLine("],");

            // ...(^▽^)ﾄﾞｳﾀﾞｯﾀｶﾅ～☆
            return sb.ToString();
        }

        public static SySet<SyElement> Masus_Now(SkyConst src_Sky, Playerside pside)
        {
            SySet_Default<SyElement> masus = new SySet_Default<SyElement>("今の升");

            src_Sky.Foreach_Starlights((Finger finger, Starlight mlLight, ref bool toBreak) =>
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(mlLight.Now);


                    if (koma.Pside == pside && Util_Masu.GetOkiba(koma.Masu) == Okiba.ShogiBan)
                    {
                        masus.AddElement(koma.Masu);
                    }
            });

            return masus;
        }

        /// <summary>
        /// 駒のハンドルを返します。
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="kifuD"></param>
        /// <returns></returns>
        public static Fingers Fingers_ByOkibaPsideNow(SkyConst src_Sky, Okiba okiba, Playerside pside)
        {
            Fingers fingers = new Fingers();

            src_Sky.Foreach_Starlights((Finger finger, Starlight dd, ref bool toBreak) =>
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(dd.Now);

                    if (Util_Masu.GetOkiba(koma.Masu) == okiba
                        && pside == koma.Pside
                        )
                    {
                        fingers.Add(finger);
                    }
            });

            return fingers;
        }

        /// <summary>
        /// 駒の種類（不成として扱います）を指定して、駒を検索します。
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="syurui"></param>
        /// <param name="uc_Main"></param>
        /// <returns>無ければ -1</returns>
        public static Finger FingerNow_BySyuruiIgnoreCase(SkyConst src_Sky, Okiba okiba, Ks14 syurui)
        {
            Finger found = Fingers.Error_1;

            Ks14 syuruiNarazuCase = KomaSyurui14Array.NarazuCaseHandle(syurui);

            foreach (Finger finger in Finger_Honshogi.Items_KomaOnly)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).Now);

                    if (Util_Masu.GetOkiba(koma.Masu) == okiba
                        && KomaSyurui14Array.Matches(koma.ToNarazuCase(), syuruiNarazuCase))
                    {
                        found = finger;
                        break;
                    }
            }

            return found;
        }

        /// <summary>
        /// 駒のハンドル(*1)を返します。
        /// 
        ///         *1…将棋の駒１つ１つに付けられた番号です。
        /// 
        /// </summary>
        /// <param name="syurui"></param>
        /// <param name="hKomas"></param>
        /// <returns></returns>
        public static Fingers Fingers_BySyuruiNow(SkyConst src_Sky, Ks14 syurui)
        {
            Fingers figKomas = new Fingers();

            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);


                    if (KomaSyurui14Array.Matches(syurui, Haiyaku184Array.Syurui(koma.Haiyaku)))
                    {
                        figKomas.Add(figKoma);
                    }
            }

            return figKomas;
        }

        /// <summary>
        /// 駒のハンドルを返します。　：　置き場、種類
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="syurui"></param>
        /// <param name="kifu"></param>
        /// <returns></returns>
        public static Fingers Fingers_ByOkibaSyuruiNow(SkyConst src_Sky, Okiba okiba, Ks14 syurui)
        {
            Fingers komas = new Fingers();

            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);


                    if (
                        okiba == Util_Masu.GetOkiba(koma.Masu)
                        && KomaSyurui14Array.Matches(syurui, Haiyaku184Array.Syurui(koma.Haiyaku))
                        )
                    {
                        komas.Add(figKoma);
                    }
            }

            return komas;
        }

        /// <summary>
        /// 駒のハンドルを返します。
        /// </summary>
        /// <param name="okiba"></param>
        /// <param name="pside"></param>
        /// <param name="syurui"></param>
        /// <param name="kifu"></param>
        /// <returns></returns>
        public static Fingers Fingers_ByOkibaPsideSyuruiNow(SkyConst src_Sky, Okiba okiba, Playerside pside, Ks14 syurui)
        {
            Fingers figKomas = new Fingers();

            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);

                    if (
                        okiba == Util_Masu.GetOkiba(koma.Masu)
                        && pside == koma.Pside
                        && syurui == Haiyaku184Array.Syurui(koma.Haiyaku)
                        )
                    {
                        figKomas.Add(figKoma);
                    }
            }

            return figKomas;
        }

        /// <summary>
        /// 軌道上の駒たち
        /// </summary>
        /// <param name="km"></param>
        /// <returns></returns>
        public static Fingers Fingers_EachSrcNow(SkyConst src_Sky, Playerside pside, Starlight itaru, SySet<SyElement> srcList)
        {
            Fingers fingers = new Fingers();

            foreach (SyElement masu in srcList.Elements)
            {
                Finger finger = Util_Sky.Finger_AtMasuNow_Shogiban(src_Sky,pside, masu);
                if (Util_Finger.ForHonshogi(finger))
                {
                    // 指定の升に駒がありました。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
                    fingers.Add(finger);
                }
            }

            return fingers;
        }

        /// <summary>
        /// 指定のマスにある駒を返します。（本将棋用）
        /// </summary>
        /// <param name="masu">マス番号</param>
        /// <param name="logTag">ログ名</param>
        /// <returns>スプライト番号。なければエラー番号。</returns>
        public static RO_Star_Koma Koma_AtMasuNow(SkyConst src_Sky, SyElement masu)
        {
            RO_Star_Koma koma = null;

            Finger fig = Util_Sky.Fingers_AtMasuNow(src_Sky, masu).ToFirst();

            if (Fingers.Error_1 == fig)
            {
                // 指定の升には駒がない。
                goto gt_EndMethod;
            }

            koma = Util_Koma.FromFinger(src_Sky, fig);

        gt_EndMethod:
            return koma;
        }

        /// <summary>
        /// 指定のマスにある駒を返します。（本将棋用）
        /// </summary>
        /// <param name="masu">マス番号</param>
        /// <param name="logTag">ログ名</param>
        /// <returns>スプライト番号。なければエラー番号。</returns>
        public static RO_Star_Koma Koma_AtMasuNow(SkyConst src_Sky, SyElement masu, Playerside pside)
        {
            RO_Star_Koma koma = null;

            Finger fig = Util_Sky.Fingers_AtMasuNow(src_Sky, masu).ToFirst();

            if (Fingers.Error_1 == fig)
            {
                // 指定の升には駒がない。
                goto gt_EndMethod;
            }

            koma = Util_Koma.FromFinger(src_Sky, fig);
            if(koma.Pside!=pside)
            {
                // サイドが異なる
                koma = null;
                goto gt_EndMethod;
            }

        gt_EndMethod:
            return koma;
        }

        /// <summary>
        /// 指定のマスにある駒を返します。（本将棋用）
        /// </summary>
        /// <param name="masu">マス番号</param>
        /// <param name="logTag">ログ名</param>
        /// <returns>スプライト番号。なければエラー番号。</returns>
        public static RO_Star_Koma Koma_AtMasuNow(SkyConst src_Sky, SyElement masu, Playerside pside, Ks14 syurui)
        {
            RO_Star_Koma koma = null;

            Finger fig = Util_Sky.Fingers_AtMasuNow(src_Sky, masu).ToFirst();

            if (Fingers.Error_1 == fig)
            {
                // 指定の升には駒がない。
                goto gt_EndMethod;
            }

            koma = Util_Koma.FromFinger(src_Sky, fig);
            if (koma.Pside != pside || koma.Syurui!=syurui)
            {
                // サイド または駒の種類が異なる
                koma = null;
                goto gt_EndMethod;
            }

        gt_EndMethod:
            return koma;
        }

        /// <summary>
        /// 指定のマスにあるスプライトを返します。（本将棋用）
        /// </summary>
        /// <param name="masu">マス番号</param>
        /// <param name="logTag">ログ名</param>
        /// <returns>スプライト番号。なければエラー番号。</returns>
        public static Fingers Fingers_AtMasuNow(SkyConst src_Sky, SyElement masu)
        {
            Fingers found = new Fingers();

            foreach (Finger finger in Finger_Honshogi.Items_KomaOnly)
            {
                RO_Star_Koma koma = Util_Koma.FromFinger(src_Sky, finger);

                    if (koma.Masu == masu)
                    {
                        found.Add(finger);
                    }
            }

            return found;
        }

        /// <summary>
        /// 指定の筋にあるスプライトを返します。（本将棋用）
        /// </summary>
        /// <param name="suji">筋番号1～9</param>
        /// <returns></returns>
        public static Fingers Fingers_InSuji(SkyConst src_Sky, int suji)
        {
            Fingers found = new Fingers();

            foreach (Finger finger in Finger_Honshogi.Items_KomaOnly)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).Now);

                int suji2;
                if(Util_MasuNum.MasuToSuji(koma.Masu, out suji2))
                {
                    if (suji2 == suji)
                    {
                        found.Add(finger);
                    }
                }
            }

            return found;
        }

        /// <summary>
        /// 指定の場所にある駒を返します。
        /// 
        ///         先後は見ますが、将棋盤限定です。
        /// 
        /// </summary>
        /// <param name="okiba">置き場</param>
        /// <param name="masu">筋、段</param>
        /// <param name="uc_Main">メインパネル</param>
        /// <returns>駒。無ければヌル。</returns>
        public static Finger Finger_AtMasuNow_Shogiban(SkyConst src_Sky, Playerside pside, SyElement masu)
        {
            Finger foundKoma = Fingers.Error_1;

            foreach (Finger finger in Finger_Honshogi.Items_KomaOnly)
            {

                Starlight sl = src_Sky.StarlightIndexOf(finger);

                RO_Star_Koma koma = Util_Koma.AsKoma(sl.Now);

                int suji1;
                int suji2;
                int dan1;
                int dan2;
                Util_MasuNum.MasuToSuji(koma.Masu, out suji1);
                Util_MasuNum.MasuToSuji(masu, out suji2);
                Util_MasuNum.MasuToDan(koma.Masu, out dan1);
                Util_MasuNum.MasuToDan(masu, out dan2);

                    // 先後は見ますが、将棋盤限定です。
                    if (
                        koma.Pside == pside
                        && Util_Masu.GetOkiba(koma.Masu) == Okiba.ShogiBan
                        && suji1 == suji2
                        && dan1 == dan2
                        )
                    {
                        foundKoma = finger;
                        break;
                    }

            }

            return foundKoma;
        }

        /// <summary>
        /// 指定した置き場にある駒のハンドルを返します。
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="okiba"></param>
        /// <returns></returns>
        public static Fingers Fingers_ByOkibaNow(SkyConst src_Sky, Okiba okiba)
        {
            Fingers komas = new Fingers();

            foreach (Finger figKoma in Finger_Honshogi.Items_KomaOnly)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);


                    if (okiba == Util_Masu.GetOkiba(koma.Masu))
                    {
                        komas.Add(figKoma);
                    }
            }

            return komas;
        }

        /// <summary>
        /// 駒のハンドルを返します。
        /// </summary>
        /// <param name="pside"></param>
        /// <param name="hKomas"></param>
        /// <returns></returns>
        public static Fingers Fingers_ByPsideNow(SkyConst src_Sky,Playerside pside)
        {
            Fingers fingers = new Fingers();

            src_Sky.Foreach_Starlights((Finger finger, Starlight ds, ref bool toBreak) =>
            {

                RO_Star_Koma koma = Util_Koma.AsKoma(ds.Now);

                    if (pside == koma.Pside)
                    {
                        fingers.Add(finger);
                    }

            });

            return fingers;
        }

        /// <summary>
        /// 含まれるか判定。
        /// </summary>
        /// <param name="masus"></param>
        /// <returns></returns>
        public static bool ExistsIn(Starlight sl, SySet<SyElement> masus, SkyConst src_Sky)
        {
            bool matched = false;

            foreach (SyElement masu in masus.Elements)
            {
                RO_Star_Koma koma = Util_Koma.AsKoma(sl.Now);


                Finger finger = Util_Sky.Finger_AtMasuNow_Shogiban(src_Sky, koma.Pside, masu);

                if (
                    finger != Fingers.Error_1  //2014-07-21 先後も見るように追記。
                    && koma.Masu == masu
                    )
                {
                    matched = true;
                    break;
                }

            }

            return matched;
        }

        /// <summary>
        /// 成ケース
        /// </summary>
        /// <returns></returns>
        public static Ks14 ToNariCase(RO_MotionlessStarlight ms)
        {
            Ks14 result;

            RO_Star_Koma koma = Util_Koma.AsKoma(ms.Now);

                result = KomaSyurui14Array.NariCaseHandle[(int)Haiyaku184Array.Syurui(koma.Haiyaku)];

            return result;
        }

        /// <summary>
        /// 相手陣に入っていれば真。
        /// 
        ///         後手は 7,8,9 段。
        ///         先手は 1,2,3 段。
        /// </summary>
        /// <returns></returns>
        public static bool InAitejin(Starlight ms)
        {
            bool result;

            RO_Star_Koma koma = Util_Koma.AsKoma(ms.Now);

            int dan;
            Util_MasuNum.MasuToDan(koma.Masu, out dan);

                result = (Util_Sky.IsGote(ms) && 7 <= dan) || (Util_Sky.IsSente(ms) && dan <= 3);

            return result;
        }

        /// <summary>
        /// 成り
        /// </summary>
        public static bool IsNari(Starlight ms)
        {
            bool result;

            RO_Star_Koma koma = Util_Koma.AsKoma(ms.Now);

                result = KomaSyurui14Array.FlagNari[(int)Haiyaku184Array.Syurui(koma.Haiyaku)];

            return result;
        }

        /// <summary>
        /// 不成
        /// </summary>
        public static bool IsFunari(RO_MotionlessStarlight ms)
        {
            bool result;

            RO_Star_Koma koma = Util_Koma.AsKoma(ms.Now);

                result = !KomaSyurui14Array.FlagNari[(int)Haiyaku184Array.Syurui(koma.Haiyaku)];

            return result;
        }

        public static bool IsNareruKoma(Starlight ms)
        {
            bool result;

            RO_Star_Koma koma = Util_Koma.AsKoma(ms.Now);

                result = KomaSyurui14Array.FlagNareruKoma[(int)Haiyaku184Array.Syurui(koma.Haiyaku)];


            return result;
        }

        /// <summary>
        /// 外字を利用した、デバッグ用の駒の名前１文字だぜ☆
        /// </summary>
        /// <returns></returns>
        public static char ToGaiji(RO_MotionlessStarlight ms)
        {
            char result;

            RO_Star_Koma koma = Util_Koma.AsKoma(ms.Now);

                result = KomaSyurui14Array.ToGaiji(Haiyaku184Array.Syurui(koma.Haiyaku), koma.Pside);

            return result;
        }

        /// <summary>
        /// 不一致判定：　先後、駒種類  が、自分と同じものが　＜ひとつもない＞
        /// </summary>
        /// <returns></returns>
        public static bool NeverOnaji(Starlight ms, SkyConst src_Sky, params Fingers[] komaGroupArgs)
        {
            bool unmatched = true;

            foreach (Fingers komaGroup in komaGroupArgs)
            {
                foreach (Finger figKoma in komaGroup.Items)
                {
                    RO_Star_Koma koma1 = Util_Koma.AsKoma(ms.Now);
                    RO_Star_Koma koma2 = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(figKoma).Now);



                    if (
                            koma1.Pside == koma2.Pside // 誰のものか
                        && Haiyaku184Array.Syurui(koma1.Haiyaku) == Haiyaku184Array.Syurui(koma2.Haiyaku) // 駒の種類は
                        )
                    {
                        // １つでも一致するものがあれば、終了します。
                        unmatched = false;
                        goto gt_EndLoop;
                    }
                }

            }
        gt_EndLoop:

            return unmatched;
        }

        /// <summary>
        /// 一致判定：　先後、駒種類  が、自分と同じ
        /// </summary>
        /// <returns></returns>
        public static Fingers Matches(Starlight ms, SkyConst src_Sky, params Fingers[] fingersGroupArgs)
        {
            Fingers fingers = new Fingers();

            foreach (Fingers fingers2 in fingersGroupArgs)
            {
                foreach (Finger finger in fingers2.Items)
                {
                    RO_Star_Koma koma1 = Util_Koma.AsKoma(ms.Now);
                    RO_Star_Koma koma2 = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).Now);


                        if (
                                koma1.Pside == koma2.Pside // 誰のものか
                            && Haiyaku184Array.Syurui(koma1.Haiyaku) == Haiyaku184Array.Syurui(koma2.Haiyaku) // 駒の種類は
                            )
                        {
                            // 一致するもの
                            fingers.Add(finger);
                        }
                }
            }

            return fingers;
        }

        public static Json_Val ToJsonVal(Starlight ms)
        {
            Json_Obj obj = new Json_Obj();


            RO_Star_Koma koma = Util_Koma.AsKoma(ms.Now);


                // プレイヤーサイド
                obj.Add(new Json_Prop("pside", Converter04.Pside_ToStr(koma.Pside)));// ▲△

                // マス  
                obj.Add(new Json_Prop("masu", Util_Masu.AsMasuNumber(koma.Masu)));// ▲△

                // 駒の種類。歩、香、桂…。
                obj.Add(new Json_Prop("syurui", Converter04.Syurui_ToStrIchimoji(Haiyaku184Array.Syurui(koma.Haiyaku))));// ▲△

            return obj;
        }

        /// <summary>
        /// 駒台の上にあれば真。
        /// </summary>
        /// <returns></returns>
        public static bool OnKomadai(RO_MotionlessStarlight ms)
        {
            bool result;

            RO_Star_Koma koma = Util_Koma.AsKoma(ms.Now);

                result = (Okiba.Sente_Komadai | Okiba.Gote_Komadai).HasFlag(
                    Util_Masu.Masu_ToOkiba(koma.Masu));

            return result;
        }

        /// <summary>
        /// 先後一致判定。
        /// </summary>
        /// <param name="ms2"></param>
        /// <returns></returns>
        public static bool MatchPside(RO_MotionlessStarlight ms1, RO_MotionlessStarlight ms2)
        {
            bool result;

            RO_Star_Koma koma1 = Util_Koma.AsKoma(ms1.Now);
            RO_Star_Koma koma2 = Util_Koma.AsKoma(ms2.Now);


                result = koma1.Pside == koma2.Pside;

            return result;
        }

        /// <summary>
        /// 先手
        /// </summary>
        /// <returns></returns>
        public static bool IsSente(Starlight ms)
        {
            bool result;

            RO_Star_Koma koma = Util_Koma.AsKoma(ms.Now);

                result = Playerside.P1 == koma.Pside;

            return result;
        }

        /// <summary>
        /// 後手
        /// </summary>
        /// <returns></returns>
        public static bool IsGote(Starlight ms)
        {
            bool result;

            RO_Star_Koma koma = Util_Koma.AsKoma(ms.Now);

                result = Playerside.P2 == koma.Pside;

            return result;
        }


        /// <summary>
        /// ログが多くなるので、１行で出力されるようにします。
        /// </summary>
        /// <returns></returns>
        public static Json_Val ToJsonVal(SkyConst src_Sky)
        {
            Json_Obj obj = new Json_Obj();

            Json_Arr arr = new Json_Arr();
            src_Sky.Foreach_Starlights((Finger finger, Starlight light, ref bool toBreak) =>
            {
                if (null != light)
                {
                    arr.Add(Util_Sky.ToJsonVal(light));
                }
            });

            obj.Add(new Json_Prop("sprite", arr));

            return obj;
        }

        /// <summary>
        /// “打” ＜アクション時＞
        /// </summary>
        /// <returns></returns>
        public static bool IsDaAction(ShootingStarlightable move)
        {
            bool result;

            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);

                result = Okiba.ShogiBan != Util_Masu.GetOkiba(srcKoma.Masu)
                    && Okiba.Empty != Util_Masu.GetOkiba(srcKoma.Masu);//初期配置から移動しても、打にはしません。

            return result;
        }


        /// <summary>
        /// SFEN符号表記。
        /// 
        /// ファイル名にも使えるように、ファイル名に使えない文字を置換します。
        /// </summary>
        /// <returns></returns>
        public static string ToSfenMoveTextForFilename(
            ShootingStarlightable move,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            string moveText = Util_Sky.ToSfenMoveText(move);

            moveText = moveText.Replace('*', '＊');//SFENの打記号の「*」は、ファイルの文字名に使えないので。


            return moveText;
        }

        /// <summary>
        /// SFEN符号表記。
        /// </summary>
        /// <returns></returns>
        public static string ToSfenMoveText(
            ShootingStarlightable move,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
            )
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
                RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);



                int srcDan;
                if (!Util_MasuNum.MasuToDan(srcKoma.Masu, out srcDan))
                {
                    throw new Exception($"指定の元マス[{srcKoma.Masu}]は、段に変換できません。　：　{memberName}.{sourceFilePath}.{sourceLineNumber}");
                }

                int dan;
                if (!Util_MasuNum.MasuToDan(dstKoma.Masu, out dan))
                {
                    throw new Exception($"指定の先マス[{dstKoma.Masu}]は、段に変換できません。　：　{memberName}.{sourceFilePath}.{sourceLineNumber}");
                }


                if (Util_Sky.IsDaAction(move))
                {
                    // 打でした。
                    //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    // (自)筋・(自)段は書かずに、「P*」といった表記で埋めます。
                    sb.Append(KomaSyurui14Array.SfenDa[(int)Haiyaku184Array.Syurui(srcKoma.Haiyaku)]);
                    sb.Append("*");
                }
                else
                {
                    //------------------------------------------------------------
                    // (自)筋
                    //------------------------------------------------------------
                    string strSrcSuji;
                    int srcSuji;
                    if (Util_MasuNum.MasuToSuji(srcKoma.Masu, out srcSuji))
                    {
                        strSrcSuji = srcSuji.ToString();
                    }
                    else
                    {
                        strSrcSuji = "Ｎ筋";//エラー表現
                    }
                    sb.Append(strSrcSuji);

                    //------------------------------------------------------------
                    // (自)段
                    //------------------------------------------------------------
                    string strSrcDan2;
                    int srcDan2;
                    if (Util_MasuNum.MasuToDan(srcKoma.Masu, out srcDan2))
                    {
                        strSrcDan2 = Converter04.Int_ToAlphabet(srcDan2);
                    }
                    else
                    {
                        strSrcDan2 = "Ｎ段";//エラー表現
                    }
                    sb.Append(strSrcDan2);
                }

                //------------------------------------------------------------
                // (至)筋
                //------------------------------------------------------------
                string strSuji;
                int suji2;
                if (Util_MasuNum.MasuToSuji(dstKoma.Masu, out suji2))
                {
                    strSuji = suji2.ToString();
                }
                else
                {
                    strSuji = "Ｎ筋";//エラー表現
                }
                sb.Append(strSuji);


                //------------------------------------------------------------
                // (至)段
                //------------------------------------------------------------
                string strDan;
                int dan2;
                if (Util_MasuNum.MasuToDan(dstKoma.Masu, out dan2))
                {
                    strDan = Converter04.Int_ToAlphabet(dan2);
                }
                else
                {
                    strDan = "Ｎ段";//エラー表現
                }
                sb.Append(strDan);


                //------------------------------------------------------------
                // 成
                //------------------------------------------------------------
                if (Util_Sky.IsNattaMove(move))
                {
                    sb.Append("+");
                }
            }
            catch (Exception e)
            {
                sb.Append(e.Message);//FIXME:
            }

            return sb.ToString();
        }

        public static bool isEnableSfen(ShootingStarlightable move)
        {
            bool enable = true;

            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);


            int srcDan;
            if (!Util_MasuNum.MasuToDan(srcKoma.Masu, out srcDan))
            {
                enable = false;
            }

            int dan;
            if (!Util_MasuNum.MasuToDan(dstKoma.Masu, out dan))
            {
                enable = false;
            }

            return enable;
        }

        /// <summary>
        /// 元位置。
        /// </summary>
        /// <returns></returns>
        public static ShootingStarlightable Src(ShootingStarlightable move)
        {
            RO_ShootingStarlight result;


            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);


                result = new RO_ShootingStarlight(
                    //move.Finger,//共通

                    new RO_Star_Koma(
                        dstKoma.Pside,
                        Masu_Honshogi.Error, // ソースのソースは未定義。
                        Ks14.H00_Null
                    ),

                    // ソースの目的地はソース
                    new RO_Star_Koma(
                        dstKoma.Pside,
                        srcKoma.Masu,
                        srcKoma.Syurui
                    ),

                    Ks14.H00_Null
                );

            return result;
        }

        public static RO_ShootingStarlight New(
            //Finger finger,

            Starlightable longTimeAgo,
            Starlightable now,

            Ks14 tottaKomaSyurui
        )
        {
            return new RO_ShootingStarlight(longTimeAgo, now, tottaKomaSyurui);//finger,
        }

        /// <summary>
        /// SFEN符号表記。（取った駒付き）
        /// </summary>
        /// <returns></returns>
        public static string ToSfenText_TottaKomaSyurui(RO_ShootingStarlight ss)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Util_Sky.ToSfenMoveText(ss));
            if (Ks14.H00_Null != (Ks14)ss.FoodKomaSyurui)
            {
                sb.Append("(");
                sb.Append(ss.FoodKomaSyurui);
                sb.Append(")");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 成った
        /// </summary>
        /// <returns></returns>
        public static bool IsNattaMove(ShootingStarlightable move)
        {
            // 元種類が不成、現種類が成　の場合のみ真。
            bool natta = true;


            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);


                // 成立しない条件を１つでも満たしていれば、偽　確定。
                if (
                    Kh185.n000_未設定 == srcKoma.Haiyaku
                    //Ks14.H00_Null == Haiyaku184Array.Syurui[(int)this.SrcHaiyaku]
                    ||
                    Kh185.n000_未設定 == dstKoma.Haiyaku
                    //Ks14.H15_ErrorKoma == Haiyaku184Array.Syurui[(int)this.Haiyaku]
                    ||
                    KomaSyurui14Array.FlagNari[(int)Haiyaku184Array.Syurui(srcKoma.Haiyaku)]
                    ||
                    !KomaSyurui14Array.FlagNari[(int)Haiyaku184Array.Syurui(dstKoma.Haiyaku)]
                    )
                {
                    natta = false;
                }

            return natta;
        }

        /// <summary>
        /// 移動前と、移動後の場所が異なっていれば真。
        /// </summary>
        /// <returns></returns>
        public static bool DoneMove(RO_ShootingStarlight ss)
        {
            bool result;

            RO_Star_Koma koma1 = Util_Koma.AsKoma(ss.Now);
            RO_Star_Koma koma2 = Util_Koma.AsKoma(Util_Sky.Src(ss).Now);

            result = Util_Masu.AsMasuNumber(koma1.Masu) != Util_Masu.AsMasuNumber(koma2.Masu);

            return result;
        }

        /// <summary>
        /// 駒の移動可能升
        /// 
        /// FIXME: ポテンシャルなので、貫通している。
        /// 
        /// </summary>
        /// <param name="light"></param>
        /// <returns></returns>
        public static SySet<SyElement> KomaKidou_Potential(Finger finger, SkyConst src_Sky)
        {
            SySet<SyElement> result;

            RO_Star_Koma koma = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(finger).Now);

            //
            // 貫通してないか？
            //
            result = Rule01_PotentialMove_15Array.ItemMethods[(int)koma.Syurui](koma.Pside, koma.Masu);

            return result;
        }

        public static SkyConst New_Komabukuro()
        {
            SkyBuffer buffer_Sky = new SkyBuffer();
            buffer_Sky.Clear();

            Finger finger = 0;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight(/*finger,*/ new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro01, Ks14.H06_Oh)));// Kh185.n051_底奇王
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro02, Ks14.H06_Oh)));// Kh185.n051_底奇王
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro03, Ks14.H07_Hisya)));// Kh185.n061_飛
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro04, Ks14.H07_Hisya)));// Kh185.n061_飛
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro05, Ks14.H08_Kaku)));// Kh185.n072_奇角
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro06, Ks14.H08_Kaku)));// Kh185.n072_奇角
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro07, Ks14.H05_Kin)));// Kh185.n038_底偶金
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro08, Ks14.H05_Kin)));// Kh185.n038_底偶金
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro09, Ks14.H05_Kin)));// Kh185.n038_底偶金
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro10, Ks14.H05_Kin)));// Kh185.n038_底偶金
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro11, Ks14.H04_Gin)));// Kh185.n023_底奇銀
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro12, Ks14.H04_Gin)));// Kh185.n023_底奇銀
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro13, Ks14.H04_Gin)));// Kh185.n023_底奇銀
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro14, Ks14.H04_Gin)));// Kh185.n023_底奇銀
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro15, Ks14.H03_Kei)));// Kh185.n007_金桂
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro16, Ks14.H03_Kei)));// Kh185.n007_金桂
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro17, Ks14.H03_Kei)));// Kh185.n007_金桂
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro18, Ks14.H03_Kei)));// Kh185.n007_金桂
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro19, Ks14.H02_Kyo)));// Kh185.n002_香
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro20, Ks14.H02_Kyo)));// Kh185.n002_香
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro21, Ks14.H02_Kyo)));// Kh185.n002_香
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro22, Ks14.H02_Kyo)));// Kh185.n002_香
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro23, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro24, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro25, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro26, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro27, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro28, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro29, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro30, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.fukuro31, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro32, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro33, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro34, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro35, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro36, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro37, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro38, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro39, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            buffer_Sky.AddOverwriteStarlight(finger, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.fukuro40, Ks14.H01_Fu)));// Kh185.n001_歩
            finger++;

            // 以上、全40駒。
            Debug.Assert(buffer_Sky.Starlights.Count == 40);

            return new SkyConst( buffer_Sky);
        }

        /// <summary>
        /// 駒を、平手の初期配置に並べます。
        /// </summary>
        public static SkyConst New_Hirate()
        {
            SkyBuffer dst_Sky = new SkyBuffer();
            Finger figKoma;

            figKoma = Finger_Honshogi.SenteOh;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight(/*figKoma,*/ new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban59_５九, Ks14.H06_Oh)));//先手王
            figKoma = (int)Finger_Honshogi.GoteOh;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban51_５一, Ks14.H06_Oh)));//後手王

            figKoma = (int)Finger_Honshogi.Hi1;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban28_２八, Ks14.H07_Hisya)));//飛
            figKoma = (int)Finger_Honshogi.Hi2;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban82_８二, Ks14.H07_Hisya)));

            figKoma = (int)Finger_Honshogi.Kaku1;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban88_８八, Ks14.H08_Kaku)));//角
            figKoma = (int)Finger_Honshogi.Kaku2;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban22_２二, Ks14.H08_Kaku)));

            figKoma = (int)Finger_Honshogi.Kin1;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban49_４九, Ks14.H05_Kin)));//金
            figKoma = (int)Finger_Honshogi.Kin2;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban69_６九, Ks14.H05_Kin)));
            figKoma = (int)Finger_Honshogi.Kin3;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban41_４一, Ks14.H05_Kin)));
            figKoma = (int)Finger_Honshogi.Kin4;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban61_６一, Ks14.H05_Kin)));

            figKoma = (int)Finger_Honshogi.Gin1;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban39_３九, Ks14.H04_Gin)));//銀
            figKoma = (int)Finger_Honshogi.Gin2;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban79_７九, Ks14.H04_Gin)));
            figKoma = (int)Finger_Honshogi.Gin3;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban31_３一, Ks14.H04_Gin)));
            figKoma = (int)Finger_Honshogi.Gin4;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban71_７一, Ks14.H04_Gin)));

            figKoma = (int)Finger_Honshogi.Kei1;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban29_２九, Ks14.H03_Kei)));//桂
            figKoma = (int)Finger_Honshogi.Kei2;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban89_８九, Ks14.H03_Kei)));
            figKoma = (int)Finger_Honshogi.Kei3;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban21_２一, Ks14.H03_Kei)));
            figKoma = (int)Finger_Honshogi.Kei4;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban81_８一, Ks14.H03_Kei)));

            figKoma = (int)Finger_Honshogi.Kyo1;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban19_１九, Ks14.H02_Kyo)));//香
            figKoma = (int)Finger_Honshogi.Kyo2;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban99_９九, Ks14.H02_Kyo)));
            figKoma = (int)Finger_Honshogi.Kyo3;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban11_１一, Ks14.H02_Kyo)));
            figKoma = (int)Finger_Honshogi.Kyo4;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban91_９一, Ks14.H02_Kyo)));

            figKoma = (int)Finger_Honshogi.Fu1;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban17_１七, Ks14.H01_Fu)));//歩
            figKoma = (int)Finger_Honshogi.Fu2;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban27_２七, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu3;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban37_３七, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu4;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban47_４七, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu5;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban57_５七, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu6;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban67_６七, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu7;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban77_７七, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu8;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban87_８七, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu9;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P1, Masu_Honshogi.ban97_９七, Ks14.H01_Fu)));

            figKoma = (int)Finger_Honshogi.Fu10;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban13_１三, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu11;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban23_２三, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu12;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban33_３三, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu13;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban43_４三, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu14;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban53_５三, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu15;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban63_６三, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu16;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban73_７三, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu17;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban83_８三, Ks14.H01_Fu)));
            figKoma = (int)Finger_Honshogi.Fu18;
            dst_Sky.AddOverwriteStarlight(figKoma, new RO_MotionlessStarlight( new RO_Star_Koma(Playerside.P2, Masu_Honshogi.ban93_９三, Ks14.H01_Fu)));


            //LarabeLogger.GetInstance().WriteAddMemo(logTag, "平手局面にセットしたぜ☆");

            return new SkyConst( dst_Sky);
        }


        public static void Assert_Honshogi(SkyConst src_Sky)
        {
            Debug.Assert(src_Sky.Count == 40, "siteiSky.Starlights.Count=[" + src_Sky.Count + "]");//将棋の駒の数

            ////デバッグ
            //{
            //    StringBuilder sb = new StringBuilder();

            //    for (int i = 0; i < 40; i++)
            //    {
            //        sb.Append("駒" + i + ".種類=[" + ((RO_Star_KomaKs)siteiSky.StarlightIndexOf(i).Now).Syurui + "]\n");
            //    }

            //    MessageBox.Show(sb.ToString());
            //}


            // 王
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(0).Now).Syurui == Ks14.H06_Oh, "駒0.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(0).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(1).Now).Syurui == Ks14.H06_Oh, "駒1.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(1).Now).Syurui + "]");

            // 飛車
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(2).Now).Syurui == Ks14.H07_Hisya || ((RO_Star_Koma)src_Sky.StarlightIndexOf(2).Now).Syurui == Ks14.H09_Ryu, "駒2.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(2).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(3).Now).Syurui == Ks14.H07_Hisya || ((RO_Star_Koma)src_Sky.StarlightIndexOf(3).Now).Syurui == Ks14.H09_Ryu, "駒3.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(3).Now).Syurui + "]");

            // 角
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(4).Now).Syurui == Ks14.H08_Kaku || ((RO_Star_Koma)src_Sky.StarlightIndexOf(4).Now).Syurui == Ks14.H10_Uma, "駒4.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(4).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(5).Now).Syurui == Ks14.H08_Kaku || ((RO_Star_Koma)src_Sky.StarlightIndexOf(5).Now).Syurui == Ks14.H10_Uma, "駒5.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(5).Now).Syurui + "]");

            // 金
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(6).Now).Syurui == Ks14.H05_Kin, "駒6.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(6).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(7).Now).Syurui == Ks14.H05_Kin, "駒7.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(7).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(8).Now).Syurui == Ks14.H05_Kin, "駒8.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(8).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(9).Now).Syurui == Ks14.H05_Kin, "駒9.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(9).Now).Syurui + "]");

            // 銀
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(10).Now).Syurui == Ks14.H04_Gin || ((RO_Star_Koma)src_Sky.StarlightIndexOf(10).Now).Syurui == Ks14.H14_NariGin, "駒10.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(10).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(11).Now).Syurui == Ks14.H04_Gin || ((RO_Star_Koma)src_Sky.StarlightIndexOf(11).Now).Syurui == Ks14.H14_NariGin, "駒11.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(11).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(12).Now).Syurui == Ks14.H04_Gin || ((RO_Star_Koma)src_Sky.StarlightIndexOf(12).Now).Syurui == Ks14.H14_NariGin, "駒12.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(12).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(13).Now).Syurui == Ks14.H04_Gin || ((RO_Star_Koma)src_Sky.StarlightIndexOf(13).Now).Syurui == Ks14.H14_NariGin, "駒13.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(13).Now).Syurui + "]");

            // 桂
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(14).Now).Syurui == Ks14.H03_Kei || ((RO_Star_Koma)src_Sky.StarlightIndexOf(14).Now).Syurui == Ks14.H13_NariKei, "駒14.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(14).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(15).Now).Syurui == Ks14.H03_Kei || ((RO_Star_Koma)src_Sky.StarlightIndexOf(15).Now).Syurui == Ks14.H13_NariKei, "駒15.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(15).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(16).Now).Syurui == Ks14.H03_Kei || ((RO_Star_Koma)src_Sky.StarlightIndexOf(16).Now).Syurui == Ks14.H13_NariKei, "駒16.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(16).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(17).Now).Syurui == Ks14.H03_Kei || ((RO_Star_Koma)src_Sky.StarlightIndexOf(17).Now).Syurui == Ks14.H13_NariKei, "駒17.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(17).Now).Syurui + "]");

            // 香
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(18).Now).Syurui == Ks14.H02_Kyo || ((RO_Star_Koma)src_Sky.StarlightIndexOf(18).Now).Syurui == Ks14.H12_NariKyo, "駒18.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(18).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(19).Now).Syurui == Ks14.H02_Kyo || ((RO_Star_Koma)src_Sky.StarlightIndexOf(19).Now).Syurui == Ks14.H12_NariKyo, "駒19.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(19).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(20).Now).Syurui == Ks14.H02_Kyo || ((RO_Star_Koma)src_Sky.StarlightIndexOf(20).Now).Syurui == Ks14.H12_NariKyo, "駒20.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(20).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(21).Now).Syurui == Ks14.H02_Kyo || ((RO_Star_Koma)src_Sky.StarlightIndexOf(21).Now).Syurui == Ks14.H12_NariKyo, "駒21.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(21).Now).Syurui + "]");

            // 歩
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(22).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(22).Now).Syurui == Ks14.H11_Tokin, "駒22.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(22).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(23).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(23).Now).Syurui == Ks14.H11_Tokin, "駒23.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(23).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(24).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(24).Now).Syurui == Ks14.H11_Tokin, "駒24.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(24).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(25).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(25).Now).Syurui == Ks14.H11_Tokin, "駒25.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(25).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(26).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(26).Now).Syurui == Ks14.H11_Tokin, "駒26.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(26).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(27).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(27).Now).Syurui == Ks14.H11_Tokin, "駒27.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(27).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(28).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(28).Now).Syurui == Ks14.H11_Tokin, "駒28.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(28).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(29).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(29).Now).Syurui == Ks14.H11_Tokin, "駒29.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(29).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(30).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(30).Now).Syurui == Ks14.H11_Tokin, "駒30.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(30).Now).Syurui + "]");

            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(31).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(31).Now).Syurui == Ks14.H11_Tokin, "駒31.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(31).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(32).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(32).Now).Syurui == Ks14.H11_Tokin, "駒32.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(32).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(33).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(33).Now).Syurui == Ks14.H11_Tokin, "駒33.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(33).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(34).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(34).Now).Syurui == Ks14.H11_Tokin, "駒34.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(34).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(35).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(35).Now).Syurui == Ks14.H11_Tokin, "駒35.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(35).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(36).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(36).Now).Syurui == Ks14.H11_Tokin, "駒36.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(36).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(37).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(37).Now).Syurui == Ks14.H11_Tokin, "駒37.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(37).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(38).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(38).Now).Syurui == Ks14.H11_Tokin, "駒38.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(38).Now).Syurui + "]");
            Debug.Assert(((RO_Star_Koma)src_Sky.StarlightIndexOf(39).Now).Syurui == Ks14.H01_Fu || ((RO_Star_Koma)src_Sky.StarlightIndexOf(39).Now).Syurui == Ks14.H11_Tokin, "駒39.種類=[" + ((RO_Star_Koma)src_Sky.StarlightIndexOf(39).Now).Syurui + "]");



            for (int i = 0; i < 40; i++)
            {
                RO_Star_Koma koma = (RO_Star_Koma)src_Sky.StarlightIndexOf(0).Now;
                Kh185 haiyaku = koma.Haiyaku;

                if (Okiba.ShogiBan == Util_Masu.Masu_ToOkiba(koma.Masu))
                {
                    Debug.Assert(!Util_Haiyaku184.IsKomabukuro(haiyaku), "将棋盤の上に、配役：駒袋　があるのはおかしい。");
                }


                //if(
                //    haiyaku==Kh185.n164_歩打
                //    )
                //{
                //}
                //koma.Syurui
                //Debug.Assert((.Syurui == Ks14.H06_Oh, "駒0.種類=[" + ((RO_Star_Koma)siteiSky.StarlightIndexOf(0).Now).Syurui + "]");
                //sb.Append("駒" + i + ".種類=[" + ((RO_Star_KomaKs)siteiSky.StarlightIndexOf(i).Now).Syurui + "]\n");
            }


        }

    }
}
