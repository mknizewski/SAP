using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP.BOL.LogicClasses.Exceptions;
using System.Timers;
using System.Diagnostics;

namespace SAP.BOL.LogicClasses
{
    /// <summary>
    /// Klasa kompilująca programy uczestników turnieju
    /// </summary>
    public class Compiler : IDisposable
    {
        private string tempPath;
        private string compiledFile;
        private double executedTime;
        private string program;
        private CompilerType? language;
        private string errorInfo;
        private string outputData;
        private string inputData;
        private bool hasErrors;
        private double maxTime;
        private Task executeTask;
        private Timer timer;

        public string TempPath { get { return tempPath; } }
        public string CompiledFile { get { return compiledFile; } }
        public double ExecutedTime { get { return executedTime; } }
        public string Program { get { return program; } set { program = value; } }
        public CompilerType Language { get { return language.Value; } set { language = value; } }
        public string ErrorInfo { get { return errorInfo; } }
        public string OutputData { get { return outputData; } }
        public string InputData { get { return inputData; } set { inputData = value; } }
        public bool HasError { get { return hasErrors; } }
        public double MaxTime { get { return maxTime; } set { maxTime = value; } }

        public Compiler()
        {
            string tempDirectory = Path.GetRandomFileName();
            tempPath = Path.Combine(Path.GetTempPath(), tempDirectory);
            Directory.CreateDirectory(tempPath);
        }

        public void CompileAndExecute()
        {
            
        }

        public void Compile()
        {
            //sprawdzanie poprawności danych
            if (program == String.Empty)
                throw new ProgramNotFoundException("Program nie został załadowany do pola Program");
            else if (!language.HasValue)
                throw new LanguageNotFoundException("Język nie został załadowany do pola Language");

            //TODO: Sprawdzenie programu pod kątem niedozwolonych technik programistycznych

            //działanie
            timer = new Timer();
            timer.Interval = MaxTime;
            timer.Elapsed += StopTask;

            Process compile = new Process();
            ProcessStartInfo compileInfo = new ProcessStartInfo();

            compileInfo.UseShellExecute = false;
            compileInfo.CreateNoWindow = true;
            compileInfo.RedirectStandardInput = true;
            compileInfo.RedirectStandardOutput = true;
            compileInfo.RedirectStandardError = true;
            
            switch (language)
            {
                case CompilerType.C:
                    compileInfo.FileName = CompilerInfo.CPath;
                    compileInfo.Arguments = "-o ";
                    break;
                case CompilerType.Cpp:
                    compileInfo.FileName = CompilerInfo.CppPath;
                    break;
                case CompilerType.Java:
                    compileInfo.FileName = CompilerInfo.JavaPath;
                    break;
                case CompilerType.Pascal:
                    compileInfo.FileName = CompilerInfo.PascalPath;
                    break;
            }
        }

        public void Execute()
        {
            //sprawdzanie poprawności
            if (compiledFile == String.Empty)
                throw new ProgramNotCompiledException("Program nie został skompilowany");


        }

        private void StopTask(object state, ElapsedEventArgs e)
        {
            executeTask.Dispose();
            timer.Stop();
            timer.Dispose();
        }

        public void Dispose()
        {
            Directory.Delete(tempPath, true);
        }
    }

    /// <summary>
    /// Klasa przechowująca metadane kompilatorów
    /// </summary>
    public static class CompilerInfo
    {
        public static string CPath { get; set; }
        public static string CppPath { get; set; }
        public static string JavaPath { get; set; }
        public static string PascalPath { get; set; }


    }

    public enum CompilerType
    {
        C, Cpp, Java, Pascal
    }
}
