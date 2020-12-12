using System.Text;
using System.Collections.Generic;

using Grayscale.P025_KifuLarabe.L00012_Atom;

namespace Grayscale.P025_KifuLarabe.L002_GraphicLog
{

    /// <summary>
    /// グラフィカル局面ログの、盤１個。
    /// </summary>
    public class GraphicalLog_Board
    {
        /// <summary>
        /// あれば指し手。なければヌル。
        /// </summary>
        public IMove moveOrNull{get;set;}

        /// <summary>
        /// 説明文章（１行）。
        /// </summary>
        public string Caption { get; set; }

        public Gkl_NounaiSeme NounaiSeme { get; set; }

        public List<Gkl_KomaMasu> KomaMasu1 { get; set; }
        public List<Gkl_KomaMasu> KomaMasu2 { get; set; }
        public List<Gkl_KomaMasu> KomaMasu3 { get; set; }
        public List<Gkl_KomaMasu> KomaMasu4 { get; set; }

        /// <summary>
        /// 手目済
        /// </summary>
        public int Tesumi { get; set; }

        public Playerside GenTeban { get; set; }
        public int NounaiYomiDeep { get; set; }
        public double Score { get; set; }//局面評価関数の評価値

        /// <summary>
        /// 色つきマス
        /// </summary>
        public List<int> Masu_theMove { get; set; }//移動可能
        public List<int> Masu_theEffect { get; set; }//利き
        public List<int> Masu_3 { get; set; }
        public List<int> Masu_4 { get; set; }

        /// <summary>
        /// マークがあるマス
        /// </summary>
        public List<int> MarkMasu1 { get; set; }
        public List<int> MarkMasu2 { get; set; }
        public List<int> MarkMasu3 { get; set; }
        public List<int> MarkMasu4 { get; set; }

        /// <summary>
        /// 矢印
        /// </summary>
        public List<Gkl_Arrow> Arrow { get; set; }


        public GraphicalLog_Board()
        {
            this.Caption = "";

            this.NounaiSeme = Gkl_NounaiSeme.Empty;
            this.KomaMasu1 = new List<Gkl_KomaMasu>();
            this.KomaMasu2 = new List<Gkl_KomaMasu>();
            this.KomaMasu3 = new List<Gkl_KomaMasu>();
            this.KomaMasu4 = new List<Gkl_KomaMasu>();

            this.Tesumi = int.MinValue;
            this.GenTeban = Playerside.Empty;
            this.NounaiYomiDeep = int.MinValue;
            this.Score = double.MinValue;

            this.Masu_theMove = new List<int>();
            this.Masu_theEffect = new List<int>();
            this.Masu_3 = new List<int>();
            this.Masu_4 = new List<int>();

            this.MarkMasu1 = new List<int>();
            this.MarkMasu2 = new List<int>();
            this.MarkMasu3 = new List<int>();
            this.MarkMasu4 = new List<int>();

            this.Arrow = new List<Gkl_Arrow>();
        }

        public GraphicalLog_Board(GraphicalLog_Board src)
        {
            this.Caption = src.Caption;

            this.NounaiSeme = src.NounaiSeme;
            this.KomaMasu1 = src.KomaMasu1;
            this.KomaMasu2 = src.KomaMasu2;
            this.KomaMasu3 = src.KomaMasu3;
            this.KomaMasu4 = src.KomaMasu4;

            this.Tesumi = src.Tesumi;
            this.GenTeban = src.GenTeban;
            this.NounaiYomiDeep = src.NounaiYomiDeep;
            this.Score = src.Score;

            this.Masu_theMove = src.Masu_theMove;
            this.Masu_theEffect = src.Masu_theEffect;
            this.Masu_3 = src.Masu_3;
            this.Masu_4 = src.Masu_4;

            this.MarkMasu1 = src.MarkMasu1;
            this.MarkMasu2 = src.MarkMasu2;
            this.MarkMasu3 = src.MarkMasu3;
            this.MarkMasu4 = src.MarkMasu4;

            this.Arrow = src.Arrow;
        }

