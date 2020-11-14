using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if USING_LUA
using NLua;
#endif

namespace Grayscale.P045_Atama.L012_Lua
{


    public abstract class Util_Lua_KifuWarabe
    {

#if USING_LUA
        private static Lua lua;
#endif

        public static double Score { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="luaFuncName">実行したいLua関数の名前。</param>
        public static void Perform(string luaFuncName)
        {
            //System.Windows.Forms.MessageBox.Show("[" + luaFuncName + "]呼び出し");

#if USING_LUA
            using (Util_Lua_KifuWarabe.lua = new Lua())
                // 要設定 プラットフォームターゲット x64。32bit/64bit混在できない。
                // KifuNaraveVs, KifuWarabe
            {
                try
                {
                    // 初期化
                    lua.LoadCLRPackage();
                    Util_Lua_KifuWarabe.Score = 0;

                    //
                    // 関数の登録
                    //

                    // Lua「debugOut("あー☆")」
                    // ↓
                    // C#「C onsole.WriteLine("あー☆")」
                    lua.RegisterFunction("debugOut", typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }));

                    // Lua「random(0,100)」
                    // ↓
                    // C#「Util_Lua_KifuWarabe.Random(0,100)」
                    lua.RegisterFunction("random", typeof(Util_Lua_KifuWarabe).GetMethod("Random", new Type[] { typeof(double), typeof(double) }));

                    //----------------------------------------------------------------------------------------------------

                    string file = "../../Data/lua/KifuWarabe/data_score.lua";
                    //System.Windows.Forms.MessageBox.Show("[" + file + "]ファイル読込み");

                    Util_Lua_KifuWarabe.lua.DoFile(file);// KifuNarabeVS の、bin/Release等に入れ忘れていないこと。
                    
                    //System.Windows.Forms.MessageBox.Show("[" + luaFuncName + "]呼び出し");
                    Util_Lua_KifuWarabe.lua.GetFunction(luaFuncName).Call();

                    //System.Windows.Forms.MessageBox.Show("スコアは？");
                    var score2 = lua["score"];
                    //System.Windows.Forms.MessageBox.Show("スコアの型は[" + score2.GetType().Name + "]");
                    
                    Util_Lua_KifuWarabe.Score = (double)score2;

                    //Util_Lua_KifuWarabe.lua.Close(); // アプリが終了してしまう？

                    //----------------------------------------------------------------------------------------------------

                }
                catch(Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show( ex.GetType().Name + "：" + ex.Message);
                }    
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
            return P025_KifuLarabe.L007_Random.LarabeRandom.Random.Next((int)begin, (int)end);
        }

    }


}
