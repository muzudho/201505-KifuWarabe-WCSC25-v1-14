using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P200_KifuNarabe.L00006_Shape;
using Grayscale.P200_KifuNarabe.L00048_ShogiGui;
using Grayscale.P200_KifuNarabe.L100_GUI;

namespace Grayscale.P400_KifuNaraVs.L100_GUI
{
    public class WidgetsLoader_KifuNarabeVs : WidgetsLoader_KifuNarabe
    {

        public WidgetsLoader_KifuNarabeVs(string fileName):base(fileName)
        {
        }

        public override void Step3_SetEvent(object obj_shogiGui)
        {
            ShogiGui shogiGui1 = (ShogiGui)obj_shogiGui;

            //----------
            // 将棋エンジン起動ボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnShogiEngineKido");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    , ILogTag logTag
                    ) =>
                {
                    ShogiGui shogiGui = (ShogiGui)obj_shogiGui2;

                    Ui_PnlMain ui_PnlMain = ((Ui_ShogiForm1)shogiGui.OwnerForm).Ui_PnlMain1;

                    ui_PnlMain.ShogiGui.Start(ui_PnlMain.SetteiFile.ShogiEngineFilePath);
                };
            }

        }

    }
}