        public GraphicalLog_Board Clone()
        {
            GraphicalLog_Board clone = new GraphicalLog_Board(this);
            return clone;
        }

        public string ToJsonStr()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("[");

            // 色つきマス
            {
                // 青
                {
                    sb.AppendLine("{ \"act\":\"colorMasu\", \"style\":\"rgba(100,100,240,0.5)\" },");

                    foreach (int masu in this.Masu_theMove)
                    {
                        sb.AppendLine("{ \"act\":\"drawMasu\" , \"masu\":" + masu.ToString() + " },");
                    }
                }

                // 赤
                {
                    sb.AppendLine("{ \"act\":\"colorMasu\", \"style\":\"rgba(240,100,100,0.5)\" },");

                    foreach (int masu in this.Masu_theEffect)
                    {
                        sb.AppendLine("{ \"act\":\"drawMasu\" , \"masu\":" + masu.ToString() + " },");
                    }
                }

                // 水色
                {
                    sb.AppendLine("{ \"act\":\"colorMasu\", \"style\":\"rgba(100,240,240,0.5)\" },");

                    foreach (int masu in this.Masu_3)
                    {
                        sb.AppendLine("{ \"act\":\"drawMasu\" , \"masu\":" + masu.ToString() + " },");
                    }
                }

                // 緑
                {
                    sb.AppendLine("{ \"act\":\"colorMasu\", \"style\":\"rgba(100,240,100,0.5)\" },");

                    foreach(int masu in this.Masu_4)
                    {
                        sb.AppendLine("{ \"act\":\"drawMasu\" , \"masu\":" + masu.ToString() + " },");
                    }
                }
            }

            // マークがあるマス
            {
                // 赤駒
                {
                    sb.AppendLine("{");
                    sb.AppendLine("    \"act\":\"begin_imgColor\",");
                    sb.AppendLine("    \"colors\":[");
                    sb.AppendLine("        { \"sR\":255, \"sG\":255, \"sB\":255, \"sA\":255, \"dR\":0, \"dG\":0, \"dB\":200, \"dA\":255 },");
                    sb.AppendLine("    ],");
                    sb.AppendLine("},");

                    foreach (int masu in this.MarkMasu1)
                    {
                        sb.AppendLine("{ \"act\":\"drawImg\", \"img\":\"mark1\", \"masu\":" + masu + " },");
                    }

                    sb.AppendLine("{ \"act\":\"end_imgColor\", },");
                }

                // 青駒
                {
                    sb.AppendLine("{");
                    sb.AppendLine("    \"act\":\"begin_imgColor\",");
                    sb.AppendLine("    \"colors\":[");
                    sb.AppendLine("        { \"sR\":255, \"sG\":255, \"sB\":255, \"sA\":255, \"dR\":200, \"dG\":0, \"dB\":0, \"dA\":255 },");
                    sb.AppendLine("    ],");
                    sb.AppendLine("},");

                    foreach (int masu in this.MarkMasu2)
                    {
                        sb.AppendLine("{ \"act\":\"drawImg\", \"img\":\"mark1\", \"masu\":" + masu + " },");
                    }

                    sb.AppendLine("{ \"act\":\"end_imgColor\", },");
                }

                // 水色
                {
                    sb.AppendLine("{");
                    sb.AppendLine("    \"act\":\"begin_imgColor\",");
                    sb.AppendLine("    \"colors\":[");
                    sb.AppendLine("        { \"sR\":255, \"sG\":255, \"sB\":255, \"sA\":255, \"dR\":0, \"dG\":200, \"dB\":200, \"dA\":255 },");
                    sb.AppendLine("    ],");
                    sb.AppendLine("},");

                    foreach (int masu in this.MarkMasu3)
                    {
                        sb.AppendLine("{ \"act\":\"drawImg\", \"img\":\"mark1\", \"masu\":" + masu + " },");
                    }

                    sb.AppendLine("{ \"act\":\"end_imgColor\", },");
                }

                // 緑駒
                {
                    sb.AppendLine("{");
                    sb.AppendLine("    \"act\":\"begin_imgColor\",");
                    sb.AppendLine("    \"colors\":[");
                    sb.AppendLine("        { \"sR\":255, \"sG\":255, \"sB\":255, \"sA\":255, \"dR\":0, \"dG\":200, \"dB\":0, \"dA\":255 },");
                    sb.AppendLine("    ],");
                    sb.AppendLine("},");

                    foreach (int masu in this.MarkMasu4)
                    {
                        sb.AppendLine("{ \"act\":\"drawImg\", \"img\":\"mark1\", \"masu\":" + masu + " },");
                    }

                    sb.AppendLine("{ \"act\":\"end_imgColor\", },");
                }
            }

