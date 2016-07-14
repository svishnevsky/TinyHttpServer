using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace TinyHttpServer.Core.WinPhone
{
    public class Listener : IListener, IDisposable
    {
        public event RequestHandler OnRequestReceived;

        private const uint BufferSize = 8192;

        private readonly int port;

        private StreamSocketListener listener;

        public Listener(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            this.listener = new StreamSocketListener();
            this.listener.ConnectionReceived += ListenerOnConnectionReceived;
            this.listener.BindServiceNameAsync(this.port.ToString()).GetResults();
        }

        private void ListenerOnConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            if (this.OnRequestReceived == null)
            {
                return;
            }

            this.ProccessRequest(args.Socket).Wait();
        }

        public void Stop()
        {
            this.listener.ConnectionReceived -= ListenerOnConnectionReceived;
            this.listener.Dispose();
        }

        public void Dispose()
        {
            var streamSocketListener = this.listener;
            streamSocketListener?.Dispose();
        }

        private async Task ProccessRequest(StreamSocket socket)
        {
            var requestData = await this.ReadData(socket);

            var result = await this.OnRequestReceived(requestData);

            using (var output = socket.OutputStream)
            {
                using (var resp = output.AsStreamForWrite())
                {
                    await resp.WriteAsync(result, 0, result.Length);
                    await resp.FlushAsync();
                }
            }
        }

        private async Task<byte[]> ReadData(StreamSocket socket)
        {
            var requestData = new List<byte>();

            using (var input = socket.InputStream)
            {
                var data = new byte[BufferSize];
                var buffer = data.AsBuffer();
                var dataRead = BufferSize;
                while (dataRead == BufferSize)
                {
                    await input.ReadAsync(buffer, BufferSize, InputStreamOptions.Partial);
                    requestData.AddRange(buffer.ToArray());
                    dataRead = buffer.Length;
                }
            }

            return requestData.ToArray();
        }
    }
}
