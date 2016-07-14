using System;

namespace TinyHttpServer.Core
{
    public class RelativePath
    {
        public string Path { get; private set; }

        public RelativePath(string relativePath)
        {
            if (!relativePath.StartsWith("/"))
            {
                throw new FormatException("Path should start with \"/\".");
            }

            this.Path = relativePath;
        }
    }
}
