using System;
using System.Windows.Forms;

#if USING_LUA
using NLua;
#endif

namespace LuaTest
{
    public partial class UcMain1 : UserControl
    {
        public UcMain1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
#if USING_LUA
            using (Lua lua = new Lua())
            {
                // 初期化
                lua.LoadCLRPackage();


                // 関数の登録
                //
                // Lua「writeLine("あー☆")」
                // ↓
                // C#「C onsole.WriteLine("あー☆")」
                lua.RegisterFunction("writeLine", typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));

                // Luaファイル読込み
                lua.DoFile("./test.lua");
                lua.DoFile("./test2.lua");
                lua.DoFile("./main.lua");

                // 実行
                lua.GetFunction("main").Call();
            }
#endif
        }

        private void button2_Click(object sender, EventArgs e)
        {
#if USING_LUA
            using (Lua lua = new Lua())
            {
                // 初期化
                lua.LoadCLRPackage();

                //
                // 関数の登録
                //

                // Lua「writeLine("あー☆")」
                // ↓
                // C#「C onsole.WriteLine("あー☆")」
                lua.RegisterFunction("writeLine", typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));

                // Lua「random(0,100)」
                // ↓
                // C#「Util_Lua_KifuWarabe.Random(0,100)」
                lua.RegisterFunction("random", typeof(Uc_Main1).GetMethod("Random", new Type[] { typeof(double), typeof(double) }));

                // Luaファイル読込み
                lua.DoFile("./random.lua");

                // 実行
                lua.GetFunction("main").Call();

                var scoreX = lua["score"];
                double score = (double)scoreX;

                MessageBox.Show("scoreXの型=[" + scoreX.GetType().Name + "]　score=[" + score + "]");

                //lua.Close(); // アプリが終了してしまう？

            }
#endif
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="begin">この値を含む始端値。int型にキャストして使われます。</param>
        /// <param name="end">この値を含む終端値。int型にキャストして使われます。</param>
        /// <returns>int型にキャストして使われます。</returns>
        public static double Random(double begin, double end)
        {
            return new Random(0).Next((int)begin, (int)end);
        }

    }
}
