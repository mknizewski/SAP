using SAP.DAL.Abstract;
using SAP.DAL.Repositories;

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
                        break;

                    case CompilerType.Cpp:
                        CompilerInfo.CppPath = item.FullPath;
                        break;

                    case CompilerType.Java:
                        CompilerInfo.JavaPath = item.FullPath;
                        break;

                    case CompilerType.Pascal:
                        CompilerInfo.PascalPath = item.FullPath;
                        break;
                }
            }
        }
    }
}