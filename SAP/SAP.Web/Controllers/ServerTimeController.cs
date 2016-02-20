using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace SAP.Web.Controllers
{
    public class ServerTimeController : ApiController
    {
        private static readonly Lazy<Timer> _timer = new Lazy<Timer>(() => new Timer(TimerCallback, null, 0, 1000));
        private static readonly ConcurrentQueue<StreamWriter> _streammessage = new ConcurrentQueue<StreamWriter>();

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
                    data.WriteLine("data: Czas systemowy " + DateTime.Now.ToString("H:mm:ss dd/MM/yyyy") + "\n");
                    data.Flush();
                }
                catch
                {
                    StreamWriter streamWriter;
                    _streammessage.TryDequeue(out streamWriter);
                    streamWriter.Dispose();
                }
            }
        }

        public void OnStreamAvailable(Stream stream, HttpContent headers, TransportContext context)
        {
            StreamWriter streamwriter = new StreamWriter(stream);
            _streammessage.Enqueue(streamwriter);
        }
    }
}