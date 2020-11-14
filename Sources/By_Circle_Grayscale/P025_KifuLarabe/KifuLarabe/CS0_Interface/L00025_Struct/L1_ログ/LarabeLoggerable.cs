﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grayscale.P025_KifuLarabe.L00025_Struct
{
    public interface LarabeLoggerable
    {
        string FileName { get; }
        string FileNameWoe { get; }
        string Extension { get; }

        /// <summary>
        /// ログ出力の有無。
        /// </summary>
        bool Enable { get; }

        bool Print_TimeStamp { get; }



        /// <summary>
        /// ************************************************************************************************************************
        /// メモを、ログ・ファイルの末尾に追記します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="line"></param>
        void WriteLine_AddMemo(string line);

                /// <summary>
        /// ************************************************************************************************************************
        /// エラーを、ログ・ファイルに記録します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="line"></param>
        void WriteLine_Error(string line);



        /// <summary>
        /// ************************************************************************************************************************
        /// メモを、ログ・ファイルに記録します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="line"></param>
        void WriteLine_OverMemo(string line);



        /// <summary>
        /// ************************************************************************************************************************
        /// サーバーへ送ったコマンドを、ログ・ファイルに記録します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="line"></param>
        void WriteLine_S(string line);


        /// <summary>
        /// ************************************************************************************************************************
        /// サーバーから受け取ったコマンドを、ログ・ファイルに記録します。
        /// ************************************************************************************************************************
        /// </summary>
        /// <param name="line"></param>
        void WriteLine_R(string line);

    }
}
