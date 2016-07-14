namespace TinyHttpServer.Core
{
    using System.Threading.Tasks;

    public class HttpServer
    {
        private readonly IRequestReader requestReader;

        private readonly IResponseWriter responseWriter;

        private readonly IHttpHandler httpHandler;

        private readonly IListener listener;

        public HttpServer(IListener listener) : this(new RequestReader(), new HttpHandler(), new ResponseWriter(), listener)
        {
            
        }

        public HttpServer(IRequestReader requestReader, IHttpHandler httpHandler, IResponseWriter responseWriter, IListener listener)
        {
            this.requestReader = requestReader;
            this.httpHandler = httpHandler;
            this.responseWriter = responseWriter;
            this.listener = listener;
        }

        public void Start()
        {
            this.listener.OnRequestReceived += this.OnRequestReceived;
            this.listener.Start();
        }

        public void Stop()
        {
            this.listener.Stop();
            this.listener.OnRequestReceived -= this.OnRequestReceived;
        }

        private async Task<byte[]> OnRequestReceived(byte[] requestData)
        {
            var request = await this.requestReader.Read(requestData);

            var response = await this.httpHandler.Handle(request);

            return await this.responseWriter.Write(response);
        }
    }
}
