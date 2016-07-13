using System.Threading.Tasks;

namespace TinyHttpServer.Core
{
    public interface IHttpHandler
    {
        Task<HttpResponse> Handle(HttpRequest request);
    }
}
