using Grayscale.P025_KifuLarabe.L00012_Atom;
using Grayscale.P025_KifuLarabe.L012_Common;
using System.Collections.Generic;
using System.Diagnostics;

namespace Grayscale.P025_KifuLarabe.L00025_Struct
{

    /// <summary>
    /// ノードです。
    /// </summary>
    public class NodeImpl<T1, T2> : Node<T1, T2>
    {



        /// <summary>
        /// 配列型。[0]平手局面、[1]１手目の局面……。リンクリスト→ツリー構造の順に移行を進めたい。
        /// </summary>
        public T2 Value { get; set; }

        public Node<T1, T2> PreviousNode { get; set; }

        public T1 Key
        {
            get
            {
                return this.teMove;
            }
        }
        private T1 teMove;

        /// <summary>
        /// キー：SFEN ※この仕様は暫定
        /// 値：ノード
        /// </summary>
        private Dictionary<string, Node<T1, T2>> NextNodes { get; set; }
        public delegate void DELEGATE_NextNodes(string key, Node<T1, T2> node, ref bool toBreak);


        //public NodeImpl()
        //{
        //}


        public void Foreach_NextNodes(NodeImpl<T1, T2>.DELEGATE_NextNodes delegate_NextNodes)
        {
            bool toBreak = false;

            foreach (KeyValuePair<string, Node<T1, T2>> entry in this.NextNodes)//Foreach
            {
                delegate_NextNodes(entry.Key, entry.Value, ref toBreak);

                if (toBreak)
                {
                    break;
                }
            }
        }

        public void Clear_NextNodes()
        {
            this.NextNodes.Clear();
        }

        public bool ContainsKey_NextNodes(string key)
        {
            return this.NextNodes.ContainsKey(key);
        }

        public void Add_NextNode(string key,Node<T1, T2> newNode)
        {
            this.NextNodes.Add(key,newNode);
        }

        public void Set_NextNodes(Dictionary<string, Node<T1, T2>> newNextNodes)
        {
            this.NextNodes = newNextNodes;
        }

        public int Count_NextNodes
        {
            get
            {
                return this.NextNodes.Count;
            }
        }


        public NodeImpl(T1 teMove, T2 sky)
        {
            this.PreviousNode = null;
            this.teMove = teMove;
            this.Value = sky;
            this.NextNodes = new Dictionary<string, Node<T1, T2>>();
        }

        public Json_Val ToJsonVal()
        {
            Json_Obj obj = new Json_Obj();

            KyokumenWrapper kWrap = this.Value as KyokumenWrapper;
            if (null != kWrap)
            {
                // TODO: ログが大きくなるので、１行で出力したあとに改行にします。

                Json_Prop prop = new Json_Prop("kyokumen", Util_Sky.ToJsonVal(kWrap.ToKyokumenConst));
                obj.Add(prop);
            }
            else
            {
                Debug.Fail("this.Value as KyokumenWrapper じゃなかった。");
            }

            return obj;
        }
    }


}
