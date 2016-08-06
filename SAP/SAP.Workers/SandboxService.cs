using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAP.Workers
{
    /// <summary>
    /// Serwis Sandboxowy - komunikacja z API
    /// </summary>
    public class SandboxService
    {
        private string _apiFullPath;
        private string _apiPathPattern;
        private SAPRequestType _requestType;

        public string ApiFullPath
        {
            get
            {
                return _apiFullPath;
            }
        }

        private SandboxService(SAPRequestType reqType)
        {
            this._requestType = reqType;
            this._apiPathPattern = Properties
        }

        public static SandboxService Create(SAPRequestType reqType)
        {
            var sandboxWorker = new SandboxService(reqType);

            return sandboxWorker;
        }

        public string MakeRequest()
        {


            return string.Empty;
        }
    }

    public enum SAPRequestType
    {
        Info, Token, Api
    }
}
