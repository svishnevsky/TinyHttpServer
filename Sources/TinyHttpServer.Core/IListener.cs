namespace TinyHttpServer.Core
{
    using System.Threading.Tasks;
    
    public delegate Task<byte[]> RequestHandler(byte[] requestData);

    public interface IListener
    {
        event RequestHandler OnRequestReceived;

        void Start();

        void Stop();
    }
}
