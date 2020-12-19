using System.Collections.Generic;
using System.Windows.Forms;
using Grayscale.Kifuwarazusa.Entities.Features.Gui;
using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P200_KifuNarabe.L00006_Shape;
using Grayscale.P200_KifuNarabe.L00012_Ui;
using Grayscale.P200_KifuNarabe.L00047_Scene;
using Grayscale.P200_KifuNarabe.L00048_ShogiGui;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.Kifuwarazusa.GuiOfNarabe.Gui
{
    public interface NarabeRoomViewModel : IRoomViewModel
    {
        Timed TimedA { get; set; }
        Timed TimedB { get; set; }
        Timed TimedC { get; set; }
        void Timer_Tick();

        Response ResponseData { get; set; }

        /// <summary>
        /// 使い方：((Ui_Form1)this.OwnerForm)
        /// </summary>
        Form OwnerForm { get; set; }



        List<WidgetsLoader> WidgetLoaders { get; set; }

        /// <summary>
        /// ************************************************************************************************************************
        /// 手番が替わったときの挙動を、ここに書きます。
        /// ************************************************************************************************************************
        /// </summary>
        void ChangeTurn(ILogTag logTag);

        /// <summary>
        /// 将棋エンジンに、終了するように促します。
        /// </summary>
        void Shutdown();

        
        /// <summary>
        /// 将棋エンジンに、ログを出すように促します。
        /// </summary>
        void Logdase();

        
        /// <summary>
        /// ************************************************************************************************************************
        /// 将棋エンジンを起動します。
        /// ************************************************************************************************************************
        /// </summary>
        void Start(string shogiEngineFilePath);


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// グラフィックを描くツールは全部この中です。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        Shape_PnlTaikyoku Shape_PnlTaikyoku { get; }


        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// ゲームの流れの状態遷移図はこれです。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        SceneName Scene { get; }
        void SetScene(SceneName scene);


        void Response(string mutexString, ILogTag logTag);



        string Input99 { get; set; }

        RO_Star_Koma GetKoma(Finger finger);

    }
}
