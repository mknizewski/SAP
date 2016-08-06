using SAP.Services.Resources;
using System;
using System.IO;
using System.Net;
using System.Resources;

namespace SAP.Workers
{
    public delegate string TypeOfRequest();

    /// <summary>
    /// Serwis Sandboxowy - komunikacja z API
    /// </summary>
    public class SandboxService
    {
        private string _apiFullPath;
        private string _apiPathPattern;
        private RequestType _requestType;
        private ResourceManager _resourceManager;
        private TypeOfRequest _typeOfRequest;

        public string ApiFullPath
        {
            get
            {
                return _apiFullPath;
            }
        }

        private SandboxService(RequestType reqType)
        {
            this._resourceManager = new ResourceManager(typeof(SandboxServiceResource));
            this._requestType = reqType;
            this._apiPathPattern = _resourceManager.GetString("SandboxApiPattern");
        }

        public static SandboxService Create(RequestType reqType)
        {
            var sandboxWorker = new SandboxService(reqType);

            return sandboxWorker;
        }

        public string MakeRequest()
        {
            _typeOfRequest = _requestType == RequestType.Info ? (TypeOfRequest)GetInfo :
                             _requestType == RequestType.Api ? (TypeOfRequest)GetToken :
                             _requestType == RequestType.Token ? (TypeOfRequest)GetExecute :
                             (TypeOfRequest)(() => { return string.Empty; });

            return _typeOfRequest();
        }

        private string GetInfo()
        {
            _apiFullPath = string.Format(_apiPathPattern, _resourceManager.GetString("SandboxApiInfo"));

            WebRequest webRequest = WebRequest.Create(_apiFullPath);
            webRequest.Method = "GET";

            WebResponse webRespone = webRequest.GetResponse();
            var dataStream = webRespone.GetResponseStream();
            string responseFromServer = string.Empty;

            using (var reader = new StreamReader(dataStream))
            {
                responseFromServer = reader.ReadToEnd();
                reader.Close();
            }

            dataStream.Close();

            return responseFromServer;
        }

        private string GetToken()
        {
            return string.Empty;
        }

        private string GetExecute()
        {
            return string.Empty;
        }
    }

    public enum RequestType
    {
        Info, Token, Api
    }
}

