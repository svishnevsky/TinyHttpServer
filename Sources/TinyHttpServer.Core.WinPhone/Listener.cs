using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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

            var requestData = new List<byte>();

            using (var input = args.Socket.InputStream)
            {
                var data = new byte[BufferSize];
                var buffer = data.AsBuffer();
                var dataRead = BufferSize;
                while (dataRead == BufferSize)
                {
                    input.ReadAsync(buffer, BufferSize, InputStreamOptions.Partial).GetResults();
                    requestData.AddRange(data);
                    dataRead = buffer.Length;
                }
            }

            var result = this.OnRequestReceived(requestData.ToArray()).Result;

            using (var output = args.Socket.OutputStream)
            {
                using (var resp = output.AsStreamForWrite())
                {
                    resp.WriteAsync(result, 0, result.Length);
                    resp.Flush();
                }
            }
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
    }
}
