//スプライト番号

namespace Grayscale.Kifuwarazusa.GuiOfNarabe.Features
{

    /// <summary>
    /// ************************************************************************************************************************
    /// このメインパネルに、何かして欲しいという要求は、ここに入れられます。
    /// ************************************************************************************************************************
    /// </summary>
    public class ResponseImpl : Response
    {

        #region プロパティ類
        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 入力欄のテキストを上書きしたいときに設定(*1)します。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///         *1…ヌルなら、要求フラグは偽になります。
        /// </summary>
        public string InputTextString
        {
            get
            {
                return this.inputTextString;
            }
            set
            {
                string str = value;

                if (null == str)
                {
                    this.canInputTextFlag = false;
                }
                else
                {
                    this.canInputTextFlag = true;
                }

                this.inputTextString = value;
            }
        }
        private string inputTextString;

        /// <summary>
        /// フラグ。読取専用。
        /// </summary>
        public bool CanInputTextFlag
        {
            get
            {
                return this.canInputTextFlag;
            }
        }
        private bool canInputTextFlag;

        /// <summary>
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 入力欄の後ろにテキストを付け足したいときに設定(*1)します。
        /// ------------------------------------------------------------------------------------------------------------------------
        /// 
        ///         *1…ヌルなら、要求フラグは偽になります。
        /// 
        /// </summary>
        public string AppendInputTextString
        {
            get
            {
                return this.appendInputTextString;
            }
        }

        public void SetAppendInputTextString(string value)
        {
            if (null == value)
            {
                this.canAppendInputTextFlag = false;
            }
            else
            {
                this.canAppendInputTextFlag = true;
            }
            this.appendInputTextString = value;
        }
        private string appendInputTextString;

        /// <summary>
        /// フラグ。読取専用。
        /// </summary>
        public bool CanAppendInputTextFlag
        {
            get
            {
                return canAppendInputTextFlag;
            }
        }
        private bool canAppendInputTextFlag;



        /// <summary>
        ///------------------------------------------------------------------------------------------------------------------------
        /// 出力欄を更新したいとき。
        ///------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public ResponseGedanTxt OutputTxt
        {
            get;
            set;
        }

        /// <summary>
        ///------------------------------------------------------------------------------------------------------------------------
        /// メインパネルを再描画したいときは、真にしてください。
        ///------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public void ToRedraw()
        {
            this.redraw = true;
        }
        public void ClearRedraw()
        {
            this.redraw = false;
        }
        public bool IsRedraw()
        {
            return this.redraw;
        }
        private bool redraw;

        /// <summary>
        ///------------------------------------------------------------------------------------------------------------------------
        /// 手番が交代していたら真です。
        ///------------------------------------------------------------------------------------------------------------------------
        /// </summary>
        public bool ChangedTurn
        {
            get;
            set;
        }

        /// <summary>
        ///------------------------------------------------------------------------------------------------------------------------
        /// リドローして欲しい駒
        ///------------------------------------------------------------------------------------------------------------------------
        ///
        /// 要素は駒ハンドル。
        /// 
        /// </summary>
        public void RedrawStarlights()
        {
            this.redrawStarlights = true;
        }
        public void Clear_RedrawStarlights()
        {
            this.redrawStarlights = false;
        }
        public bool Is_RedrawStarlights()
        {
            return this.redrawStarlights;
        }
        private bool redrawStarlights;

        #endregion


        /// <summary>
        /// ************************************************************************************************************************
        /// コンストラクタです。
        /// ************************************************************************************************************************
        /// </summary>
        public ResponseImpl()
        {
            this.OutputTxt = ResponseGedanTxt.None;
        }

    }


}
