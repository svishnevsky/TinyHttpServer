namespace TinyHttpServer.Core
{
    public class HttpRequest
    {
        public string Protocol { get; set; }

        public string Method { get; set; }

        public byte[] Body { get; set; }

        public RelativePath RelativePath { get; set; }

        public HttpHeaders Headers { get; set; }

        public HttpRequest()
        {
            this.Headers = new HttpHeaders();
        }
    }
}
