using System;
using System.IO;
using System.Threading.Tasks;
using SAP.BOL.LogicClasses.Exceptions;
using System.Timers;
using System.Diagnostics;

namespace SAP.BOL.LogicClasses
{
    /// <summary>
    /// Klasa kompilująca / uruchamiająca programy uczestników turnieju
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
        private string sourceFile;
        private double memoryUsed;
        private InputDataType inputDataType;

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
        public double MemoryUsed { get { return memoryUsed; } }
        public InputDataType InputDataType { get { return inputDataType; } set { inputDataType = value; } }

        public Compiler()
        {
            string tempDirectory = Path.GetRandomFileName();
            tempPath = Path.Combine(Path.GetTempPath(), tempDirectory);
            Directory.CreateDirectory(tempPath);
        }

        public void CompileAndExecute()
        {
            Compile();
            Execute();
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
                        compileInfo.Arguments = sourceFile + ".pp";
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

            Process exec = new Process();
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
                executeTask = Task.Run(() =>
                {
                    switch (inputDataType)
                    {
                        case InputDataType.Arguments:
                            exec.StartInfo.Arguments = inputData;
                            exec.Start();
                            break;
                        case InputDataType.Stream:
                            exec.Start();
                            if (inputData != String.Empty)
                            {
                                StreamWriter input = exec.StandardInput;
                                input.WriteLine(inputData);
                                input.Close();
                            }
                            break;
                        case InputDataType.None:
                            exec.Start();
                            break;
                    }

                    outputData = exec.StandardOutput.ReadToEnd();
                    errorInfo = exec.StandardError.ReadToEnd();
                    exec.WaitForExit();
                    executedTime = exec.TotalProcessorTime.Seconds;
                    memoryUsed = exec.NonpagedSystemMemorySize64;

                    if (errorInfo != String.Empty)
                        hasErrors = true;
                });
            }
            else
            {
                execInfo.EnvironmentVariables.Add("CLASSPATH", tempPath);
                //TODO: Algorytm wykonania programu javy
            }

            timer.Stop();
            timer.Dispose();
            executeTask.Dispose();
        }

        private void StopTask(object state, ElapsedEventArgs e)
        {
            executeTask.Dispose();
            timer.Stop();
            timer.Dispose();

            hasErrors = true;
            errorInfo = "Program przekroczył wyznaczony czas!";
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

    public enum InputDataType
    {
        Arguments, Stream, None
    }
}
