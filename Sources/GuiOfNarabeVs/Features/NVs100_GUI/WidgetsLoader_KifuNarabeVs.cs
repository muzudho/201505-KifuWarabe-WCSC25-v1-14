using Grayscale.Kifuwarazusa.GuiOfNarabe.Features;
using Grayscale.Kifuwarazusa.GuiOfNarabe.Gui;

namespace Grayscale.Kifuwarazusa.GuiOfNarabeVs.Features
{
    public class WidgetsLoader_KifuNarabeVs : WidgetsLoader_KifuNarabe
    {

        public WidgetsLoader_KifuNarabeVs(string fileName) : base(fileName)
        {
        }

        public override void Step3_SetEvent(object obj_shogiGui)
        {
            NarabeRoomViewModel shogiGui1 = (NarabeRoomViewModel)obj_shogiGui;

            //----------
            // 将棋エンジン起動ボタン
            //----------
            {
                UserWidget widget = shogiGui1.Shape_PnlTaikyoku.GetWidget("BtnShogiEngineKido");
                widget.Delegate_MouseHitEvent = (
                    object obj_shogiGui2
                    , Shape_BtnKoma btnKoma_Selected
                    ) =>
                {
                    NarabeRoomViewModel shogiGui = (NarabeRoomViewModel)obj_shogiGui2;

                    Ui_PnlMain ui_PnlMain = ((Ui_ShogiForm1)shogiGui.OwnerForm).Ui_PnlMain1;

                    ui_PnlMain.ShogiGui.Start(ui_PnlMain.SetteiFile.ShogiEngineFilePath);
                };
            }

        }

    }
}
