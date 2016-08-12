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
        }
    }
}