using ApimMessageProcessor;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ApimEventProcessorTests
{
    public class ApplicationInsightMessageProcessorTests
    {
        [Fact]
        public async Task Test()
        {
            var processor = EventProcessorFactory.GetMessageProcessor();            

            var httpRequestMessage = new HttpRequestMessage()
            {
                RequestUri = new Uri("http://example.com/foo")
            };

            var httpMessage = new HttpMessage()
            {
                IsRequest = true,
                HttpRequestMessage = httpRequestMessage
            };

            await processor.ProcessHttpMessage(httpMessage);
        }

    }
}
