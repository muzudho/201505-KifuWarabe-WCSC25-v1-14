﻿namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public class ReportEnvironmentImpl : ReportEnvironment
    {
        /// <summary>
        /// 駒画像ファイル。
        /// </summary>
        public string KmFile { get { return this.kmFile; } }
        private string kmFile;

        /// <summary>
        /// 数字画像ファイル。
        /// </summary>
        public string SjFile { get { return this.sjFile; } }
        private string sjFile;


        /// <summary>
        /// 駒の横幅
        /// </summary>
        public int KmW { get { return this.kmW; } }
        private int kmW;

        /// <summary>
        /// 駒の縦幅
        /// </summary>
        public int KmH { get { return this.kmH; } }
        private int kmH;

        /// <summary>
        /// 数字の横幅
        /// </summary>
        public int SjW { get { return this.sjW; } }
        private int sjW;

        /// <summary>
        /// 数字の縦幅
        /// </summary>
        public int SjH { get { return this.sjH; } }
        private int sjH;


        public ReportEnvironmentImpl(
            string kmFile, string sjFile,
            string kmW_str, string kmH_str, string sjW_str, string sjH_str
            )
        {
            this.kmFile = kmFile;
            this.sjFile = sjFile;

            int kmW;
            if (!int.TryParse(kmW_str, out kmW))
            {
                kmW = 1;
            }
            this.kmW = kmW;

            int kmH;
            if (!int.TryParse(kmH_str, out kmH))
            {
                kmH = 1;
            }
            this.kmH = kmH;

            int sjW;
            if (!int.TryParse(sjW_str, out sjW))
            {
                sjW = 1;
            }
            this.sjW = sjW;

            int sjH;
            if (!int.TryParse(sjH_str, out sjH))
            {
                sjH = 1;
            }
            this.sjH = sjH;
        }

    }
}
