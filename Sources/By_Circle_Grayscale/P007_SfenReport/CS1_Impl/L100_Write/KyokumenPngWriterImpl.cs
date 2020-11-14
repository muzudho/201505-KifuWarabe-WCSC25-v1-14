using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P007_SfenReport.L00050_Write;
using Grayscale.P007_SfenReport.L00025_Report;
using Grayscale.P007_SfenReport.L050_Report;
using System.Drawing;
using Grayscale.P006_Sfen.L0005_Sfen;
using Grayscale.P006_Sfen.L100_Sfen;
using System.IO;

namespace Grayscale.P007_SfenReport.L100_Write
{
    public class KyokumenPngWriterImpl : KyokumenPngWriter
    {

        public const int MOTI_LEN = 7;//飛、角、金、銀、桂、香、歩の７つ。
        public const int BN_SUJIS = 9;//将棋盤は9筋。ban sujis
        public const int BN_DANS = 9;

        public const int BN_BRD_R_W = 1;//将棋盤の右辺の枠線幅。ban border right width。
        public const int BN_BRD_B_W = 1;

        /// <summary>
        /// SFEN文字列と、出力ファイル名を指定することで、局面の画像ログを出力します。
        /// </summary>
        /// <param name="sfenstring"></param>
        /// <param name="outFile"></param>
        /// <param name="reportEnvironment"></param>
        /// <returns></returns>
        public static bool Write2(
            string sfenstring,
            string relFolder,
            string outFile,
            ReportEnvironment reportEnvironment
            )
        {
            bool successful = true;


            //System.Windows.Forms.MessageBox.Show(sfenstring + "\n" + outFile, "局面PNG書き出し");

            //
            // SFEN → RO_SfenStartpos
            //
            RO_Kyokumen2 ro_SfenStartpos;
            string rest;
            if (!Util_SfenstringReader.ReadString(sfenstring, out rest, out ro_SfenStartpos))
            {
                //System.Windows.Forms.MessageBox.Show(sfenstring,"sfenstringパース失敗");
                successful = false;
                goto gt_EndMethod;
            }

            KyokumenPngWriterImpl.Write1(
                ro_SfenStartpos.ToKyokumen1(),
                relFolder,
                outFile,
                reportEnvironment
                );

        gt_EndMethod:
            return successful;
        }

        /// <summary>
        /// SFEN文字列と、出力ファイル名を指定することで、局面の画像ログを出力します。
        /// </summary>
        /// <param name="sfenstring"></param>
        /// <param name="outFile"></param>
        /// <param name="reportEnvironment"></param>
        /// <returns></returns>
        public static bool Write1(
            RO_Kyokumen1 ro_Kyokumen1,
            string relFolder,
            string outFile,
            ReportEnvironment reportEnvironment
            )
        {
            bool successful = true;

            KyokumenPngWriter repWriter = new KyokumenPngWriterImpl();
            ReportArgs args = new ReportArgsImpl(
                ro_Kyokumen1,
                outFile,
                reportEnvironment
                );

            // 局面画像を描きだします。
            Bitmap bmp = new Bitmap(
                2 * (args.Env.KmW + 2 * args.Env.SjW) + BN_SUJIS * args.Env.KmW + BN_BRD_R_W,
                BN_DANS * args.Env.KmH + BN_BRD_B_W
                );


            repWriter.Paint(Graphics.FromImage(bmp), args);


            //System.Windows.Forms.MessageBox.Show(args.Env.OutFolder + args.OutFile, "bmp.Save");
            string filepath = args.Env.OutFolder + relFolder + args.OutFile;
            // フォルダーが無ければ、作る必要があります。
            {
                DirectoryInfo dirInfo = Directory.GetParent(filepath);
                if (!Directory.Exists(dirInfo.FullName))
                {
                    Directory.CreateDirectory(dirInfo.FullName);
                }
            }
            bmp.Save(filepath);

            return successful;
        }

        private KyokumenPngWriterImpl()
        {
        }