            // 脳内攻め手
            switch (this.NounaiSeme)
            {
                case Gkl_NounaiSeme.Sente:
                    {
                        sb.AppendLine("{");
                        sb.AppendLine("    \"act\":\"begin_imgColor\",");
                        sb.AppendLine("    \"colors\":[");
                        sb.AppendLine("        { \"sR\":255, \"sG\":255, \"sB\":255, \"sA\":255, \"dR\":  0, \"dG\":  0, \"dB\":200, \"dA\":255 },");
                        sb.AppendLine("        { \"sR\":238, \"sG\":238, \"sB\":238, \"sA\":255, \"dR\":128, \"dG\":128, \"dB\":  0, \"dA\":128 },");
                        sb.AppendLine("    ],");
                        sb.AppendLine("},");
                        sb.AppendLine("{ \"act\":\"drawImg\", \"img\":\"nounaiSeme\", \"masu\": 118 },");
                        sb.AppendLine("{ \"act\":\"end_imgColor\", },");
                    }
                    break;
                case Gkl_NounaiSeme.Gote:
                    {
                        sb.AppendLine("{");
                        sb.AppendLine("    \"act\":\"begin_imgColor\",");
                        sb.AppendLine("    \"colors\":[");
                        sb.AppendLine("        { \"sR\":255, \"sG\":255, \"sB\":255, \"sA\":255, \"dR\":  0, \"dG\":  0, \"dB\":200, \"dA\":255 },");
                        sb.AppendLine("        { \"sR\":238, \"sG\":238, \"sB\":238, \"sA\":255, \"dR\":128, \"dG\":128, \"dB\":  0, \"dA\":128 },");
                        sb.AppendLine("    ],");
                        sb.AppendLine("},");
                        sb.AppendLine("{ \"act\":\"drawImg\", \"img\":\"nounaiSemeV\", \"masu\": 151 },");
                        sb.AppendLine("{ \"act\":\"end_imgColor\", },");
                    }
                    break;
                default:
                    break;
            }

            // 赤駒
            {
                sb.AppendLine("{");
                sb.AppendLine("    \"act\":\"begin_imgColor\",");
                sb.AppendLine("    \"colors\":[");
                sb.AppendLine("        { \"sR\":255, \"sG\":255, \"sB\":255, \"sA\":255, \"dR\":0, \"dG\":0, \"dB\":200, \"dA\":255 },");
                sb.AppendLine("    ],");
                sb.AppendLine("},");

                foreach (Gkl_KomaMasu km in this.KomaMasu1)
                {
                    sb.AppendLine("{ act:\"drawImg\", img:\"" + km.KomaImg + "\", masu:" + km.Masu + " },");
                }

                sb.AppendLine("{ act:\"end_imgColor\", },");
            }

            // 青駒
            {
                sb.AppendLine("{");
                sb.AppendLine("    \"act\":\"begin_imgColor\",");
                sb.AppendLine("    \"colors\":[");
                sb.AppendLine("        { \"sR\":255, \"sG\":255, \"sB\":255, \"sA\":255, \"dR\":200, \"dG\":0, \"dB\":0, \"dA\":255 },");
                sb.AppendLine("    ],");
                sb.AppendLine("},");

                foreach (Gkl_KomaMasu km in this.KomaMasu2)
                {
                    sb.AppendLine("{ act:\"drawImg\", img:\"" + km.KomaImg + "\", masu:" + km.Masu + " },");
                }

                sb.AppendLine("{ act:\"end_imgColor\", },");
            }

