
namespace ApimMessageProcessor
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;

    /// <summary>
    /// Parser for format being sent from APIM logtoeventhub policy that contains a complete HTTP request or response message.
    /// </summary>
    /// <remarks>
    ///     Might want to add a version number property to the format before actually letting it out
    ///     in the wild.
    /// </remarks>
    public class HttpMessage
    {
        public Guid MessageId { get; set; }
        public bool IsRequest { get; set; }
        public HttpRequestMessage HttpRequestMessage { get; set; }
        public HttpResponseMessage HttpResponseMessage { get; set; }


        public static HttpMessage Parse(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return Parse(sr.ReadToEnd());
            }
        }

        public static HttpMessage Parse(string data)
        {
            var httpMessage = new HttpMessage();

            HttpContent content;
            using (var sr = new StringReader(data))
            {
                // First line of data is (request|response) followed by a GUID to link request to response
                // Rest of data is in message/http format

                var readLine = sr.ReadLine();

                if (readLine == null)
                {
                    throw new ArgumentException("First line is null ");
                }

                var firstLine = readLine.Split(':');
                if (firstLine.Length < 2)
                {
                    throw new ArgumentException("Invalid formatted event :" + data);
                }

                httpMessage.IsRequest = firstLine[0] == "request";
                httpMessage.MessageId = Guid.Parse(firstLine[1]);

                string rawcontent = sr.ReadToEnd();
                if (rawcontent == null)
                {
                    throw new ArgumentException("Null response message :" + data);
                }

                var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawcontent));
                stream.Position = 0;
                content = new StreamContent(stream);
            }

            var contentType = new MediaTypeHeaderValue("application/http");

            if (httpMessage.IsRequest)
            {
                contentType.Parameters.Add(new NameValueHeaderValue("msgtype", "request"));

                // Using .Result isn't too evil because content is a locally buffered memory stream
                // Although if this were hosted in a System.Web based ASP.NET host it might block
                httpMessage.HttpRequestMessage = content.ReadAsHttpRequestMessageAsync().Result;
            }
            else
            {
                contentType.Parameters.Add(new NameValueHeaderValue("msgtype", "response"));
                httpMessage.HttpResponseMessage = content.ReadAsHttpResponseMessageAsync().Result;
            }

            content.Headers.ContentType = contentType;

            return httpMessage;
        }
    }
}
