using SAP.BOL.Abstract;
using SAP.BOL.LogicClasses.Exceptions;
using System;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace SAP.BOL.LogicClasses
{
    /// <summary>
    /// Klasa kompilująca / uruchamiająca programy uczestników turnieju
    /// </summary>
    public class ProgramManager : IProgramManager, IDisposable
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
        private double maxMemory;
        private Timer timer;
        private string sourceFile;
        private double memoryUsed;
        private InputDataType inputDataType;
        private Process exec;

        public string TempPath { get { return tempPath; } }
        public string CompiledFile { get { return compiledFile; } }
        public double ExecutedTime { get { return executedTime; } }
        public string Program { get { return program; } set { program = value; } }
        public CompilerType Language { get { return language.Value; } set { language = value; } }
        public string ErrorInfo { get { return errorInfo; } }
        public string OutputData { get { return outputData; } }
        public string InputData { get { return inputData; } set { inputData = value; } }
        public bool HasError { get { return hasErrors; } }
        public double MaxTime { get { return maxTime; } set { maxTime = value / 0.001; } }
        public double MaxMemory { get { return maxMemory; } set { maxMemory = value; } }
        public double MemoryUsed { get { return memoryUsed; } }
        public InputDataType InputDataType { get { return inputDataType; } set { inputDataType = value; } }

        public ProgramManager()
        {
            string tempDirectory = Path.GetRandomFileName();
            tempPath = Path.Combine(Path.GetTempPath(), tempDirectory);
            Directory.CreateDirectory(tempPath);
        }

        public void CompileAndExecute()
        {
            Compile();

            if (!hasErrors)
                Execute();
        }

        public void Compile()
        {
            //sprawdzanie poprawności danych
            if (program == String.Empty)
                throw new ProgramNotFoundException("Program nie został załadowany do pola Program");
            else if (!language.HasValue)
                throw new LanguageNotFoundException("Język nie został załadowany do pola Language");

            //działanie
            Process compile = new Process();
            ProcessStartInfo compileInfo = new ProcessStartInfo();

            compileInfo.UseShellExecute = false;
            compileInfo.CreateNoWindow = true;
            compileInfo.RedirectStandardOutput = true;
            compileInfo.RedirectStandardError = true;

            if (language.Value != CompilerType.Java)
            {
                sourceFile = Path.Combine(tempPath, Path.GetRandomFileName());

                switch (language.Value)
                {
                    case CompilerType.C:
                        compileInfo.FileName = CompilerInfo.CPath;
                        File.AppendAllText(sourceFile + ".c", program);
                        compileInfo.Arguments = sourceFile + ".c -o " + sourceFile + ".exe";
                        break;

                    case CompilerType.Cpp:
                        compileInfo.FileName = CompilerInfo.CppPath;
                        File.AppendAllText(sourceFile + ".cpp", program);
                        compileInfo.Arguments = sourceFile + ".cpp -o " + sourceFile + ".exe";
                        break;

                    case CompilerType.Pascal:
                        compileInfo.FileName = CompilerInfo.PascalPath;
                        File.AppendAllText(sourceFile + ".pas", program);
                        compileInfo.Arguments = sourceFile + ".pas";
                        break;
                }

                compile.StartInfo = compileInfo;
                compile.Start();
                errorInfo = compile.StandardError.ReadToEnd();
                compile.WaitForExit();

                if (errorInfo != String.Empty)
                    hasErrors = true;
                else
                    compiledFile = sourceFile + ".exe";
            }
            else //w przypadku jezyka JAVA wystepuje inne rozwiązanie kompilacji niż w przypadku pozostałych języków
            {
                compileInfo.EnvironmentVariables.Add("CLASSPATH", tempPath);
                //TODO: Opisać przypadek javy
            }
        }

        public void Execute()
        {
            //sprawdzanie poprawności
            if (compiledFile == String.Empty)
                throw new ProgramNotCompiledException("Program nie został skompilowany");

            exec = new Process();
            ProcessStartInfo execInfo = new ProcessStartInfo();

            execInfo.UseShellExecute = false;
            execInfo.CreateNoWindow = true;
            execInfo.RedirectStandardError = true;
            execInfo.RedirectStandardInput = true;
            execInfo.RedirectStandardOutput = true;

            execInfo.FileName = compiledFile;
            exec.StartInfo = execInfo;

            if (language.Value != CompilerType.Java)
            {
                timer = new Timer();
                timer.Interval = maxTime;
                timer.Elapsed += StopTask;

                timer.Start();
                switch (inputDataType)
                {
                    case InputDataType.Arguments:
                        exec.StartInfo.Arguments = inputData;
                        exec.Start();
                        memoryUsed = 0; // (exec.PrivateMemorySize64 / 1024f) / 1024f;
                        outputData = exec.StandardOutput.ReadToEnd();
                        break;

                    case InputDataType.Stream:
                        string[] arguments = inputData.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); // rozdzielanie spacją
                        exec.Start();
                        memoryUsed = 0; // (exec.PrivateMemorySize64 / 1024f) / 1024f;

                        StreamWriter writer = exec.StandardInput;
                        foreach (string arg in arguments)
                            writer.WriteLine(arg);

                        outputData = exec.StandardOutput.ReadToEnd();
                        break;

                    case InputDataType.None:
                        exec.Start();
                        memoryUsed = 0; //(exec.PrivateMemorySize64 / 1024f) / 1024f;
                        outputData = exec.StandardOutput.ReadToEnd();
                        break;
                }

                exec.WaitForExit();
                executedTime = exec.TotalProcessorTime.Milliseconds;

                if (errorInfo != String.Empty)
                    hasErrors = true;
            }
            else
            {
                execInfo.EnvironmentVariables.Add("CLASSPATH", tempPath);
                //TODO: Algorytm wykonania programu javy
            }

            timer.Stop();
            timer.Dispose();
        }

        private void StopTask(object state, ElapsedEventArgs e)
        {
            exec.Kill();

            hasErrors = true;
            errorInfo = "Program przekroczył zadany czas";
        }

        public void Dispose()
        {
            Directory.Delete(tempPath, true);

            exec.Dispose();
            timer.Dispose();
        }
    }

    /// <summary>
    /// Klasa przechowująca sciezki kompilatorów
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

    public enum InputDataType
    {
        Arguments, Stream, None
    }
}