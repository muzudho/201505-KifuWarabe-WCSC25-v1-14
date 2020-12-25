
namespace Grayscale.P050_KifuWarabe.L030_Shogisasi
{

    /// <summary>
    /// 思考エンジンの性格です。
    /// </summary>
    public class Seikaku
    {

        /// <summary>
        /// 妄想実現までの粘り強さ。
        /// 
        /// とりあえず 1～5。
        /// 5なら、妄想が実現できていなくても、5回は繰り返します。
        /// </summary>
        public int NebariDuyosa { get; set; }

        public Seikaku()
        {
            // 粘り強さの初期値 2。
            this.NebariDuyosa = 2;
        }
    }
}
