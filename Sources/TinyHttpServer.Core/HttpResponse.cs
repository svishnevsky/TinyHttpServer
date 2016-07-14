namespace TinyHttpServer.Core
{
    public class HttpResponse
    {
        public string Protocol { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public HttpHeaders Headers { get; set; }

        public byte[] Body { get; set; }

        public HttpResponse()
        {
            this.Headers = new HttpHeaders();
        }
    }
}
