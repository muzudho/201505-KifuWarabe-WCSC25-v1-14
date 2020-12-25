using System.Collections.Generic;

namespace Grayscale.Kifuwarazusa.Entities.Features
{

    /// <summary>
    /// ツリー構造です。
    /// 
    /// 局面を入れるのに利用します。
    /// 根ノードに平手局面、最初の子ノードに１手目の局面、を入れるような使い方を想定しています。
    /// </summary>
    public interface Node<T1, T2>
    {

        /// <summary>
        /// このノードのキー。インスタンスを作った後では変更できません。
        /// </summary>
        T1 Key { get; }

        /// <summary>
        /// このノードの値。
        /// </summary>
        T2 Value { get; set; }

        /// <summary>
        /// 親ノード。変更可能。
        /// </summary>
        Node<T1, T2> PreviousNode { get; set; }

        int Count_NextNodes { get; }
        void Clear_NextNodes();
        bool ContainsKey_NextNodes(string key);
        void Add_NextNode(string key, Node<T1, T2> newNode);
        void Set_NextNodes(Dictionary<string, Node<T1, T2>> newNextNodes);

        void Foreach_NextNodes(NodeImpl<T1, T2>.DELEGATE_NextNodes d);

        Json_Val ToJsonVal();

    }
}
