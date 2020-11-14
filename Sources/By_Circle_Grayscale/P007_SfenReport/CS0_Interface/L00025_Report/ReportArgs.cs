using Grayscale.P006_Sfen.L0005_Sfen;

namespace Grayscale.P007_SfenReport.L00025_Report
{
    public interface ReportArgs
    {

        ReportEnvironment Env { get; }

        /// <summary>
        /// 出力ファイルへのパス。
        /// </summary>
        string OutFile { get; }

        RO_Kyokumen1 Ro_Kyokumen1 { get; }

    }
}
