using System.Configuration;

namespace SAP.Services.Infrastructure
{
    /// <summary>
    /// Klasa przechowująca klucze w App.config
    /// </summary>
    public static class ServiceDictionary
    {
        public static string SandboxApiUrl
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("SandboxApiUrl");
            }
        }
    }
}
