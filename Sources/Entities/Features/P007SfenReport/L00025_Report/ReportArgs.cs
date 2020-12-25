using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.P007_SfenReport.L00025_Report
{
    public interface ReportArgs
    {

        ReportEnvironment Env { get; }

        /// <summary>
        /// 出力ファイルへのパス。
        /// </summary>
        string OutFileFullName { get; }

        ISfenPosition1 Ro_Kyokumen1 { get; }

    }
}
