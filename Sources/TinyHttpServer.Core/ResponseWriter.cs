using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TinyHttpServer.Core
{
    public class ResponseWriter : IResponseWriter
    {
        public async Task<byte[]> Write(HttpResponse response)
        {
            return await Task.Factory.StartNew(() =>
            {
                var builder = new StringBuilder(
                    $"{response.Protocol} {(int) response.StatusCode} {Regex.Replace(response.StatusCode.ToString(), "[A-Z]", " $0")}\r\n");
                
                builder.Append(string.Join("\r\n",
                    response.Headers.Select(h => $"{h.Key}: {h.Value}")));

                builder.Append("\r\n\r\n");

                var result = new byte[builder.Length + (response.Body?.Length ?? 0)];

                var meta = Encoding.UTF8.GetBytes(builder.ToString());
                meta.CopyTo(result, 0);

                response.Body?.CopyTo(result, builder.Length);

                return result;
            });
        }
    }
}
