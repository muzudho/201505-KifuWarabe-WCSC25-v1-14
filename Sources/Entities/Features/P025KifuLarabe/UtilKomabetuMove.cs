﻿#if DEBUG
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using Grayscale.Kifuwarazusa.Entities.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.Kifuwarazusa.Entities.Features;
#else
using System.Collections.Generic;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
#endif

namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public abstract class UtilKomabetuMove
    {
        /// <summary>
        /// 次の局面の一覧をもった、入れ物ノードを返します。
        /// </summary>
        /// <param name="kifu"></param>
        /// <param name="pside_genTeban"></param>
        /// <returns></returns>
        public static KifuNode ToNextNodes_AsHubNode(
            Maps_OneAndMulti<Finger, ShootingStarlightable> komabetuAllMove,
            Node<ShootingStarlightable, KyokumenWrapper> siteiNode,
            Playerside pside_genTeban)
        {
            KifuNode hubNode = new KifuNodeImpl(null, null, Playerside.Empty);//蝶番

#if DEBUG
            string dump = komabetuAllMove.Dump();
#endif

            foreach (KeyValuePair<Finger, List<ShootingStarlightable>> entry1 in komabetuAllMove.Items)
            {

                Finger figKoma = entry1.Key;// 駒

                // 駒の動ける升全て
                foreach (ShootingStarlightable move in entry1.Value)
                {
                    RO_Star_Koma koma = Util_Koma.AsKoma(move.Now);

                    SyElement masu = koma.Masu;

                    SkyConst nextSky = Util_Sasu.Sasu(siteiNode.Value.ToKyokumenConst, figKoma, masu, pside_genTeban);

                    Node<ShootingStarlightable, KyokumenWrapper> nextNode = new KifuNodeImpl(move, new KyokumenWrapper(nextSky), KifuNodeImpl.GetReverseTebanside(pside_genTeban));//次のノード

                    string sfenText = Util_Sky.ToSfenMoveText(move);
                    if (hubNode.ContainsKey_NextNodes(sfenText))
                    {
                        // 既存の指し手なら無視
                        System.Console.WriteLine("既存の指し手なので無視します。sfenText=[" + sfenText + "]");
                    }
                    else
                    {
                        hubNode.Add_NextNode(Util_Sky.ToSfenMoveText(move), nextNode);
                    }

                }
            }

            return hubNode;
        }

    }
}
