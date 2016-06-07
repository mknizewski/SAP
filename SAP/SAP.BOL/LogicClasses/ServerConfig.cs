using SAP.DAL.Abstract;
using SAP.DAL.Repositories;
using System;
using System.Collections;
using System.Reflection;
using System.Resources;

namespace SAP.BOL.LogicClasses
{
    public static class ServerConfig
    {
        public static bool SynchronizeData;
        public static bool OnlyLocalConnection;

        static ServerConfig()
        {
            SynchronizeData = false;
            OnlyLocalConnection = false;
        }

        public static void Inicialize()
        {
            CompilersSetup();
        }

        public static void CompilersSetup()
        {
            ICompilerRespository _compilerRepo = new CompilerRepository();

            //Szukanie scieżek w bazie danych
            var compilers = _compilerRepo.Compilers;

            foreach (var item in compilers)
            {
                CompilerType type = (CompilerType)item.SystemId;

                switch (type)
                {
                    case CompilerType.C:
                        CompilerInfo.CPath = item.FullPath;
                        CompilerInfo.CArguments = item.Arguments;
                        break;

                    case CompilerType.Cpp:
                        CompilerInfo.CppPath = item.FullPath;
                        CompilerInfo.CppArguments = item.Arguments;
                        break;

                    case CompilerType.Java:
                        CompilerInfo.JavaPath = item.FullPath;
                        CompilerInfo.JavaArguments = item.Arguments;
                        break;

                    case CompilerType.Pascal:
                        CompilerInfo.PascalPath = item.FullPath;
                        CompilerInfo.PascalArguments = item.Arguments;
                        break;
                }
            }
            //Disposing
            _compilerRepo.Dispose();
            _compilerRepo = null;
        }
    }
}