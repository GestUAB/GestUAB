namespace GestUAB
{
    using System;
    using System.IO;
    using Nancy;

    /// <summary>
    /// Wraps a regular response in a cached response
    /// The cached response invokes the old response and stores it as a string.
    /// Obviously this only works for ASCII text based responses, so don't use this 
    /// in a real application :-)
    /// </summary>
    public class CachedResponse : Response
    {
        private readonly byte[] oldResponseOutput;

        public CachedResponse(Response response)
        {
            this.ContentType = response.ContentType;
            this.Headers = response.Headers;
            this.StatusCode = response.StatusCode;

//            this.Fill(response);

            using (var memoryStream = new MemoryStream())
            {
                response.Contents.Invoke(memoryStream);
                this.oldResponseOutput = memoryStream.GetBuffer();
            }

            this.Contents = GetContents(this.oldResponseOutput);
        }

        protected static Action<Stream> GetContents(byte[] contents)
        {
            return stream =>
            {
                var writer = new  BinaryWriter(stream);
                writer.Write(contents);
            };
        }
    }
}