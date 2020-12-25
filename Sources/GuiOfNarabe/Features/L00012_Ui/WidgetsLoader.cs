
namespace Grayscale.Kifuwarazusa.GuiOfNarabe.Features
{
    public interface WidgetsLoader
    {
        string FileName { get; set; }

        void Step1_ReadFile(object obj_shogiGui);
        void Step2_Compile_AllWidget(object obj_shogiGui);
        void Step3_SetEvent(object obj_shogiGui);

    }
}
