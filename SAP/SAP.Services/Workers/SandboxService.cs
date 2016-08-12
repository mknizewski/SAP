using SAP.Services.Dictionaries;
using SAP.Services.Infrastructure;
using SAP.Services.Resources;
using System;
using System.IO;
using System.Net;
using System.Resources;
using System.Runtime.Serialization.Json;

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
        private object[] _jsonParameters;
        private const int _jsonLength = 6;

        public string ApiFullPath
        {
            get
            {
                return _apiFullPath;
            }
        }

        public object[] JsonParameters
        {
            get
            {
                return _jsonParameters;
            }
            set
            {
                var val = value as object[];

                if (val != null && val.Length == _jsonLength)
                    _jsonParameters = val;
                else
                {
                    string fieldName = _jsonParameters.GetType().Name;
                    string exMessage = _resourceManager.GetString(SandboxDictionary.IncorrectJson);
                    throw new ArgumentException(exMessage, fieldName);
                }
            }
        }

        private SandboxService(RequestType reqType)
        {
            this._resourceManager = new ResourceManager(typeof(SandboxServiceResource));
            this._requestType = reqType;
            this._apiPathPattern = ServiceDictionary.SandboxApiUrl;
        }

        public static SandboxService Create(RequestType reqType)
        {
            var sandboxService = new SandboxService(reqType);

            return sandboxService;
        }

        public string MakeRequest()
        {
            _typeOfRequest = _requestType == RequestType.Info ?
                () =>
                {
                    _apiFullPath = string.Format(_apiPathPattern, _resourceManager.GetString(SandboxDictionary.SandboxApiInfo));
                    return DoGet();
                } :
                             _requestType == RequestType.Token ?
                () =>
                {
                    _apiFullPath = string.Format(_apiPathPattern, _resourceManager.GetString(SandboxDictionary.SandboxApiToken));
                    return DoGet();
                } :
                             _requestType == RequestType.Api ?
                () =>
                {
                    if (_jsonParameters == null)
                    {
                        string fieldName = _jsonParameters.GetType().Name;
                        string exMessage = _resourceManager.GetString(SandboxDictionary.IncorrectJson);

                        throw new ArgumentException(exMessage, fieldName);
                    }

                    _apiFullPath = string.Format(_apiPathPattern, _resourceManager.GetString(SandboxDictionary.SandboxApi));
                    return DoPost();
                } :
                (TypeOfRequest)(() => 
                {
                    return string.Empty;
                });

            return _typeOfRequest();
        }

        private string DoGet()
        {
            WebRequest webRequest = WebRequest.Create(_apiFullPath);
            webRequest.Method = SandboxDictionary.Get;

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

        private string DoPost()
        {
            var webRequest = WebRequest.Create(_apiFullPath);
            webRequest.ContentType = SandboxDictionary.JsonRequestMimeType;
            webRequest.Method = SandboxDictionary.Post;

            var jsonSerializer = new DataContractJsonSerializer(typeof(object[]));
            var memStream = new MemoryStream();

            jsonSerializer.WriteObject(memStream, _jsonParameters);
            memStream.Position = (long)decimal.Zero;

            string jsonToServer = string.Empty;

            using (var streamReader = new StreamReader(memStream))
            {
                jsonToServer = streamReader.ReadToEnd();
                streamReader.Close();
            }

            webRequest.ContentLength = jsonToServer.Length;

            using (var streamWriter = new StreamWriter(webRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonToServer);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var webResponse = webRequest.GetResponse();
            string responseFromServer = string.Empty;

            using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
            {
                responseFromServer = streamReader.ReadToEnd();
                streamReader.Close();
            }

            memStream.Close();
            return responseFromServer;
        }
    }

    public enum RequestType
    {
        Info, Token, Api
    }
}

