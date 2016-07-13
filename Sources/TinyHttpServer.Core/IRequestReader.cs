using System.Threading.Tasks;

namespace TinyHttpServer.Core
{
    public interface IRequestReader
    {
        Task<HttpRequest> Read(byte[] requestData);
    }
}
