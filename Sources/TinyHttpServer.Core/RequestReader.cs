using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyHttpServer.Core
{
    public class RequestReader : IRequestReader
    {
        public Task<HttpRequest> Read(byte[] requestData)
        {
            return Task.Factory.StartNew(() =>
            {
                var start = 0;

                var nextIndex = this.NextIndexOf(requestData, 10, start);

                if (nextIndex == -1)
                {
                    throw new InvalidDataException("Http request is not well formed.");
                }

                var str = Encoding.UTF8.GetString(requestData, start, nextIndex - start).Trim();

                start = nextIndex + 1;

                var parts = str.Split(' ');

                if (parts.Length != 3)
                {
                    throw new InvalidDataException("Http request is not well formed.");
                }

                var builder = new HttpRequestBuilder();

                builder.Method(parts[0]);
                builder.RelativePath(new RelativePath(parts[1]));
                builder.Protocol(parts[2]);

                var bodyIndex = requestData.Length - 1;

                while (start < bodyIndex)
                {
                    nextIndex = this.NextIndexOf(requestData, 10, start);
                    if (nextIndex == -1)
                    {
                        nextIndex = requestData.Length;
                    }

                    str = Encoding.UTF8.GetString(requestData, start, nextIndex - start).Trim();
                    start = nextIndex + 1;

                    var header = str.Split(':');

                    if (header.Length != 2)
                    {
                        continue;
                    }

                    builder.Header(header[0].Trim(), header[1].Trim());

                    if (header[0].Trim().ToUpper() != "CONTENT-LENGTH")
                    {
                        continue;
                    }

                    var contentLength = int.Parse(header[1].Trim());
                    bodyIndex = requestData.Length - contentLength;

                    builder.Body(requestData.Skip(bodyIndex).ToArray());
                }

                return builder.Build();
            });
        }

        private int NextIndexOf(IReadOnlyList<byte> data, byte symbol, int start)
        {
            for (var i = start; i < data.Count; i++)
            {
                if (data[i] == symbol)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