        /// <summary>
        /// 局面を描きます。
        /// </summary>
        public void Paint(Graphics g, ReportArgs args)
        {
            // 8×8 の将棋盤
            int bOx = args.Env.KmW + 2 * args.Env.SjW; // 将棋盤の左辺
            int bOy = 0;
            {

                // 縦線
                for (int col = 0; col < 10; col++)
                {
                    g.DrawLine(Pens.Black, col * args.Env.KmW + bOx, 0 + bOy, col * args.Env.KmW + bOx, 180 + bOy);
                }

                // 横線
                for (int row = 0; row < 10; row++)
                {
                    g.DrawLine(Pens.Black, 0 + bOx, row * args.Env.KmH + bOy, 180 + bOx, row * args.Env.KmH + bOy);
                }
            }

            // 盤上の駒
            {
                for (int suji = 1; suji < 10; suji++)
                {
                    for (int dan = 1; dan < 10; dan++)
                    {
                        string sign = args.Ro_Kyokumen1.Ban[suji][dan];
                        if ("" != sign)
                        {
                            Point pt = this.SignToXy(sign, args);
                            g.DrawImage(Image.FromFile(args.Env.ImgFolder + args.Env.KmFile),
                                new Rectangle((9 - suji) * args.Env.KmW + bOx, (dan - 1) * args.Env.KmH + bOy, args.Env.KmW, args.Env.KmH),//dst
                                new Rectangle(pt.X, pt.Y, args.Env.KmW, args.Env.KmH),//src
                                GraphicsUnit.Pixel
                                );
                        }
                    }
                }
            }

            // 後手の持駒 （飛,角,金,銀,桂,香,歩）
            {
                string[] signs = new string[] { "r", "b", "g", "s", "n", "l", "p" };
                int ox = 0;
                int oy = 0;
                for (int moti = 0; moti < 7; moti++)
                {
                    Point pt = this.SignToXy(signs[moti], args);
                    // 枚数
                    int maisu = args.Ro_Kyokumen1.MotiP2[moti];
                    if (0 < maisu)
                    {
                        //駒
                        g.DrawImage(
                            Image.FromFile(args.Env.ImgFolder + args.Env.KmFile),
                            new Rectangle(ox, (signs.Length - moti - 1) * args.Env.KmH + oy, args.Env.KmW, args.Env.KmH),//dst
                            new Rectangle(pt.X, pt.Y, args.Env.KmW, args.Env.KmH),//src
                            GraphicsUnit.Pixel
                            );

                        // 1桁目が先
                        {
                            int ichi = maisu % 10;
                            g.DrawImage(Image.FromFile(args.Env.ImgFolder + args.Env.SjFile),
                                new Rectangle(ox + args.Env.KmW, (signs.Length - moti - 1) * args.Env.KmH + (args.Env.KmH - args.Env.SjH) + oy, args.Env.SjW, args.Env.SjH),//dst
                                new Rectangle(ichi * args.Env.SjW, args.Env.SjH, args.Env.SjW, args.Env.SjH),//src
                                GraphicsUnit.Pixel
                                );// 一の位
                        }

                        // 2桁目が後
                        {
                            int ju = maisu / 10;
                            if (ju < 1)
                            {
                                ju = -1;//空桁
                            }
                            g.DrawImage(
                                Image.FromFile(args.Env.ImgFolder + args.Env.SjFile),
                                new Rectangle(ox + args.Env.KmW + args.Env.SjW, (signs.Length - moti - 1) * args.Env.KmH + (args.Env.KmH - args.Env.SjH) + oy, args.Env.SjW, args.Env.SjH),//dst
                                new Rectangle(ju * args.Env.SjW, 0, args.Env.SjW, args.Env.SjH),//src
                                GraphicsUnit.Pixel
                                );// 十の位
                        }
                    }
                }
            }

            // 先手の持駒 （飛,角,金,銀,桂,香,歩）
            {
                string[] signs = new string[] { "R", "B", "G", "S", "N", "L", "P" };
                int ox = (args.Env.KmW + 2 * args.Env.SjW) + 9 * args.Env.KmW + BN_BRD_R_W;
                int oy = (9 * args.Env.KmW + BN_BRD_B_W) - 7 * args.Env.KmH;
                for (int moti = 0; moti < 7; moti++)
                {
                    Point pt = this.SignToXy(signs[moti], args);

                    // 枚数
                    int maisu = args.Ro_Kyokumen1.MotiP1[moti];
                    if (0 < maisu)
                    {
                        g.DrawImage(Image.FromFile(args.Env.ImgFolder + args.Env.KmFile),
                            new Rectangle(ox, moti * args.Env.KmH + oy, args.Env.KmW, args.Env.KmH),//dst
                            new Rectangle(pt.X, pt.Y, args.Env.KmW, args.Env.KmH),//src
                            GraphicsUnit.Pixel
                            );//駒

                        // 十の位が先
                        {
                            int ju = maisu / 10;
                            if (ju < 1)
                            {
                                ju = -1;//空桁
                            }
                            g.DrawImage(Image.FromFile(args.Env.ImgFolder + args.Env.SjFile),
                                new Rectangle(ox + args.Env.KmW, moti * args.Env.KmH + (args.Env.KmH - args.Env.SjH) + oy, args.Env.SjW, args.Env.SjH),//dst
                                new Rectangle(ju * args.Env.SjW, 0, args.Env.SjW, args.Env.SjH),//src
                                GraphicsUnit.Pixel
                                );// 十の位
                        }

                        // 一の位が後
                        {
                            int ichi = maisu % 10;
                            g.DrawImage(Image.FromFile(args.Env.ImgFolder + args.Env.SjFile),
                                new Rectangle(ox + args.Env.KmW + args.Env.SjW, moti * args.Env.KmH + (args.Env.KmH - args.Env.SjH) + oy, args.Env.SjW, args.Env.SjH),//dst
                                new Rectangle(ichi * args.Env.SjW, 0, args.Env.SjW, args.Env.SjH),//src
                                GraphicsUnit.Pixel
                                );// 一の位
                        }
                    }
                }
            }
        }

