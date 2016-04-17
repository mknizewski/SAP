using System;

namespace SAP.BOL.LogicClasses.Exceptions
{
    /// <summary>
    /// Klasa wyjątku w przypadku braku zdefiniowania programu w klasie kompilującej
    /// </summary>
    public class ProgramNotFoundException : Exception
    {
        public ProgramNotFoundException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Klasa wyjątku w przypadku braku zdefiniowania języka w klasie kompilującej
    /// </summary>
    public class LanguageNotFoundException : Exception
    {
        public LanguageNotFoundException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Klasa wyjątku w przypadku braku kompilacji
    /// </summary>
    public class ProgramNotCompiledException : Exception
    {
        public ProgramNotCompiledException(string message) : base(message)
        {
        }
    }

    public class InputDataNotFound : Exception
    {
        public InputDataNotFound() : base("Nie znaleziono danych wejściowych")
        {
        }
    }

    public class TimeElasped : Exception
    {
        public TimeElasped() : base("Program przekroczył ograniczony czas")
        {
        }
    }
}