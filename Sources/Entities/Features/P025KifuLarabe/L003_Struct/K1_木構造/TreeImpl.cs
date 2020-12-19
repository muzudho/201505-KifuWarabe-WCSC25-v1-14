using System;
using System.Collections.Generic;
using Grayscale.P025_KifuLarabe.L00025_Struct;


namespace Grayscale.P025_KifuLarabe.L00025_Struct
{

    /// <summary>
    /// 棋譜。
    /// </summary>
    public class TreeImpl<
        T1,//ノードのキー
        T2//ノードの値
        > : Tree<
        T1,//ノードのキー
        T2//ノードの値
        >
    {
        #region プロパティ類

        /// <summary>
        /// ツリー構造になっている本譜の葉ノード。
        /// 根を「startpos」等の初期局面コマンドとし、次の節からは棋譜の符号「2g2f」等が連なっている。
        /// </summary>
        public Node<T1, T2> CurNode { get; set; }

        ///// <summary>
        ///// ------------------------------------------------------------------------------------------------------------------------
        ///// 一手目の手番
        ///// ------------------------------------------------------------------------------------------------------------------------
        ///// 
        /////     １手目を指す手番の、先後を指定してください。
        /////
        /////     ルート局面には　平手初期局面　が入っていると想定していますので、
        /////     ２個目のノードが　一手目の局面と想定しています。
        ///// 
        /////     TODO:プロパティに「pside=black」のように「文字列,object」で入れたい。
        ///// 
        ///// </summary>
        //public Object Pside_FirstSky{get;set;}

        /// <summary>
        /// 使い方自由。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetProperty(string key,object value)
        {
            if (this.properties.ContainsKey(key))
            {
                this.properties[key] = value;
            }
            else
            {
                this.properties.Add(key,value);
            }
        }

        /// <summary>
        /// 使い方自由。
        /// </summary>
        public object GetProperty(string key)
        {
            object result;

            if (this.properties.ContainsKey(key))
            {
                result = this.properties[key];
            }
            else
            {
                result = "Unknown kifu property [" + key + "]";
            }

            return result;
        }
        private Dictionary<string,object> properties;

        #endregion





        public TreeImpl(
            Node<T1, T2> root
            )
        {
            this.properties = new Dictionary<string,object>();
            this.CurNode = root;
        }



        #region カレント移動系

        public void Move_Previous()
        {
            this.CurNode = this.CurNode.PreviousNode;
        }

        #endregion


        /// <summary>
        /// ************************************************************************************************************************
        /// 現在の要素を切り取って返します。なければヌル。
        /// ************************************************************************************************************************
        /// 
        /// カレントは、１手前に戻ります。
        /// 
        /// </summary>
        /// <returns>ルートしかないリストの場合、ヌルを返します。</returns>
        public Node<T1, T2> PopCurrentNode()
        {
            Node<T1, T2> deleteeElement = null;

            if (this.CurNode.PreviousNode==null)
            {
                // やってはいけない操作は、例外を返すようにします。
                throw new Exception("ルート局面を削除しようとしました。");
            }

            //>>>>> ラスト要素がルートでなかったら

            // 一手前の要素（必ずあるはずです）
            deleteeElement = this.CurNode;
            // 残されたリストの最後の要素の、次リンクを切ります。
            deleteeElement.PreviousNode.Clear_NextNodes();

            // カレントを、１つ前の要素に替えます。
            this.CurNode = deleteeElement.PreviousNode;

            return deleteeElement;
        }



        /// <summary>
        /// 本譜だけ。
        /// </summary>
        /// <param name="endNode"></param>
        /// <param name="delegate_Foreach"></param>
        public void ForeachHonpu(Node<T1, T2> endNode, DELEGATE_Foreach<T1, T2> delegate_Foreach)
        {
            bool toBreak = false;

            List<Node<T1, T2>> list8 = new List<Node<T1, T2>>();

            //
            // ツリー型なので、１本のリストに変換するために工夫します。
            //
            // カレントからルートまで遡り、それを逆順にすれば、本譜になります。
            //

            while (null!=endNode)//ルートを含むところまで遡ります。
            {
                list8.Add(endNode); // リスト作成

                endNode = endNode.PreviousNode;
            }


            list8.Reverse();

            int tesumi = 0;//初期局面が[0]

            foreach (Node<T1, T2> item in list8)//正順になっています。
            {
                T2 sky = item.Value;

                delegate_Foreach(tesumi, sky, item, ref toBreak);
                if (toBreak)
                {
                    break;
                }

                tesumi++;
            }
        }
        /// <summary>
        /// 全て。
        /// </summary>
        /// <param name="endNode"></param>
        /// <param name="delegate_Foreach"></param>
        public void ForeachZenpuku(Node<T1, T2> startNode, DELEGATE_Foreach<T1, T2> delegate_Foreach)
        {

            List<Node<T1, T2>> list8 = new List<Node<T1, T2>>();

            //
            // ツリー型なので、１本のリストに変換するために工夫します。
            //
            // カレントからルートまで遡り、それを逆順にすれば、本譜になります。
            //

            int tesumi = 0;//※指定局面が0。
            bool toFinish_ZenpukuTansaku = false;

            this.Recursive_Node_NextNode(tesumi, startNode, delegate_Foreach, ref toFinish_ZenpukuTansaku);
            if (toFinish_ZenpukuTansaku)
            {
                goto gt_EndMetdhod;
            }

        gt_EndMetdhod:
            ;
        }

        private void Recursive_Node_NextNode(int tesumi1, Node<T1, T2> node1, DELEGATE_Foreach<T1, T2> delegate_Foreach1, ref bool toFinish_ZenpukuTansaku)
        {
            bool toBreak1 = false;

            // このノードを、まず報告。
            delegate_Foreach1(tesumi1, node1.Value, node1, ref toBreak1);
            if (toBreak1)
            {
                //この全幅探索を終わらせる指示が出ていた場合
                toFinish_ZenpukuTansaku = true;
                goto gt_EndMetdhod;
            }

            // 次のノード
            node1.Foreach_NextNodes((string key2, Node<T1, T2> node2, ref bool toBreak2) =>
            {
                bool toFinish_ZenpukuTansaku2 = false;
                this.Recursive_Node_NextNode(tesumi1 + 1, node2, delegate_Foreach1, ref toFinish_ZenpukuTansaku2);
                if (toFinish_ZenpukuTansaku2)//この全幅探索を終わらせる指示が出ていた場合
                {
                    toBreak2 = true;
                    goto gt_EndBlock;
                }

            gt_EndBlock:
                ;
            });

        gt_EndMetdhod:
            ;
        }









        #region クリアー系

        /// <summary>
        /// ************************************************************************************************************************
        /// 棋譜を空っぽにします。
        /// ************************************************************************************************************************
        /// 
        /// ルートは残します。
        /// 
        /// </summary>
        public void Clear()
        {
            // ルートまで遡ります。
            while (this.CurNode.PreviousNode!=null)
            {
                this.CurNode = this.CurNode.PreviousNode;
            }

            // ルートの次の手を全クリアーします。
            this.CurNode.Clear_NextNodes();

            //LarabeLogger.GetInstance().WriteLineMemo(LarabeLoggerTag_Impl.LINKED_LIST, "リンクトリストは、クリアーされた");
        }

        #endregion




        #region 手目

        /// <summary>
        /// 何手目か。
        /// 
        /// 新版では、初期局面（ルート）は必ず含まれていることから、オリジン1です。
        /// </summary>
        public int CountTesumi(Node<T1, T2> node)
        {
            // [0]初期局面 は必ず入っているので、ループが１回も回らないということはないはず。
            int countTesumi = -1;

            this.ForeachHonpu(node, (int tesumi2, T2 sky, Node<T1, T2> node6, ref bool toBreak) =>
            {
                countTesumi = tesumi2;
            });

            if (-1 == countTesumi)
            {
                throw new Exception($@"手目を調べるのに失敗しました。
[0]初期局面 は必ず入っているので、ループが１回も回らないということはないはずですが、-1手目になりました。");
            }

            // ログ出すぎ
            //GameViewModel.Kifu.TREE_LOGGER.WriteLineMemo(LarabeLoggerTag.LINKED_LIST, "リンクトリストの高さを調べられた Count=[" + count + "]");
            return countTesumi;
        }

        #endregion



        #region ランダムアクセッサ

        public Node<T1, T2> GetRoot()
        {
            return (Node<T1, T2>)this.NodeAt(0);
        }

        public Node<T1, T2> NodeAt(int tesumi1)
        {
            Node<T1, T2> found6 = null;

            this.ForeachHonpu(this.CurNode, (int tesumi2, T2 sky, Node<T1, T2> node6, ref bool toBreak) =>
            {
                if (tesumi1 == tesumi2) //新Verは 0 にも対応。 tesumi1が 0 のとき、配列Verは 1 スタートなので、スルーされるので、このループに入る前に処理しておきます。
                {
                    found6 = node6;
                    toBreak = true;
                }

            });

            if (null == found6)
            {
                throw new Exception($"[{tesumi1}]の局面ノード6はヌルでした。");
            }

            return found6;
        }

        #endregion

    }


}
