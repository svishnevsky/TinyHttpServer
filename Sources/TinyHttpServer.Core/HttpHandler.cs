namespace TinyHttpServer.Core
{
    using System;
    using System.Threading.Tasks;

    public class HttpHandler : IHttpHandler
    {
        private const string ServerVersion = "TinyHttpServer/0.1";

        public async Task<HttpResponse> Handle(HttpRequest request)
        {
            return await Task.Factory.StartNew(() =>
            {
                var response = new HttpResponse
                {
                    Protocol = request.Protocol,
                    StatusCode = HttpStatusCode.Ok,
                    Body = request.Body
                };

                response.Headers.Add("Content-Length", (response.Body?.Length ?? 0).ToString());
                response.Headers.Add("Date", DateTime.UtcNow.ToString("R"));
                response.Headers.Add("Server", ServerVersion);
                response.Headers.Add("Connection", "close");

                return response;
            });
        }
    }
}
