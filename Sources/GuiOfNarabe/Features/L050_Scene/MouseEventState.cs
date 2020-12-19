
using Grayscale.P025_KifuLarabe.L00025_Struct;
//スプライト番号

using System.Windows.Forms;
using System.Drawing;
using Grayscale.P200_KifuNarabe.L00047_Scene;
using Grayscale.Kifuwarazusa.Entities.Logging;

namespace Grayscale.P200_KifuNarabe.L050_Scene
{
    /// <summary>
    /// マウス操作の状態です。
    /// </summary>
    public class MouseEventState
    {

        public SceneName Name1 { get { return this.name1; } }
        private SceneName name1;


        public MouseEventStateName Name2 { get { return this.name2; } }
        private MouseEventStateName name2;

        public Point MouseLocation { get { return this.mouseLocation; } }
        private Point mouseLocation;

        public MouseEventState()
        {
            this.name1 = SceneName.Ignore;
            this.name2 = MouseEventStateName.Ignore;
            this.mouseLocation = Point.Empty;
        }

        public MouseEventState(SceneName name1, MouseEventStateName name2, Point mouseLocation)
        {
            this.name1 = name1;
            this.name2 = name2;
            this.mouseLocation = mouseLocation;
        }

    }
}
