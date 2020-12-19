using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P050_KifuWarabe.L030_Shogisasi;
using System.Collections.Generic;
using Grayscale.P025_KifuLarabe.L00050_StructShogi;
using Grayscale.Kifuwarazusa.Entities.Logging;

namespace Grayscale.P050_KifuWarabe.L00049_Kokoro
{


    public interface Kokoro
    {

        /// <summary>
        /// 「移動」タイプの狙いを作成します☆ｗ
        /// </summary>
        /// <param name="input_myPside"></param>
        /// <param name="save_node">狙い作成時の局面を状況として記録するために読取します。</param>
        /// <param name="forDebug_kifu"></param>
        /// <param name="input_seikaku"></param>
        void Omoituki(
            Playerside input_myPside,
            KifuNode save_node,
            Seikaku input_seikaku
            );



        /// <summary>
        /// 頭の隅コレクションのWrite。
        /// </summary>
        /// <param name="obj_sikouEngine"></param>
        void WriteTenonagare(object obj_sikouEngine, ILogTag logTag);

        Json_Val TenonagareToJsonVal();

        void ReadTenonagare();

        /// <summary>
        /// 頭の隅に置いてある、いくつかの手の流れ。
        /// </summary>
        List<Tenonagare> TenonagareItems { get; set; }

        void ClearTenonagare();

        void AddTenonagare(Tenonagare kiokuHow);

    }


}
