using SAP.BOL.Abstract;
using System;

namespace SAP.BOL.LogicClasses
{
    /// <summary>
    /// Klasa kompilująca / uruchamiająca programy uczestników turnieju
    /// </summary>
    public class ProgramManager : IProgramManager, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public enum CompilerType
    {
        C, Cpp, Java, Pascal
    }

    public enum InputDataType
    {
        Arguments, Stream, None
    }
}