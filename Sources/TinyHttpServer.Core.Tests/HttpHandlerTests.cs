using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TinyHttpServer.Core.Tests
{
    [TestClass]
    public class HttpHandlerTests
    {
        [TestMethod]
        public void PostTest()
        {

            var request =
                Encoding.UTF8.GetBytes("POST /UpdateCheck.aspx?isBeta=False HTTP/1.1\n\rUser - Agent: Fiddler / 4.6.2.3(.NET 4.0.30319.42000; WinNT 6.1.7601 SP1; ru - RU; 4xAMD64; Auto Update; Full Instance)\n\rPragma: no - cache\n\rHost: www.telerik.com\n\rAccept - Language: ru - RU\n\rReferer: http://fiddler2.com/client/4.6.2.3\n\rAccept - Encoding: gzip, deflate\n\rContent-Length:5\n\rConnection: close\n\r12345");

            var reader = new RequestReader();

            var httpRequest = reader.Read(request).Result;

            var httpHandler = new HttpHandler();

            var result = httpHandler.Handle(httpRequest).Result;

            var byteResponse = new ResponseWriter().Write(result).Result;


            var text = Encoding.UTF8.GetString(byteResponse);
            Assert.IsNotNull(byteResponse);
        }
    }
}
