using SAP.BOL.LogicClasses;

namespace SAP.BOL.Abstract
{
    public interface IProgramManager
    {
        string TempPath { get; }
        string CompiledFile { get; }
        double ExecutedTime { get; }
        string Program { get; set; }
        CompilerType Language { get; set; }
        string ErrorInfo { get; }
        string OutputData { get; }
        string InputData { get; set; }
        bool HasError { get; }
        double MaxTime { get; set; }
        double MemoryUsed { get; }
        InputDataType InputDataType { get; set; }

        void CompileAndExecute();

        void Compile();

        void Execute();

        void Dispose();
    }
}