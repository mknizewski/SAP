using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SAP.Web.Infrastructrue.Server
{
    /// <summary>
    /// Słownik trzymający klucze w Web.config
    /// </summary>
    public static class ServerDictionary
    {
        public static string CaptchaPrivateKey
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("reCaptchaPrivateKey");
            }
        }
    }
}