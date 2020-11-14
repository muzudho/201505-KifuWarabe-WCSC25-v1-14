
namespace Grayscale.P200_KifuNarabe.L00048_ShogiGui
{
    public interface Response
    {

        string AppendInputTextString { get; }

        /// <summary>
        /// フラグ。読取専用。
        /// </summary>
        bool CanAppendInputTextFlag { get; }


        /// <summary>
        /// フラグ。読取専用。
        /// </summary>
        bool CanInputTextFlag { get; }

        /// <summary>
        ///------------------------------------------------------------------------------------------------------------------------
        /// 手番が交代していたら真です。
        ///------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        bool ChangedTurn { get; set; }

        void Clear_RedrawStarlights();

        void ClearRedraw();
        
        string InputTextString { get; set; }


        bool Is_RedrawStarlights();

        bool IsRedraw();

        /// <summary>
        ///------------------------------------------------------------------------------------------------------------------------
        /// 出力欄を更新したいとき。
        ///------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        ResponseGedanTxt OutputTxt { get; set; }

        void RedrawStarlights();

        void SetAppendInputTextString(string value);

        
        /// <summary>
        ///------------------------------------------------------------------------------------------------------------------------
        /// メインパネルを再描画したいときは、真にしてください。
        ///------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        void ToRedraw();

    }
}
