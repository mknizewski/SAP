using System.Configuration;
/// <summary>
/// Słownik do serwisu sandboxa
/// </summary>
namespace SAP.Services.Dictionaries
{
    public static class SandboxDictionary
    {
        public static string SandboxApi
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("SandboxApiUrl");
            }
        }

        public static string SandboxApiGetRequest = "SandboxApiGetRequest";

        public static string SandboxApiInfo = "SandboxApiInfo";

        public static string SandboxApiPattern = "SandboxApiPattern";

        public static string SandboxApiToken = "SandboxApiToken";

        public static string Get = "GET";

        public static string Post = "POST";

        public static string JsonRequestMimeType = "application/json";

        public static string IncorrectJson = "IncorrectJson";
    }
}