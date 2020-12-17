using Grayscale.Kifuwarazusa.Entities;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L004_StructShogi;
using Grayscale.P025_KifuLarabe.L012_Common;
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
            Logger.Log_Engine.WriteLine_AddMemo("将棋サーバー「ではここで、王さまがどこにいるか確認してみましょう」");
            Logger.Log_Engine.WriteLine_AddMemo("▲王の置き場＝" + Util_Masu.GetOkiba(koma1.Masu));
            Logger.Log_Engine.WriteLine_AddMemo("△王の置き場＝" + Util_Masu.GetOkiba(koma2.Masu));



            if(Util_Masu.GetOkiba(koma1.Masu) != Okiba.ShogiBan)
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
