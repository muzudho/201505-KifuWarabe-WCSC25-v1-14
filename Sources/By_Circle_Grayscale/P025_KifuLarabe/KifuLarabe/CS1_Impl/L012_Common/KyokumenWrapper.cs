using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Grayscale.P025_KifuLarabe.L00025_Struct;
using Grayscale.P025_KifuLarabe.L006_SfenEx;

namespace Grayscale.P025_KifuLarabe.L012_Common
{
    public class KyokumenWrapper
    {

        //*
        #region sky型
        public SkyConst ToKyokumenConst { get { return this.kyokumen; } }
        public void SetKyokumen(SkyConst sky) { this.kyokumen = sky; }
        private SkyConst kyokumen;
        #endregion
        // */

        /*
        #region startposString型
        public SkyConst ToKyokumenConst {
            get {
                SkyConst result = Util_Sky.ImportSfen(this.startposString);

                StartposImporter.Assert(new SkyBuffer(result), "this.startposString=["+this.startposString+"]");

                return result;
            }
        }
        public void SetKyokumen(SkyConst sky) { this.startposString = Util_Sky.ExportSfen(sky); }
        private StartposString startposString;
        #endregion
        // */

        public KyokumenWrapper(SkyConst sky)
        {
            Debug.Assert(sky.Count == 40, "sky.Starlights.Count=[" + sky.Count + "]");//将棋の駒の数

            //*
            #region sky型
            this.kyokumen = sky;
            #endregion
            // */

            /*
            #region startposString型
            this.startposString = Util_Sky.ExportSfen(sky);
            #endregion
            // */
        }
    }
}
