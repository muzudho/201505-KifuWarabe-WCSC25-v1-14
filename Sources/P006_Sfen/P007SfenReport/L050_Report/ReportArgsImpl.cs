using System.Diagnostics;
using Grayscale.P006Sfen;
using Grayscale.P007_SfenReport.L00025_Report;

namespace Grayscale.P007_SfenReport.L050_Report
{


    public class ReportArgsImpl : ReportArgs
    {
        /// <summary>
        /// 特に変わらない設定。
        /// </summary>
        public ReportEnvironment Env { get { return this.env; } }
        private ReportEnvironment env;

        /// <summary>
        /// 出力ファイルへのパス。
        /// </summary>
        public string OutFile { get { return this.outFile; } }
        private string outFile;

        public ISfenPosition1 Ro_Kyokumen1 { get { return this.ro_Kyokumen1; } }
        private ISfenPosition1 ro_Kyokumen1;


        public ReportArgsImpl(
            ISfenPosition1 ro_Kyokumen1,
            
            string outFile,  
            
            ReportEnvironment reportEnvironment)
        {
            this.ro_Kyokumen1 = ro_Kyokumen1;

            // デバッグ
            {
                Debug.Assert(this.ro_Kyokumen1.Ban.Length == 10, "サイズ違反");
                Debug.Assert(this.ro_Kyokumen1.Ban[0].Length == 10, "サイズ違反");
                Debug.Assert(this.ro_Kyokumen1.Ban[1].Length == 10, "サイズ違反");
                Debug.Assert(this.ro_Kyokumen1.Ban[2].Length == 10, "サイズ違反");
                Debug.Assert(this.ro_Kyokumen1.Ban[3].Length == 10, "サイズ違反");
                Debug.Assert(this.ro_Kyokumen1.Ban[4].Length == 10, "サイズ違反");
                Debug.Assert(this.ro_Kyokumen1.Ban[5].Length == 10, "サイズ違反");
                Debug.Assert(this.ro_Kyokumen1.Ban[6].Length == 10, "サイズ違反");
                Debug.Assert(this.ro_Kyokumen1.Ban[7].Length == 10, "サイズ違反");
                Debug.Assert(this.ro_Kyokumen1.Ban[8].Length == 10, "サイズ違反");
                Debug.Assert(this.ro_Kyokumen1.Ban[9].Length == 10, "サイズ違反");
            }

            this.outFile = outFile;
            this.env = reportEnvironment;
        }
    }


}