            // 水色
            {
                sb.AppendLine("{");
                sb.AppendLine("    act:\"begin_imgColor\",");
                sb.AppendLine("    colors:[");
                sb.AppendLine("        { sR:255, sG:255, sB:255, sA:255, dR:0, dG:200, dB:200, dA:255 },");
                sb.AppendLine("    ],");
                sb.AppendLine("},");

                foreach (Gkl_KomaMasu km in this.KomaMasu3)
                {
                    sb.AppendLine("{ act:\"drawImg\", img:\"" + km.KomaImg + "\", masu:" + km.Masu + " },");
                }

                sb.AppendLine("{ act:\"end_imgColor\", },");
            }

            // 緑駒
            {
                sb.AppendLine("{");
                sb.AppendLine("    act:\"begin_imgColor\",");
                sb.AppendLine("    colors:[");
                sb.AppendLine("        { sR:255, sG:255, sB:255, sA:255, dR:0, dG:200, dB:0, dA:255 },");
                sb.AppendLine("    ],");
                sb.AppendLine("},");

                foreach (Gkl_KomaMasu km in this.KomaMasu4)
                {
                    sb.AppendLine("{ act:\"drawImg\", img:\"" + km.KomaImg + "\", masu:" + km.Masu + " },");
                }

                sb.AppendLine("{ act:\"end_imgColor\", },");
            }

            // 矢印
            {
                // 赤色
                sb.AppendLine("{ act:\"colorArrow\", style:\"rgba(240,100,100,0.8)\" },");

                foreach (Gkl_Arrow a in this.Arrow)
                {
                    sb.AppendLine("{ act:\"drawArrow\", from:\"" + a.From + "\", to:" + a.To + " },");
                }
            }

            // テキスト
            {
                sb.AppendLine();
                sb.Append("\"テキスト\",");

                // キャプション
                if (this.Caption!="")
                {
                    sb.Append("{ act:\"drawText\", text:\"");
                    sb.Append(this.Caption);
                    sb.Append("\"  , x:0, y:20 },");
                    sb.AppendLine();
                }

                // 例：「3手済 先手番/脳内 1手先」
                {
                    sb.Append("{ act:\"drawText\", text:\"");

                    if (this.Tesumi != int.MinValue)
                    {
                        sb.Append(this.Tesumi + "手済");
                    }

                    switch (this.GenTeban)
                    {
                        case Playerside.P1:
                            sb.Append(" 先手番");
                            break;
                        case Playerside.P2:
                            sb.Append(" 後手番");
                            break;
                        default:
                            break;
                    }

                    if (this.NounaiYomiDeep != int.MinValue)
                    {
                        sb.Append("/脳内 ");
                        sb.Append(this.NounaiYomiDeep.ToString());
                        sb.Append("手先");
                    }

                    if (this.Score != double.MinValue)
                    {
                        sb.Append(" 評価");
                        sb.Append(((double)this.Score).ToString());
                    }

                    sb.Append("\"  , x:120, y:270 },");
                    sb.AppendLine();
                }
            }

            sb.AppendLine("],");

            return sb.ToString();
        }

    }

    /// <summary>
    /// 脳内攻め（Playersideとはわざと区別してあります）
    /// </summary>
    public enum Gkl_NounaiSeme
    {
        Sente,
        Gote,
        Empty
    }

    /// <summary>
    /// 駒画像とマス
    /// </summary>
    public class Gkl_KomaMasu
    {
        public string KomaImg { get; set; }
        public int Masu { get; set; }

        public Gkl_KomaMasu(string komaImg, int masu)
        {
            this.KomaImg = komaImg;
            this.Masu = masu;
        }
    }

    /// <summary>
    /// 矢印１個
    /// </summary>
    public class Gkl_Arrow
    {
        public int From { get; set; }
        public int To { get; set; }

        public Gkl_Arrow()
        {
            this.From = int.MinValue;
            this.To = int.MinValue;
        }

        public Gkl_Arrow(int from, int to)
        {
            this.From = from;
            this.To = to;
        }
    }

}
