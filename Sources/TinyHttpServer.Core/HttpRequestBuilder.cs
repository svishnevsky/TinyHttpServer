namespace TinyHttpServer.Core
{
    using System;

    public class HttpRequestBuilder
    {
        private HttpRequest request;

        public HttpRequestBuilder()
        {
            this.request = new HttpRequest();
        }

        public HttpRequestBuilder Protocol(string protocol)
        {
            request.Protocol = protocol;

            return this;
        }

        public HttpRequestBuilder Method(string method)
        {
            request.Method = method;

            return this;
        }

        public HttpRequestBuilder Uri(Uri uri)
        {
            request.Uri = uri;

            return this;
        }

        public HttpRequestBuilder Header(string name, string value)
        {
            if (this.request.Headers.ContainsKey(name))
            {
                this.request.Headers[name] = value;
            }
            else
            {
                this.request.Headers.Add(name, value);
            }

            return this;
        }


        public HttpRequestBuilder Body(byte[] body)
        {
            request.Body = body;

            return this;
        }

        public HttpRequest Build()
        {
            var buildedRequest = this.request;
            this.request = new HttpRequest();

            return buildedRequest;
        }

    }
}
