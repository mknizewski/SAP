using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web.Http;

namespace SAP.Web.Controllers
{
    public class ServerTimeController : ApiController
    {
        private static readonly Lazy<Timer> _timer = new Lazy<Timer>(() => new Timer(TimerCallback, null, 0, 1000));
        private static readonly ConcurrentDictionary<StreamWriter, StreamWriter> _streammessage = new ConcurrentDictionary<StreamWriter, StreamWriter>();

        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            Timer timer = _timer.Value;
            HttpResponseMessage response = request.CreateResponse();
            response.Content = new PushStreamContent((Action<Stream, HttpContent, TransportContext>)OnStreamAvailable, "text/event-stream");

            return response;
        }

        private static void TimerCallback(object state)
        {
            foreach (var data in _streammessage)
            {
                try
                {
                    data.Value.WriteLine("data: Czas systemowy " + DateTime.Now.ToString("H:mm:ss dd/MM/yyyy") + "\n");
                    data.Value.Flush();
                }
                catch
                {
                    StreamWriter streamWriter;
                    _streammessage.TryRemove(data.Value, out streamWriter);
                }
            }
        }

        public void OnStreamAvailable(Stream stream, HttpContent headers, TransportContext context)
        {
            StreamWriter sWriter = new StreamWriter(stream);
            _streammessage.TryAdd(sWriter, sWriter);
        }
    }
}
