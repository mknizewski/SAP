namespace SAP.BOL.LogicClasses
{
    public static class ServerConfig
    {
        public static volatile bool SynchronizeData;
        public static volatile bool OnlyLocalConnection;

        static ServerConfig()
        {
            SynchronizeData = false;
            OnlyLocalConnection = false;
        }

        public static void Inicialize()
        {
            //TODO: Coś tutaj wypadało by zrobić
        }
    }
}