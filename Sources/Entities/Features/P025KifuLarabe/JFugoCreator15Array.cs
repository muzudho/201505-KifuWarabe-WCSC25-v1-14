namespace Grayscale.Kifuwarazusa.Entities.Features
{


    /// <summary>
    /// J符号作成１５個。
    /// 
    /// ５二金右引　のような文字を作ります。
    /// </summary>
    public abstract class JFugoCreator15Array
    {
        public delegate FugoJ DELEGATE_CreateJFugo(ShootingStarlightable teMove, KyokumenWrapper kWrap);

        public static DELEGATE_CreateJFugo[] ItemMethods
        {
            get
            {
                return JFugoCreator15Array.itemMethods;
            }
        }
        private static DELEGATE_CreateJFugo[] itemMethods;

        static JFugoCreator15Array()
        {
            // 駒種類ハンドルに対応
            JFugoCreator15Array.itemMethods = new DELEGATE_CreateJFugo[]{
                JFugoCreator15Array.CreateNullKoma,// null,//[0]
                JFugoCreator15Array.CreateFu,//[1]
                JFugoCreator15Array.CreateKyo,
                JFugoCreator15Array.CreateKei,
                JFugoCreator15Array.CreateGin,
                JFugoCreator15Array.CreateKin,
                JFugoCreator15Array.CreateOh,
                JFugoCreator15Array.CreateHisya,
                JFugoCreator15Array.CreateKaku,
                JFugoCreator15Array.CreateRyu,
                JFugoCreator15Array.CreateUma,//[10]
                JFugoCreator15Array.CreateTokin,
                JFugoCreator15Array.CreateNariKyo,
                JFugoCreator15Array.CreateNariKei,
                JFugoCreator15Array.CreateNariGin,
                JFugoCreator15Array.CreateErrorKoma,//[15]
            };
        }


        public static FugoJ CreateNullKoma(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ result;

            //************************************************************
            // エラー
            //************************************************************
            MigiHidari migiHidari = MigiHidari.No_Print;
            AgaruHiku agaruHiku = AgaruHiku.No_Print;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。


            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);

            //----------
            // TODO: 移動前の駒が成る前かどうか
            //----------
            nari = NariNarazu.CTRL_SONOMAMA;

            result = new FugoJ(
                Haiyaku184Array.Syurui(srcKoma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return result;
        }

        /// <summary>
        /// 歩のJ符号を作ります。
        /// </summary>
        /// <param name="move">移動先、移動元、両方のマス番号</param>
        /// <returns></returns>
        public static FugoJ CreateFu(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ result;

            SkyConst src_Sky = kWrap.ToKyokumenConst;

            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);


            //************************************************************
            // 歩
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
            //----------
            //┌─┬─┬─┐
            //│  │  │  │
            //├─┼─┼─┤
            //│  │至│  │
            //├─┼─┼─┤
            //│  │Ｅ│  │
            //└─┴─┴─┘
            SySet<SyElement> srcE = KomanoKidou.SrcIppo_戻引(dstKoma.Pside, dstKoma.Masu);
            ShootingStarlightable src = Util_Sky.Src(move);

            //----------
            // 棋譜の現局面：競合駒
            //----------


            Fingers kmE = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcE);

            if (Util_Sky.IsDaAction(move))
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky.ExistsIn(src, srcE, src_Sky))
            {
                // Ｅにいた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // 歩に右左、引上はありません。
                migiHidari = MigiHidari.No_Print;
                agaruHiku = AgaruHiku.No_Print;
            }
            else
            {
                // どこからか飛んできた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                // 歩に右左、引上はありません。
                migiHidari = MigiHidari.No_Print;
                agaruHiku = AgaruHiku.No_Print;
            }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky.NeverOnaji(move, src_Sky, kmE)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == Util_Sky.IsNari(move) && !Util_Sky.IsDaAction(move) && Util_Sky.InAitejin(move))
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariNarazu.Narazu;
            }
            else if (Util_Sky.IsNari(move))
            {
                nari = NariNarazu.Nari;
            }
            else
            {
                nari = NariNarazu.CTRL_SONOMAMA;
            }

            result = new FugoJ(
                Haiyaku184Array.Syurui(srcKoma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return result;
        }

        public static FugoJ CreateKyo(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ fugo;

            SkyConst src_Sky = kWrap.ToKyokumenConst;

            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);


            //************************************************************
            // 香
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //  ┌─┬─┬─┐
            //  │  │至│  │
            //  ├─┼─┼─┤
            //  │  │E0│  │
            //  ├─┼─┼─┤
            //  │  │E1│  │
            //  ├─┼─┼─┤
            //  │  │E2│  │
            //  ├─┼─┼─┤
            //  │  │E3│  │
            //  ├─┼─┼─┤
            //  │  │E4│  │
            //  ├─┼─┼─┤
            //  │  │E5│  │
            //  ├─┼─┼─┤
            //  │  │E6│  │
            //  ├─┼─┼─┤
            //  │  │E7│  │
            //  └─┴─┴─┘
            SySet<SyElement> srcE = KomanoKidou.SrcKantu_戻引(dstKoma.Pside, dstKoma.Masu);
            ShootingStarlightable src = Util_Sky.Src(move);

            //----------
            // 競合駒
            //----------


            Fingers kmE = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcE);

            if (Util_Sky.IsDaAction(move))
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky.ExistsIn(src, srcE, src_Sky))
            {
                //----------
                // 移動前はＥだった
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： 香に右はありません。
            // 「左」解除： 香に左はありません。
            // 「上」解除： 香に上はありません。
            // 「引」解除： 香に引はありません。
            // 「寄」解除： 香に寄はありません。

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky.NeverOnaji(move, src_Sky, kmE)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == Util_Sky.IsNari(move) && !Util_Sky.IsDaAction(move) && Util_Sky.InAitejin(move))
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariNarazu.Narazu;
            }
            else if (Util_Sky.IsNari(move))
            {
                nari = NariNarazu.Nari;
            }
            else
            {
                nari = NariNarazu.CTRL_SONOMAMA;
            }

            fugo = new FugoJ(
                Haiyaku184Array.Syurui(srcKoma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static FugoJ CreateKei(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ fugo;

            SkyConst src_Sky = kWrap.ToKyokumenConst;

            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);


            //************************************************************
            // 桂
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス
            //----------
            //┌─┐　┌─┐
            //│  │　│  │
            //├─┼─┼─┤
            //│　│  │  │
            //├─┼─┼─┤
            //│　│至│  │先手から見た図
            //├─┼─┼─┤
            //│　│  │  │
            //├─┼─┼─┤
            //│Ｊ│　│Ｉ│
            //└─┘　└─┘
            SySet<SyElement> srcI = KomanoKidou.SrcKeimatobi_戻跳(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcJ = KomanoKidou.SrcKeimatobi_戻駆(dstKoma.Pside, dstKoma.Masu);
            ShootingStarlightable src = Util_Sky.Src(move);

            //----------
            // 競合駒
            //----------

            Fingers kmI = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcI);
            Fingers kmJ = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcJ);

            if (Util_Sky.IsDaAction(move))
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky.ExistsIn(src, srcI, src_Sky))
            {
                //----------
                // 移動前はＩだった
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky.ExistsIn(src, srcJ, src_Sky))
            {
                //----------
                // 移動前はＪだった
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ｊに競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && Util_Sky.NeverOnaji(move, src_Sky, kmJ)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ｉに競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && Util_Sky.NeverOnaji(move, src_Sky, kmI)) { migiHidari = MigiHidari.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky.NeverOnaji(move, src_Sky, kmI, kmJ)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == Util_Sky.IsNari(move) && !Util_Sky.IsDaAction(move) && Util_Sky.InAitejin(move))
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariNarazu.Narazu;
            }
            else if (Util_Sky.IsNari(move))
            {
                nari = NariNarazu.Nari;
            }
            else
            {
                nari = NariNarazu.CTRL_SONOMAMA;
            }

            fugo = new FugoJ(
                Haiyaku184Array.Syurui(srcKoma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static FugoJ CreateGin(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ fugo;

            SkyConst src_Sky = kWrap.ToKyokumenConst;

            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);

            //************************************************************
            // 銀
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(range masu)
            //----------
            //┌─┬─┬─┐
            //│Ｈ│  │Ｂ│
            //├─┼─┼─┤
            //│　│至│  │先手から見た図
            //├─┼─┼─┤
            //│Ｆ│Ｅ│Ｄ│
            //└─┴─┴─┘
            SySet<SyElement> srcB = KomanoKidou.SrcIppo_戻昇(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcD = KomanoKidou.SrcIppo_戻沈(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcE = KomanoKidou.SrcIppo_戻引(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcF = KomanoKidou.SrcIppo_戻降(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcH = KomanoKidou.SrcIppo_戻浮(dstKoma.Pside, dstKoma.Masu);
            ShootingStarlightable src = Util_Sky.Src(move);

            //----------
            // 競合駒
            //----------

            Fingers kmB = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcB);
            Fingers kmD = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcD);
            Fingers kmE = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcE);
            Fingers kmF = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcF);
            Fingers kmH = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcH);

            if (Util_Sky.IsDaAction(move))
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky.ExistsIn(src, srcB, src_Sky))
            {
                // 移動前はＢだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky.ExistsIn(src, srcD, src_Sky))
            {
                // 移動前はＤだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky.ExistsIn(src, srcE, src_Sky))
            {
                // 移動前はＥだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Sugu;
            }
            else if (Util_Sky.ExistsIn(src, srcF, src_Sky))
            {
                // 移動前はＦだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (Util_Sky.ExistsIn(src, srcH, src_Sky))
            {
                // 移動前はＨだった
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： ①Ｅ、Ｆ、Ｈのどこにも競合駒がなければ。
            //              ②Ｈに競合駒がなく引があるなら。
            //              ③Ｅ、Ｆに競合駒がなく上があるなら。
            if (migiHidari == MigiHidari.Migi && (
                Util_Sky.NeverOnaji(move, src_Sky, kmE, kmF, kmH)
                || (Util_Sky.NeverOnaji(move, src_Sky, kmH) && agaruHiku == AgaruHiku.Hiku)
                || (Util_Sky.NeverOnaji(move, src_Sky, kmE, kmF) && agaruHiku == AgaruHiku.Agaru)
                )) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： ①Ｂ、Ｄ、Ｅのどこにも競合駒がなければ。
            //              ②Ｂに競合駒がなく引があるなら。
            //              ③Ｄ、Ｅに競合駒がなく上があるなら。
            if (migiHidari == MigiHidari.Hidari && (
                Util_Sky.NeverOnaji(move, src_Sky, kmB, kmD, kmE)
                || (Util_Sky.NeverOnaji(move, src_Sky, kmB) && agaruHiku == AgaruHiku.Hiku)
                || (Util_Sky.NeverOnaji(move, src_Sky, kmD, kmE) && agaruHiku == AgaruHiku.Agaru)
                )) { migiHidari = MigiHidari.No_Print; }

            // 「直」解除： Ｄ、Ｆのどちらにも競合駒がなければ。
            if (migiHidari == MigiHidari.Sugu && Util_Sky.NeverOnaji(move, src_Sky, kmD, kmF)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ｂ、Ｈのどこにも競合駒がなければ。また、直があるなら。
            if (agaruHiku == AgaruHiku.Agaru && (Util_Sky.NeverOnaji(move, src_Sky, kmB, kmH) || migiHidari == MigiHidari.Sugu)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： ①Ｂ、Ｄ、Ｅ、Ｆのどこにも競合駒がなければ。
            //              ②Ｄ、Ｅ、Ｆ、Ｈのどこにも競合駒がなければ。
            //              ③Ｄに競合駒がなく、右があるなら。
            //              ④Ｆに競合駒がなく、左があるなら。
            if (agaruHiku == AgaruHiku.Hiku &&
                (
                Util_Sky.NeverOnaji(move, src_Sky, kmB, kmD, kmE, kmF)
                || Util_Sky.NeverOnaji(move, src_Sky, kmD, kmE, kmF, kmH)
                || (Util_Sky.NeverOnaji(move, src_Sky, kmD) && migiHidari == MigiHidari.Migi)
                || (Util_Sky.NeverOnaji(move, src_Sky, kmF) && migiHidari == MigiHidari.Hidari)
                )) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： 銀は寄れません。

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky.NeverOnaji(move, src_Sky, kmB, kmD, kmE, kmF, kmH)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == Util_Sky.IsNari(move) && !Util_Sky.IsDaAction(move) && Util_Sky.InAitejin(move))
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariNarazu.Narazu;
            }
            else if (Util_Sky.IsNari(move))
            {
                nari = NariNarazu.Nari;
            }
            else
            {
                nari = NariNarazu.CTRL_SONOMAMA;
            }

            fugo = new FugoJ(
                Haiyaku184Array.Syurui(srcKoma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static FugoJ CreateKin(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ fugo;

            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji;

            JFugoCreator15Array.CreateKin_static(move, kWrap, out migiHidari, out agaruHiku, out nari, out daHyoji);

            RO_Star_Koma koma = Util_Koma.AsKoma(move.LongTimeAgo);

            fugo = new FugoJ(
                Haiyaku184Array.Syurui(koma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static void CreateKin_static(
            ShootingStarlightable move,//移動先、移動元、両方のマス番号
            KyokumenWrapper kWrap,
            out MigiHidari migiHidari, out AgaruHiku agaruHiku, out NariNarazu nari, out DaHyoji daHyoji
            )
        {
            SkyConst src_Sky = kWrap.ToKyokumenConst;

            RO_Star_Koma koma = Util_Koma.AsKoma(move.Now);

            //************************************************************
            // △金、△と金、△成香、△成桂、△成銀
            //************************************************************
            daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
            //----------
            //┌─┬─┬─┐
            //│  │Ａ│  │
            //├─┼─┼─┤
            //│Ｇ│至│Ｃ│先手から見た図
            //├─┼─┼─┤
            //│Ｆ│Ｅ│Ｄ│
            //└─┴─┴─┘
            SySet<SyElement> srcA = KomanoKidou.SrcIppo_戻上(koma.Pside, koma.Masu);
            SySet<SyElement> srcC = KomanoKidou.SrcIppo_戻射(koma.Pside, koma.Masu);
            SySet<SyElement> srcD = KomanoKidou.SrcIppo_戻沈(koma.Pside, koma.Masu);
            SySet<SyElement> srcE = KomanoKidou.SrcIppo_戻引(koma.Pside, koma.Masu);
            SySet<SyElement> srcF = KomanoKidou.SrcIppo_戻降(koma.Pside, koma.Masu);
            SySet<SyElement> srcG = KomanoKidou.SrcIppo_戻滑(koma.Pside, koma.Masu);
            ShootingStarlightable src = Util_Sky.Src(move);

            //----------
            // 競合駒
            //----------

            Fingers kmA = Util_Sky.Fingers_EachSrcNow(src_Sky, koma.Pside, move, srcA);
            Fingers kmC = Util_Sky.Fingers_EachSrcNow(src_Sky, koma.Pside, move, srcC);
            Fingers kmD = Util_Sky.Fingers_EachSrcNow(src_Sky, koma.Pside, move, srcD);
            Fingers kmE = Util_Sky.Fingers_EachSrcNow(src_Sky, koma.Pside, move, srcE);
            Fingers kmF = Util_Sky.Fingers_EachSrcNow(src_Sky, koma.Pside, move, srcF);
            Fingers kmG = Util_Sky.Fingers_EachSrcNow(src_Sky, koma.Pside, move, srcG);


            if (Util_Sky.IsDaAction(move))
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky.ExistsIn(src, srcA, src_Sky))
            {
                //----------
                // 移動前はＡだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.No_Print;
            }
            else if (Util_Sky.ExistsIn(src, srcC, src_Sky))
            {
                //----------
                // 移動前はＣだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky.ExistsIn(src, srcF, src_Sky))
            {
                //----------
                // 移動前はＤだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (Util_Sky.ExistsIn(src, srcE, src_Sky))
            {
                //----------
                // 移動前はＥだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Sugu;
            }
            else if (Util_Sky.ExistsIn(src, srcD, src_Sky))
            {
                //----------
                // 移動前はＦだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky.ExistsIn(src, srcG, src_Sky))
            {
                //----------
                // 移動前はＧだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }


            // 「右」解除： ①Ｅ、Ｆ、Ｇのどちらにも競合駒がなければ。
            //              ②Ｇに競合駒がなく、寄があるなら。
            //              ③上があり、Ｅ、Ｆのどちらにも競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && (
                Util_Sky.NeverOnaji(move, src_Sky, kmE, kmF, kmG)
                || (Util_Sky.NeverOnaji(move, src_Sky, kmG) && agaruHiku == AgaruHiku.Yoru)
                || (AgaruHiku.Agaru == agaruHiku && Util_Sky.NeverOnaji(move, src_Sky, kmE, kmF))
                )) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： ①Ｃ、Ｄ、Ｅのどちらにも競合駒がなければ。
            //              ②Ｃに競合駒がなく、寄があるなら。
            //              ③上があり、Ｄ、Ｅのどちらにも競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && (
                Util_Sky.NeverOnaji(move, src_Sky, kmC, kmD, kmE)
                || (Util_Sky.NeverOnaji(move, src_Sky, kmC) && agaruHiku == AgaruHiku.Yoru)
                || (AgaruHiku.Agaru == agaruHiku && Util_Sky.NeverOnaji(move, src_Sky, kmD, kmE))
                )) { migiHidari = MigiHidari.No_Print; }

            // 「直」解除： Ｄ、Ｆのどちらにも競合駒がなければ。
            if (migiHidari == MigiHidari.Sugu && Util_Sky.NeverOnaji(move, src_Sky, kmD, kmF)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： ①Ａ、Ｃ、Ｇのどこにも競合駒がなければ。
            //              ②直があるなら。
            //              ③Ｃに競合駒がなく、右があるなら。
            //              ④Ｇに競合駒がなく、左があるなら。
            if (agaruHiku == AgaruHiku.Agaru &&
                (
                Util_Sky.NeverOnaji(move, src_Sky, kmA, kmC, kmG)
                || migiHidari == MigiHidari.Sugu
                || Util_Sky.NeverOnaji(move, src_Sky, kmC) && migiHidari == MigiHidari.Migi
                || Util_Sky.NeverOnaji(move, src_Sky, kmG) && migiHidari == MigiHidari.Hidari
                )
                ) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｃ、Ｄ、Ｅ、Ｆ、Ｇのどこにも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Hiku && Util_Sky.NeverOnaji(move, src_Sky, kmC, kmD, kmE, kmF, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： ①Ａ、Ｄ、Ｅ、Ｆのどこにも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Yoru && Util_Sky.NeverOnaji(move, src_Sky, kmA, kmD, kmE, kmF)) { agaruHiku = AgaruHiku.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky.NeverOnaji(move, src_Sky, kmA, kmC, kmD, kmE, kmF, kmG)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成れません
            //----------
            nari = NariNarazu.CTRL_SONOMAMA;
        }

        public static FugoJ CreateOh(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ fugo;

            SkyConst copy_Sky = kWrap.ToKyokumenConst;

            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);


            //************************************************************
            // 王
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(range masu)
            //----------
            //┌─┬─┬─┐
            //│Ｈ│Ａ│Ｂ│
            //├─┼─┼─┤
            //│Ｇ│至│Ｃ│先手から見た図
            //├─┼─┼─┤
            //│Ｆ│Ｅ│Ｄ│
            //└─┴─┴─┘

            migiHidari = MigiHidari.No_Print;
            agaruHiku = AgaruHiku.No_Print;

            //----------
            // 王は成れません
            //----------
            nari = NariNarazu.CTRL_SONOMAMA;

            fugo = new FugoJ(
                Haiyaku184Array.Syurui(srcKoma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static FugoJ CreateHisya(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ fugo;

            SkyConst src_Sky = kWrap.ToKyokumenConst;

            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);


            //************************************************************
            // 飛
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
            //----------
            //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
            //  │  │  │  │  │  │  │  │  │A7│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A6│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A5│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A4│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │  │  │  │  │  │A3│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A2│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A1│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A0│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │G7│G6│G5│G4│G3│G2│G1│G0│至│C0│C1│C2│C3│C4│C5│C6│C7│
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E0│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E1│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E2│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E3│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │  │  │  │  │  │E4│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E5│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E6│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E7│  │  │  │  │  │  │  │  │
            //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
            SySet<SyElement> srcA = KomanoKidou.SrcKantu_戻上(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcC = KomanoKidou.SrcKantu_戻射(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcE = KomanoKidou.SrcKantu_戻引(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcG = KomanoKidou.SrcKantu_戻滑(dstKoma.Pside, dstKoma.Masu);
            ShootingStarlightable src = Util_Sky.Src(move);

            //----------
            // 棋譜の現局面：競合駒
            //----------

            Fingers kmA = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcA);
            Fingers kmC = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcC);
            Fingers kmE = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcE);
            Fingers kmG = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcG);

            if (Util_Sky.IsDaAction(move))
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky.ExistsIn(src, srcA, src_Sky))
            {
                //----------
                // Ａにいた
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.No_Print;
            }
            else if (Util_Sky.ExistsIn(src, srcC, src_Sky))
            {
                //----------
                // Ｃにいた
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky.ExistsIn(src, srcE, src_Sky))
            {
                //----------
                // Ｅにいた
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.No_Print;
            }
            else if (Util_Sky.ExistsIn(src, srcG, src_Sky))
            {
                //----------
                // Ｇにいた
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ｇに競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && Util_Sky.NeverOnaji(move, src_Sky, kmG)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ｃに競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && Util_Sky.NeverOnaji(move, src_Sky, kmC)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ａ、Ｃ、Ｇに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Agaru && Util_Sky.NeverOnaji(move, src_Sky, kmA, kmC, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｃ、Ｅ、Ｇに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Hiku && Util_Sky.NeverOnaji(move, src_Sky, kmC, kmE, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： Ａ、Ｅに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Yoru && Util_Sky.NeverOnaji(move, src_Sky, kmA) && Util_Sky.NeverOnaji(move, src_Sky, kmE)) { agaruHiku = AgaruHiku.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky.NeverOnaji(move, src_Sky, kmA, kmC, kmE, kmG)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == Util_Sky.IsNari(move) && !Util_Sky.IsDaAction(move) && Util_Sky.InAitejin(move))
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariNarazu.Narazu;
            }
            else if (Util_Sky.IsNari(move))
            {
                nari = NariNarazu.Nari;
            }
            else
            {
                nari = NariNarazu.CTRL_SONOMAMA;
            }

            fugo = new FugoJ(
                Haiyaku184Array.Syurui(srcKoma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static FugoJ CreateKaku(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ fugo;

            SkyConst src_Sky = kWrap.ToKyokumenConst;

            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);


            //************************************************************
            // 角
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(range masu)
            //----------
            //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
            //  │H7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │B7│
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │H6│  │  │  │  │  │  │  │  │  │  │  │  │  │B6│  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │H5│  │  │  │  │  │  │  │  │  │  │  │B5│  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │H4│  │  │  │  │  │  │  │  │  │B4│  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │  │H3│  │  │  │  │  │  │  │B3│  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │H2│  │  │  │  │  │B2│  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │H1│  │  │  │B1│  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │H0│  │B0│  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │至│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │F0│  │D0│  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │F1│  │  │  │D1│  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │F2│  │  │  │  │  │D2│  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │F3│  │  │  │  │  │  │  │D3│  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │F4│  │  │  │  │  │  │  │  │  │D4│  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │F5│  │  │  │  │  │  │  │  │  │  │  │D5│  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │F6│  │  │  │  │  │  │  │  │  │  │  │  │  │D6│  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │F7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │D7│
            //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
            SySet<SyElement> srcB = KomanoKidou.SrcKantu_戻昇(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcD = KomanoKidou.SrcKantu_戻沈(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcF = KomanoKidou.SrcKantu_戻降(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcH = KomanoKidou.SrcKantu_戻浮(dstKoma.Pside, dstKoma.Masu);
            ShootingStarlightable src = Util_Sky.Src(move);

            //----------
            // 競合駒
            //----------

            Fingers kmB = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcB);
            Fingers kmD = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcD);
            Fingers kmF = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcF);
            Fingers kmH = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcH);


            if (Util_Sky.IsDaAction(move))
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky.ExistsIn(src, srcB, src_Sky))
            {
                //----------
                // 移動前はＢだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky.ExistsIn(src, srcD, src_Sky))
            {
                //----------
                // 移動前はＤだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky.ExistsIn(src, srcF, src_Sky))
            {
                //----------
                // 移動前はＦだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (Util_Sky.ExistsIn(src, srcH, src_Sky))
            {
                //----------
                // 移動前はＨだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ｆ、Ｈに競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && Util_Sky.NeverOnaji(move, src_Sky, kmF, kmH)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ｂ、Ｄに競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && Util_Sky.NeverOnaji(move, src_Sky, kmB, kmD)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ｂ、Ｈに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Agaru && Util_Sky.NeverOnaji(move, src_Sky, kmB, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｄ、Ｆに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Hiku && Util_Sky.NeverOnaji(move, src_Sky, kmD, kmF)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： 角は寄れません。

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky.NeverOnaji(move, src_Sky, kmB, kmD, kmF, kmH)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成
            //----------
            if (false == Util_Sky.IsNari(move) && !Util_Sky.IsDaAction(move) && Util_Sky.InAitejin(move))
            {
                //成の指定がなく、相手陣内に指したら、非成を明示。
                nari = NariNarazu.Narazu;
            }
            else if (Util_Sky.IsNari(move))
            {
                nari = NariNarazu.Nari;
            }
            else
            {
                nari = NariNarazu.CTRL_SONOMAMA;
            }

            fugo = new FugoJ(
                Haiyaku184Array.Syurui(srcKoma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static FugoJ CreateRyu(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ fugo;

            SkyConst src_Sky = kWrap.ToKyokumenConst;

            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);

            //************************************************************
            // 竜
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
            //----------
            //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
            //  │  │  │  │  │  │  │  │  │A7│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A6│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A5│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A4│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │  │  │  │  │  │A3│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A2│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │A1│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │Ｈ│A0│Ｂ│  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │G7│G6│G5│G4│G3│G2│G1│G0│至│C0│C1│C2│C3│C4│C5│C6│C7│
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │Ｆ│E0│Ｄ│  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E1│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E2│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E3│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │  │  │  │  │  │E4│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E5│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E6│  │  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │  │E7│  │  │  │  │  │  │  │  │
            //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
            SySet<SyElement> srcA = KomanoKidou.SrcKantu_戻上(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcB = KomanoKidou.SrcIppo_戻昇(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcC = KomanoKidou.SrcKantu_戻射(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcD = KomanoKidou.SrcIppo_戻沈(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcE = KomanoKidou.SrcKantu_戻引(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcF = KomanoKidou.SrcIppo_戻降(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcG = KomanoKidou.SrcKantu_戻滑(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcH = KomanoKidou.SrcIppo_戻浮(dstKoma.Pside, dstKoma.Masu);
            ShootingStarlightable src = Util_Sky.Src(move);

            //----------
            // 競合駒
            //----------

            Fingers kmA = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcA);
            Fingers kmB = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcB);
            Fingers kmC = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcC);
            Fingers kmD = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcD);
            Fingers kmE = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcE);
            Fingers kmF = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcF);
            Fingers kmG = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcG);
            Fingers kmH = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcH);


            if (Util_Sky.IsDaAction(move))
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky.ExistsIn(src, srcA, src_Sky))
            {
                //----------
                // 移動前はＡだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.No_Print;
            }
            else if (Util_Sky.ExistsIn(src, srcB, src_Sky))
            {
                //----------
                // 移動前はＢだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky.ExistsIn(src, srcC, src_Sky))
            {
                //----------
                // 移動前はＣだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky.ExistsIn(src, srcD, src_Sky))
            {
                //----------
                // 移動前はＤだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky.ExistsIn(src, srcE, src_Sky))
            {
                //----------
                // 移動前はＥだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.No_Print;
            }
            else if (Util_Sky.ExistsIn(src, srcF, src_Sky))
            {
                //----------
                // 移動前はＦだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (Util_Sky.ExistsIn(src, srcG, src_Sky))
            {
                //----------
                // 移動前はＧだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (Util_Sky.ExistsIn(src, srcH, src_Sky))
            {
                //----------
                // 移動前はＨだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ａ、Ｅ、Ｆ、Ｇ、Ｈに１つも競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && Util_Sky.NeverOnaji(move, src_Sky, kmA, kmE, kmF, kmG, kmH)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ａ、Ｂ、Ｃ、Ｄ、Ｅに１つも競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && Util_Sky.NeverOnaji(move, src_Sky, kmA, kmB, kmC, kmD, kmE)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ａ、Ｂ、Ｃ、Ｇ、Ｈに１つも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Agaru && Util_Sky.NeverOnaji(move, src_Sky, kmA, kmB, kmC, kmG, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｃ、Ｄ、Ｅ、Ｆ、Ｇに１つも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Hiku && Util_Sky.NeverOnaji(move, src_Sky, kmC, kmD, kmE, kmF, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： Ａ、Ｂ、Ｄ、Ｅ、Ｆ、Ｈに１つも競合駒がなければ。
            if (agaruHiku == AgaruHiku.Yoru && Util_Sky.NeverOnaji(move, src_Sky, kmA, kmB, kmD, kmE, kmF, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky.NeverOnaji(move, src_Sky, kmA, kmB, kmC, kmD, kmE, kmF, kmG, kmH)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成れません
            //----------
            nari = NariNarazu.CTRL_SONOMAMA;

            fugo = new FugoJ(
                Haiyaku184Array.Syurui(srcKoma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static FugoJ CreateUma(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ fugo;

            SkyConst src_Sky = kWrap.ToKyokumenConst;

            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);


            //************************************************************
            // 馬
            //************************************************************
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            //----------
            // 競合駒マス(pre masu)
            //----------
            //  ┌─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┬─┐
            //  │H7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │B7│
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │H6│  │  │  │  │  │  │  │  │  │  │  │  │  │B6│  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │H5│  │  │  │  │  │  │  │  │  │  │  │B5│  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │H4│  │  │  │  │  │  │  │  │  │B4│  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │  │H3│  │  │  │  │  │  │  │B3│  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │H2│  │  │  │  │  │B2│  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │H1│  │  │  │B1│  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │H0│Ａ│B0│  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │Ｇ│至│Ｃ│  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │  │F0│Ｅ│D0│  │  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │  │F1│  │  │  │D1│  │  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │  │F2│  │  │  │  │  │D2│  │  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │  │  │F3│  │  │  │  │  │  │  │D3│  │  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //　│  │  │  │F4│  │  │  │  │  │  │  │  │  │D4│  │  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │  │F5│  │  │  │  │  │  │  │  │  │  │  │D5│  │  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │  │F6│  │  │  │  │  │  │  │  │  │  │  │  │  │D6│  │
            //  ├─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┼─┤
            //  │F7│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │D7│
            //  └─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┴─┘
            SySet<SyElement> srcA = KomanoKidou.SrcIppo_戻上(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcB = KomanoKidou.SrcKantu_戻昇(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcC = KomanoKidou.SrcIppo_戻射(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcD = KomanoKidou.SrcKantu_戻沈(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcE = KomanoKidou.SrcIppo_戻引(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcF = KomanoKidou.SrcKantu_戻降(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcG = KomanoKidou.SrcIppo_戻滑(dstKoma.Pside, dstKoma.Masu);
            SySet<SyElement> srcH = KomanoKidou.SrcKantu_戻浮(dstKoma.Pside, dstKoma.Masu);
            ShootingStarlightable src = Util_Sky.Src(move);

            //----------
            // 競合駒
            //----------


            Fingers kmA = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcA);
            Fingers kmB = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcB);
            Fingers kmC = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcC);
            Fingers kmD = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcD);
            Fingers kmE = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcE);
            Fingers kmF = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcF);
            Fingers kmG = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcG);
            Fingers kmH = Util_Sky.Fingers_EachSrcNow(src_Sky, dstKoma.Pside, move, srcH);

            if (Util_Sky.IsDaAction(move))
            {
                // 打と明示されていた
                //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
                daHyoji = DaHyoji.Visible;
            }
            else if (Util_Sky.ExistsIn(src, srcB, src_Sky))
            {
                //----------
                // 移動前はＢだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky.ExistsIn(src, srcD, src_Sky))
            {
                //----------
                // 移動前はＤだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky.ExistsIn(src, srcF, src_Sky))
            {
                //----------
                // 移動前はＦだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.Hidari;
            }
            else if (Util_Sky.ExistsIn(src, srcH, src_Sky))
            {
                //----------
                // 移動前はＨだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.Hidari;
            }
            else if (Util_Sky.ExistsIn(src, srcA, src_Sky))
            {
                //----------
                // 移動前はＡだった
                //----------
                agaruHiku = AgaruHiku.Hiku;
                migiHidari = MigiHidari.No_Print;
            }
            else if (Util_Sky.ExistsIn(src, srcC, src_Sky))
            {
                //----------
                // 移動前はＣだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Migi;
            }
            else if (Util_Sky.ExistsIn(src, srcE, src_Sky))
            {
                //----------
                // 移動前はＥだった
                //----------
                agaruHiku = AgaruHiku.Agaru;
                migiHidari = MigiHidari.No_Print;
            }
            else if (Util_Sky.ExistsIn(src, srcG, src_Sky))
            {
                //----------
                // 移動前はＧだった
                //----------
                agaruHiku = AgaruHiku.Yoru;
                migiHidari = MigiHidari.Hidari;
            }
            else
            {
                //----------
                // どこからか飛んできた
                //----------
                agaruHiku = AgaruHiku.No_Print;
                migiHidari = MigiHidari.No_Print;
            }

            // 「右」解除： Ａ、Ｅ、Ｆ、Ｇ、Ｈに競合駒がなければ。
            if (migiHidari == MigiHidari.Migi && Util_Sky.NeverOnaji(move, src_Sky, kmA, kmE, kmF, kmG, kmH)) { migiHidari = MigiHidari.No_Print; }

            // 「左」解除： Ａ、Ｂ、Ｃ、Ｄ、Ｅに競合駒がなければ。
            if (migiHidari == MigiHidari.Hidari && Util_Sky.NeverOnaji(move, src_Sky, kmA, kmB, kmC, kmD, kmE)) { migiHidari = MigiHidari.No_Print; }

            // 「上」解除： Ａ、Ｂ、Ｃ、Ｇ、Ｈに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Agaru && Util_Sky.NeverOnaji(move, src_Sky, kmA, kmB, kmC, kmG, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「引」解除： Ｃ、Ｄ、Ｅ、Ｆ、Ｇに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Hiku && Util_Sky.NeverOnaji(move, src_Sky, kmC, kmD, kmE, kmF, kmG)) { agaruHiku = AgaruHiku.No_Print; }

            // 「寄」解除： Ａ、Ｂ、Ｄ、Ｅ、Ｆ、Ｈに競合駒がなければ。
            if (agaruHiku == AgaruHiku.Yoru && Util_Sky.NeverOnaji(move, src_Sky, kmA, kmB, kmD, kmE, kmF, kmH)) { agaruHiku = AgaruHiku.No_Print; }

            // 「打」解除： 競合範囲全てに競合駒がなければ。
            if (daHyoji == DaHyoji.Visible && Util_Sky.NeverOnaji(move, src_Sky, kmA, kmB, kmC, kmD, kmE, kmF, kmG, kmH)) { daHyoji = DaHyoji.No_Print; }

            //----------
            // 成れません
            //----------
            nari = NariNarazu.CTRL_SONOMAMA;

            fugo = new FugoJ(
                Haiyaku184Array.Syurui(srcKoma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static FugoJ CreateTokin(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ fugo;

            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji;

            JFugoCreator15Array.CreateKin_static(move, kWrap, out migiHidari, out agaruHiku, out nari, out daHyoji);

            RO_Star_Koma koma = Util_Koma.AsKoma(move.LongTimeAgo);

            fugo = new FugoJ(
                Haiyaku184Array.Syurui(koma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static FugoJ CreateNariKyo(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji;

            JFugoCreator15Array.CreateKin_static(move, kWrap, out migiHidari, out agaruHiku, out nari, out daHyoji);

            FugoJ fugo;

            RO_Star_Koma koma = Util_Koma.AsKoma(move.LongTimeAgo);

            fugo = new FugoJ(
                Haiyaku184Array.Syurui(koma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static FugoJ CreateNariKei(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ fugo;

            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji;

            RO_Star_Koma koma = Util_Koma.AsKoma(move.LongTimeAgo);

            JFugoCreator15Array.CreateKin_static(move, kWrap, out migiHidari, out agaruHiku, out nari, out daHyoji);

            fugo = new FugoJ(
                Haiyaku184Array.Syurui(koma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static FugoJ CreateNariGin(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ fugo;

            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);


            MigiHidari migiHidari;
            AgaruHiku agaruHiku;
            NariNarazu nari;
            DaHyoji daHyoji;

            JFugoCreator15Array.CreateKin_static(move, kWrap, out migiHidari, out agaruHiku, out nari, out daHyoji);

            fugo = new FugoJ(
                Haiyaku184Array.Syurui(srcKoma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }

        public static FugoJ CreateErrorKoma(ShootingStarlightable move, KyokumenWrapper kWrap)
        {
            FugoJ fugo;

            SkyConst copy_Sky = kWrap.ToKyokumenConst;

            RO_Star_Koma srcKoma = Util_Koma.AsKoma(move.LongTimeAgo);
            RO_Star_Koma dstKoma = Util_Koma.AsKoma(move.Now);


            //************************************************************
            // エラー
            //************************************************************
            MigiHidari migiHidari = MigiHidari.No_Print;
            AgaruHiku agaruHiku = AgaruHiku.No_Print;
            NariNarazu nari;
            DaHyoji daHyoji = DaHyoji.No_Print; // “打”表示は、駒を打ったときとは異なります。

            if (Playerside.P2 == dstKoma.Pside)
            {
                //******************************
                // △後手
                //******************************
            }
            else
            {
                //******************************
                // ▲先手
                //******************************
            }


            //----------
            // TODO: 移動前の駒が成る前かどうか
            //----------
            nari = NariNarazu.CTRL_SONOMAMA;

            fugo = new FugoJ(
                Haiyaku184Array.Syurui(srcKoma.Haiyaku),//「▲２二角成」のとき、dstだと馬になってしまう。srcの角を使う。
                migiHidari,
                agaruHiku,
                nari,
                daHyoji
                );

            return fugo;
        }
    }
}
