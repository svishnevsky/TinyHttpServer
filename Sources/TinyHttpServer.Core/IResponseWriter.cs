using System.Threading.Tasks;

namespace TinyHttpServer.Core
{
    public interface IResponseWriter
    {
        Task<byte[]> Write(HttpResponse response);
    }
}
