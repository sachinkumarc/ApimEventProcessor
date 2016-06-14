
namespace ApimMessageProcessor.RunScopeProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Runscope.Messages;
    using Runscope.Links;

    public class RunscopeHttpMessageProcessor : IHttpMessageProcessor
    {
        private HttpClient _HttpClient;
        private ILogger _Logger;
        private string _BucketKey;
        public RunscopeHttpMessageProcessor(HttpClient httpClient, ILogger logger)
        {
            _HttpClient = httpClient;
            //var key = Environment.GetEnvironmentVariable("APIMEVENTS-RUNSCOPE-KEY", EnvironmentVariableTarget.Process);
            var key = "346baac8-88bb-4e06-b286-08530fbcfffe";
            _HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", key);
            _HttpClient.BaseAddress = new Uri("https://api.runscope.com");
            //_BucketKey = Environment.GetEnvironmentVariable("APIMEVENTS-RUNSCOPE-BUCKET", EnvironmentVariableTarget.Process);
            _BucketKey = "cl03od8k1tl1";
            _Logger = logger;   
        }

        public async Task ProcessHttpMessage(HttpMessage message)
        {
            var runscopeMessage = new RunscopeMessage()
            {
                UniqueIdentifier = message.MessageId
            };

            if (message.IsRequest)
            {
                _Logger.LogInfo("Processing HTTP request " + message.MessageId.ToString());
                runscopeMessage.Request = await RunscopeRequest.CreateFromAsync(message.HttpRequestMessage);
            }
            else
            {
                _Logger.LogInfo("Processing HTTP response " + message.MessageId.ToString());
                runscopeMessage.Response = await RunscopeResponse.CreateFromAsync(message.HttpResponseMessage);
            }

            var messagesLink = new MessagesLink() { Method = HttpMethod.Post };
            messagesLink.BucketKey = _BucketKey;
            messagesLink.RunscopeMessage = runscopeMessage;
            var runscopeResponse = await _HttpClient.SendAsync(messagesLink.CreateRequest());
            if (runscopeResponse.IsSuccessStatusCode)
            {
                _Logger.LogDebug("Message forwarded to Runscope");
            }
            else
            {
                _Logger.LogDebug("Failed to send request");
            }
        }
    }
}