        private Point SignToXy(string sign, ReportArgs args)
        {
            Point pt;

            switch (sign)
            {
                case "P": pt = new Point(0 * args.Env.KmW, 0 * args.Env.KmH); break;
                case "p": pt = new Point(0 * args.Env.KmW, 1 * args.Env.KmH); break;
                case "L": pt = new Point(1 * args.Env.KmW, 0 * args.Env.KmH); break;
                case "l": pt = new Point(1 * args.Env.KmW, 1 * args.Env.KmH); break;
                case "N": pt = new Point(2 * args.Env.KmW, 0 * args.Env.KmH); break;
                case "n": pt = new Point(2 * args.Env.KmW, 1 * args.Env.KmH); break;
                case "S": pt = new Point(3 * args.Env.KmW, 0 * args.Env.KmH); break;
                case "s": pt = new Point(3 * args.Env.KmW, 1 * args.Env.KmH); break;
                case "G": pt = new Point(4 * args.Env.KmW, 0 * args.Env.KmH); break;
                case "g": pt = new Point(4 * args.Env.KmW, 1 * args.Env.KmH); break;
                case "K": pt = new Point(5 * args.Env.KmW, 0 * args.Env.KmH); break;
                case "k": pt = new Point(5 * args.Env.KmW, 1 * args.Env.KmH); break;
                case "R": pt = new Point(6 * args.Env.KmW, 0 * args.Env.KmH); break;
                case "r": pt = new Point(6 * args.Env.KmW, 1 * args.Env.KmH); break;
                case "B": pt = new Point(7 * args.Env.KmW, 0 * args.Env.KmH); break;
                case "b": pt = new Point(7 * args.Env.KmW, 1 * args.Env.KmH); break;
                case "+P": pt = new Point(8 * args.Env.KmW, 0 * args.Env.KmH); break;
                case "+p": pt = new Point(8 * args.Env.KmW, 1 * args.Env.KmH); break;
                case "+L": pt = new Point(9 * args.Env.KmW, 0 * args.Env.KmH); break;
                case "+l": pt = new Point(9 * args.Env.KmW, 1 * args.Env.KmH); break;
                case "+S": pt = new Point(10 * args.Env.KmW, 0 * args.Env.KmH); break;
                case "+s": pt = new Point(10 * args.Env.KmW, 1 * args.Env.KmH); break;
                case "+R": pt = new Point(11 * args.Env.KmW, 0 * args.Env.KmH); break;
                case "+r": pt = new Point(11 * args.Env.KmW, 1 * args.Env.KmH); break;
                case "+B": pt = new Point(12 * args.Env.KmW, 0 * args.Env.KmH); break;
                case "+b": pt = new Point(12 * args.Env.KmW, 1 * args.Env.KmH); break;
                default: pt = Point.Empty; break;
            }

            return pt;
        }

    }
}
