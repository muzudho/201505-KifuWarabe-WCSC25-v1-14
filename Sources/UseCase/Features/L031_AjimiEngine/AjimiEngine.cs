using Grayscale.Kifuwarazusa.Entities.Logging;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.P050_KifuWarabe.L00025_UsiLoop;

namespace Grayscale.P050_KifuWarabe.L031_AjimiEngine
{
    /// <summary>
    /// 味見エンジン。
    /// </summary>
    public class AjimiEngine
    {
        private ShogiEngine owner;


        public AjimiEngine(ShogiEngine owner)
        {
            this.owner = owner;
        }

        public Result_Ajimi Ajimi(SkyConst src_Sky)
        {
            Result_Ajimi result_Ajimi = Result_Ajimi.Empty;

            RO_Star_Koma koma1 = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(Finger_Honshogi.SenteOh).Now);
            RO_Star_Koma koma2 = Util_Koma.AsKoma(src_Sky.StarlightIndexOf(Finger_Honshogi.GoteOh).Now);
            Logger.Trace("将棋サーバー「ではここで、王さまがどこにいるか確認してみましょう」");
            Logger.Trace("▲王の置き場＝" + Util_Masu.GetOkiba(koma1.Masu));
            Logger.Trace("△王の置き場＝" + Util_Masu.GetOkiba(koma2.Masu));

            if (Util_Masu.GetOkiba(koma1.Masu) != Okiba.ShogiBan)
            {
                // 先手の王さまが将棋盤上にいないとき☆
                result_Ajimi = Result_Ajimi.Lost_SenteOh;
            }
            else if (Util_Masu.GetOkiba(koma2.Masu) != Okiba.ShogiBan)
            {
                // または、後手の王さまが将棋盤上にいないとき☆
                result_Ajimi = Result_Ajimi.Lost_GoteOh;
            }
            else
            {
                result_Ajimi = Result_Ajimi.Empty;
            }

            return result_Ajimi;
        }

    }
}
