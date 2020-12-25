using System.Runtime.CompilerServices;
using Finger = ProjectDark.NamedInt.StrictNamedInt0; //フィンガー番号

namespace Grayscale.Kifuwarazusa.Entities.Features
{
    public interface Sky
    {
        /// <summary>
        /// TODO:
        /// </summary>
        bool PsideIsBlack { get; set; }

        Starlight StarlightIndexOf(
            Finger finger,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0
        );

        void Foreach_Starlights(SkyBuffer.DELEGATE_Sky_Foreach delegate_Sky_Foreach);

        int Count
        {
            get;
        }

        Sky Clone();

        Fingers Fingers_All();
    }
}
