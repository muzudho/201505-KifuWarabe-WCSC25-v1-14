namespace Grayscale.P050_KifuWarabe.L003_Kokoro
{
#if Debug
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Codeplex.Data;//DynamicJson
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.P050_KifuWarabe.L00049_Kokoro;
using Grayscale.P050_KifuWarabe.L030_Shogisasi;
using Grayscale.Kifuwarazusa.Entities.Logging;
#else
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Windows.Forms;
    using Codeplex.Data;//DynamicJson
    using Grayscale.Kifuwarazusa.Entities.Features;
    using Grayscale.Kifuwarazusa.Entities.Logging;
    using Grayscale.P050_KifuWarabe.L00025_UsiLoop;
    using Grayscale.P050_KifuWarabe.L00049_Kokoro;
    using Grayscale.P050_KifuWarabe.L030_Shogisasi;

#endif

    /// <summary>
    /// 心エンジン。
    /// </summary>
    public class KokoroImpl : Kokoro
    {

        private ShogiEngine owner;


        /// <summary>
        /// 頭の隅（手の流れ）を集めたもの。
        /// </summary>
        public List<Tenonagare> TenonagareItems { get; set; }


        public KokoroImpl(ShogiEngine owner)
        {
            this.owner = owner;

            this.TenonagareItems = new List<Tenonagare>();
        }


        /// <summary>
        /// 頭の隅（手の流れ）を作成します☆ｗ
        /// </summary>
        /// <param name="input_myPside"></param>
        /// <param name="save_node">狙い作成時の局面を状況として記録するために読取します。</param>
        /// <param name="input_seikaku"></param>
        public void Omoituki(
            Playerside input_myPside,
            KifuNode save_node,
            Seikaku input_seikaku
            )
        {
            SkyConst src_Sky = save_node.Value.ToKyokumenConst;

            // FIXME: どの狙いか　どんぶり勘定になっている☆
            {
                List<Tenonagare> removee = new List<Tenonagare>();

                foreach (Tenonagare atamanosumi in this.TenonagareItems)
                {
                    // 失敗回数が、粘り強さを超えたとき
                    if (input_seikaku.NebariDuyosa < atamanosumi.ResultKioku.Sippai)
                    {
                        // 今いだいている妄想を捨てます。
                        removee.Add(atamanosumi);
                    }
                }

                foreach (Tenonagare atamanosumi in removee)
                {
                    this.TenonagareItems.Remove(atamanosumi);
                }
            }

            //
            // とりあえずサンプルとして１つ作ってみます。
            //

            {
                //MessageBox.Show( "とりあえずサンプルとして１つ作ってみます。","デバッグ");
                Tenonagare atamanosumi;

                // 2Pの飛車先の歩の付き捨て
                {
                    if (input_myPside == Playerside.P2)
                    {
                        //MessageBox.Show("プレイヤー２です。", "デバッグ");

                        // ８二に飛車がいる前提です。
                        RO_Star_Koma koma82 = Util_Sky.Koma_AtMasuNow(src_Sky, Masu_Honshogi.ban82_８二, Playerside.P2, Ks14.H07_Hisya);

                        if (null != koma82)
                        {
                            //MessageBox.Show("８二に飛車がいました。", "デバッグ");
                            // ８二に飛車がいた。

                            // 飛車先の駒は？
                            RO_Star_Koma koma83 = Util_Sky.Koma_AtMasuNow(src_Sky, Masu_Honshogi.ban83_８三, Playerside.P2, Ks14.H01_Fu);

                            RO_Star_Koma koma84 = Util_Sky.Koma_AtMasuNow(src_Sky, Masu_Honshogi.ban84_８四, Playerside.P2);
                            RO_Star_Koma koma85 = Util_Sky.Koma_AtMasuNow(src_Sky, Masu_Honshogi.ban85_８五, Playerside.P2);
                            RO_Star_Koma koma86 = Util_Sky.Koma_AtMasuNow(src_Sky, Masu_Honshogi.ban86_８六, Playerside.P2);
                            RO_Star_Koma koma87 = Util_Sky.Koma_AtMasuNow(src_Sky, Masu_Honshogi.ban87_８七, Playerside.P2);
                            RO_Star_Koma koma88 = Util_Sky.Koma_AtMasuNow(src_Sky, Masu_Honshogi.ban88_８八, Playerside.P2);

                            if (koma83 != null && koma84 == null && koma85 == null && koma86 == null && koma87 == null && koma88 == null)
                            {
                                //MessageBox.Show("８三に自歩があり、８四～８八に自駒はありませんでした。", "デバッグ");

                                // ８三に自歩があり、８四～８八に自駒が無ければ。

                                // ８三の駒を、８九に向かって前進させます。

                                atamanosumi = new TenonagareImpl(save_node.ToRO_Kyokumen1(),
                                    TenonagareName.Tukisute,
                                    1000.0d,
                                    koma83,//どの駒を
                                    null,
                                    Masu_Honshogi.ban83_８三//初期位置
                                    );
                                goto gt_Next1;
                            }

                        }

                    }
                }

                int random = LarabeRandom.Random.Next(-9, 10);
                if (0 < random)
                {
                    //
                    // 自駒をランダムに１つ指定し、目指すマスをランダムに１つ指定し、進ませます。
                    // 係数は下げます。
                    //

                    // どの駒が
                    RO_Star_Koma koma1;
                    switch (input_myPside)
                    {
                        case Playerside.P1: koma1 = Util_Koma.FromFinger(src_Sky, LarabeRandom.Random.Next(0, 19)); break;
                        case Playerside.P2: koma1 = Util_Koma.FromFinger(src_Sky, LarabeRandom.Random.Next(20, 39)); break;
                        default: koma1 = Util_Koma.FromFinger(src_Sky, -1); break;
                    }

                    atamanosumi = new TenonagareImpl(save_node.ToRO_Kyokumen1(),
                        TenonagareName.Ido,// 「移動」タイプの狙い
                        0.05d,//1.0d,
                        koma1,
                        null,
                        new Basho(LarabeRandom.Random.Next(0, 80))// 目指すマス
                        );
                }
                else
                {
                    //
                    // 自駒をランダムに１つ指定し、相手の駒をランダムに１つ指定し、
                    // 取るように目指させます。
                    // 係数は下げます。
                    //
                    RO_Star_Koma koma1;
                    switch (input_myPside)
                    {
                        // FIXME: 持ち駒を考えられていない。
                        case Playerside.P1: koma1 = Util_Koma.FromFinger(src_Sky, LarabeRandom.Random.Next(0, 19)); break;
                        case Playerside.P2: koma1 = Util_Koma.FromFinger(src_Sky, LarabeRandom.Random.Next(20, 39)); break;
                        default: koma1 = Util_Koma.FromFinger(src_Sky, -1); break;
                    }

                    // どの駒を
                    RO_Star_Koma koma2;
                    switch (input_myPside)
                    {
                        // FIXME: 持ち駒を考えられていない。
                        case Playerside.P1:
                            koma2 = Util_Koma.FromFinger(src_Sky, LarabeRandom.Random.Next(20, 39));
                            break;
                        case Playerside.P2:
                            koma2 = Util_Koma.FromFinger(src_Sky, LarabeRandom.Random.Next(0, 19));
                            break;
                        default:
                            koma2 = Util_Koma.FromFinger(src_Sky, -1);
                            break;
                    }

                    // 「取る」タイプの狙い
                    atamanosumi = new TenonagareImpl(save_node.ToRO_Kyokumen1(),
                        TenonagareName.Toru,
                        0.1d,//1.0d,
                        koma1, koma2, new Basho(0));

                    // 目指す
                }
            gt_Next1:



                // 作った妄想は履歴に追加。
                //MessageBox.Show("作った妄想は履歴に追加。", "デバッグ");
                this.AddTenonagare(atamanosumi);
            }

            // FIXME: 「手の流れ」が？個を超えてたら、5個消していく☆秒読みで10超えてしまうので。
            {
                int tenonagareMax = 15;//25
                int kesuKazu = 5;

                if (tenonagareMax < this.TenonagareItems.Count)
                {
                    int iKesu = kesuKazu;// tenonagareMax - this.TenonagareItems.Count + 5;

                    List<Tenonagare> removee = new List<Tenonagare>();

                    foreach (Tenonagare nagare in this.TenonagareItems)
                    {
                        if (iKesu < 1)
                        {
                            break;
                        }

                        // 失敗回数が、粘り強さを超えたとき
                        if (input_seikaku.NebariDuyosa < nagare.ResultKioku.Sippai)
                        {
                            // 今いだいている妄想を捨てます。
                            removee.Add(nagare);
                        }

                        iKesu--;
                    }

                    foreach (Tenonagare nagare in removee)
                    {
                        this.TenonagareItems.Remove(nagare);
                    }
                }

            }

        }







        public void ClearTenonagare()
        {
            this.TenonagareItems.Clear();
        }

        public void AddTenonagare(Tenonagare kiokuHow)
        {
            this.TenonagareItems.Add(kiokuHow);
        }

        public void WriteTenonagare(object obj_sikouEngine)
        {
            ShogisasiImpl shogisasi = (ShogisasiImpl)obj_sikouEngine;

            // 追記ではなく、上書きにしたい☆
            Logger.Trace(
                shogisasi.Kokoro.TenonagareToJsonVal().ToString(), SpecifyLogFiles.MousouRireki
                );
        }

        public void ReadTenonagare()
        {
            if (File.Exists(SpecifyLogFiles.MousouRireki.Name))
            {
                string mousouRirekiLog = System.IO.File.ReadAllText(SpecifyLogFiles.MousouRireki.Name, Encoding.UTF8);

#if DEBUG
                MessageBox.Show(mousouRirekiLog, "妄想履歴ログ有り　（デバッグモード　正常）");
#endif

                var jsonMousou_arr = DynamicJson.Parse(mousouRirekiLog);

                //#if DEBUG
                //                        try
                //                        {
                //                            // 妄想配列
                //                            foreach (var mousou in jsonMousou_arr)
                //                            {
                //                                MessageBox.Show("name=[" + mousou.name + "] finger=[" + mousou.finger + "] masu=[" + mousou.masu + "] ", "DynamicJSONテスト");
                //                            }
                //                        }
                //                        catch (Exception ex)
                //                        {
                //                            MessageBox.Show("デバッグライト時にエラーか？：" + ex.GetType().Name + "：" + ex.Message);
                //                        }
                //#endif




            }


            /*
            // テスト

            // Read and Access

            // Parse (from JsonString to DynamicJson)
            var json = DynamicJson.Parse(@"{""foo"":""json"", ""bar"":100, ""nest"":{ ""foobar"":true } }");

            var r1 = json.foo; // "json" - dynamic(string)
            var r2 = json.bar; // 100 - dynamic(double)
            var r3 = json.nest.foobar; // true - dynamic(bool)
            var r4 = json["nest"]["foobar"]; // can access indexer

            MessageBox.Show( "r1=[" + r1.ToString() + "] r2=[" + r2.ToString() + "] r3=[" + r3.ToString() + "] r4=[" + r4.ToString() + "] ", "DynamicJSONテスト");
            */


        }

        public Json_Val TenonagareToJsonVal()
        {

            Json_Arr arr = new Json_Arr();
            arr.NewLineEnable = true;

            foreach (Tenonagare item in this.TenonagareItems)
            {
                arr.Add(item.ToJsonVal());
            }


            return arr;
        }

    }
}
