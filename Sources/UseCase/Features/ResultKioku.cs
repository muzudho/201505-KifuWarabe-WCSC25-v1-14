using Grayscale.Kifuwarazusa.Entities.Features;

namespace Grayscale.Kifuwarazusa.UseCases.Features
{

    /// <summary>
    /// 妄想ログ。戦法の良し悪しを覚えます。
    /// </summary>
    public class ResultKioku
    {
        /// <summary>
        /// この戦法が始まったときの状況。ノード。
        /// 
        /// ヌル可。全局面で適用可能な駒得など。
        /// </summary>
        public string StartingSfenstringForKioku { get { return startingSfenstringForKioku; } }
        private string startingSfenstringForKioku;

        /// <summary>
        /// 失敗回数
        /// </summary>
        public int Sippai { get; set; }

        /// <summary>
        /// 工程が、ちょっとでもうまく進んだ回数。
        /// </summary>
        public int Susunda { get; set; }

        /// <summary>
        /// 成功回数
        /// </summary>
        public int Seiko { get; set; }


        public ResultKioku(string sfenstringForKioku)
        {
            this.startingSfenstringForKioku = sfenstringForKioku;
            this.Sippai = 0;
            this.Susunda = 0;
            this.Seiko = 0;
        }

        public Json_Val ToJsonVal()
        {
            Json_Obj obj = new Json_Obj();


            obj.Add(new Json_Prop("sippai", this.Sippai));


            obj.Add(new Json_Prop("susunda", this.Susunda));


            obj.Add(new Json_Prop("seiko", this.Seiko));


            if (null != this.StartingSfenstringForKioku)
            {
                // 長いログになるので、後ろに回しました。
                obj.Add(new Json_Prop("start_node2", this.StartingSfenstringForKioku));
                //obj.Add(new Json_Prop("start_node", node.ToJsonVal()));
            }


            return obj;
        }

    }
}
