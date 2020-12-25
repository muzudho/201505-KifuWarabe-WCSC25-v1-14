namespace Grayscale.Kifuwarazusa.Entities.Features
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
