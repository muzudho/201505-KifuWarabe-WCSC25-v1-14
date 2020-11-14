using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L012_Common;
using Grayscale.P025_KifuLarabe.L00012_Atom;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //スプライト番号
using Grayscale.P006_Sfen.L0005_Sfen;

namespace Grayscale.P025_KifuLarabe.L00050_StructShogi
{
    public interface KifuNode : Node<ShootingStarlightable, KyokumenWrapper>
    {


        RO_Kyokumen1 ToRO_Kyokumen1(LarabeLoggerable logTag);
        string ToSfenstring(Playerside pside, LarabeLoggerable logTag);

        void AppdendNextNodes(Node<ShootingStarlightable, KyokumenWrapper> hubNode);


        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜に符号を追加します。
        /// ************************************************************************************************************************
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
        Maps_OneAndMulti<Finger, ShootingStarlightable> SplitSasite_ByKoma(Node<ShootingStarlightable, KyokumenWrapper> hubNode, LarabeLoggerable logTag);

        string Json_NextNodes_MultiSky(
            string memo,
            string hint,
            int tesumi_yomiGenTeban_forLog,//読み進めている現在の手目済
            LarabeLoggerable logTag
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
