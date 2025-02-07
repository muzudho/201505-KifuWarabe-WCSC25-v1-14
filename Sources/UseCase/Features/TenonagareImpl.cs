﻿using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.UseCases.Features
{

    /// <summary>
    /// 頭の隅に置いてある、手の流れ。
    /// 
    /// 数局面にまたぐ、データです。
    /// </summary>
    public class TenonagareImpl : TenonagareGenjoImpl, Tenonagare
    {

        /// <summary>
        /// 妄想ログ。
        /// </summary>
        public ResultKioku ResultKioku { get; set; }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="save_node_OrNull"></param>
        /// <param name="owner"></param>
        /// <param name="scoreKeisu">評価値に掛け算します。通常、1.0d を指定してください。</param>
        public TenonagareImpl(
            ISfenPosition1 ro_Kyokumen1ForKioku_OrNull,
            TenonagareName name, double scoreKeisu, RO_Star_Koma koma1, RO_Star_Koma koma2, Basho masu
            )
            : base(name, scoreKeisu, koma1, koma2, masu)
        {
            if (null != ro_Kyokumen1ForKioku_OrNull)
            {
                this.ResultKioku = new ResultKioku(ro_Kyokumen1ForKioku_OrNull.ToSfenstring(true));
            }
            else
            {
                this.ResultKioku = new ResultKioku(null);
            }

        }



        public Json_Val ToJsonVal()
        {
            Json_Obj obj = new Json_Obj();

            obj.Add(new Json_Prop("name", this.Name.ToString()));

            //TODO: obj.Add(new Json_Prop("finger", (int)this.Finger));//sprite

            obj.Add(new Json_Prop("masu", (int)this.Masu.MasuNumber));

            if (null != this.ResultKioku)
            {
                obj.Add(new Json_Prop("log", this.ResultKioku.ToJsonVal()));
            }

            return obj;
        }

    }
}
