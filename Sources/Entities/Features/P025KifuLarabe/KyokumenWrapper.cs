using System.Diagnostics;

namespace Grayscale.Kifuwarazusa.Entities.Features
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
