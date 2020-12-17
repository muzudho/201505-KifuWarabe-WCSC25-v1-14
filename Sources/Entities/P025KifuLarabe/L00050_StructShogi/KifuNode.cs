﻿using Grayscale.Kifuwarazusa.Entities;
using Grayscale.P006Sfen;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L012_Common;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号

namespace Grayscale.P025_KifuLarabe.L00050_StructShogi
{
    public interface KifuNode : Node<ShootingStarlightable, KyokumenWrapper>
    {


        ISfenPosition1 ToRO_Kyokumen1(ILogTag logTag);
        string ToSfenstring(Playerside pside, ILogTag logTag);

        void AppdendNextNodes(Node<ShootingStarlightable, KyokumenWrapper> hubNode);


        /// <summary>
        /// 棋譜に符号を追加します。
        /// 
        /// KifuIO を通して使ってください。
        /// 
        /// ①コマ送り用。
        /// ②「成り」フラグの更新用。
        /// ③マウス操作用
        /// 
        /// カレントノードは変更しません。
        /// </summary>
        void AppendChildA_New(Node<ShootingStarlightable, KyokumenWrapper> newNode);

        /// <summary>
        /// 王手がかかった局面は取り除きます。
        /// </summary>
        Maps_OneAndMulti<Finger, ShootingStarlightable> SplitMoveByKoma(Node<ShootingStarlightable, KyokumenWrapper> hubNode, ILogTag logTag);

        string Json_NextNodes_MultiSky(
            string memo,
            string hint,
            int tesumi_yomiGenTeban_forLog,//読み進めている現在の手目済
            ILogTag logTag
            );

        /// <summary>
        /// 局面評価明細。
        /// </summary>
        KyHyoka KyHyoka { get; }
        /// <summary>
        /// 枝専用。
        /// </summary>
        /// <param name="branchKyHyoka"></param>
        void SetBranchKyHyoka(KyHyoka branchKyHyoka);
        Playerside Tebanside { get; }
    }
}
