
namespace ApimMessageProcessor.RunScopeProcessor
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Runscope.Links;
    using Runscope.Messages;

    public class RunscopeHttpMessageProcessor : IHttpMessageProcessor
    {
        private readonly HttpClient httpClient;
        private readonly ILogger logger;
        private readonly string bucketKey;

        public RunscopeHttpMessageProcessor(HttpClient httpClient, ILogger logger)
        {
            //this.bucketKey = Environment.GetEnvironmentVariable("APIMEVENTS-RUNSCOPE-BUCKET", EnvironmentVariableTarget.Process);
            //var key = Environment.GetEnvironmentVariable("APIMEVENTS-RUNSCOPE-KEY", EnvironmentVariableTarget.Process);
            this.bucketKey = "cl03od8k1tl1";
            var key = "346baac8-88bb-4e06-b286-08530fbcfffe";

            this.httpClient = httpClient;
            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", key);
            this.httpClient.BaseAddress = new Uri("https://api.runscope.com");

            this.logger = logger;
        }

        public async Task ProcessHttpMessage(HttpMessage message)
        {
            var runscopeMessage = new RunscopeMessage()
            {
                UniqueIdentifier = message.MessageId
            };

            if (message.IsRequest)
            {
                this.logger.LogInfo("Processing HTTP request " + message.MessageId);
                runscopeMessage.Request = await RunscopeRequest.CreateFromAsync(message.HttpRequestMessage);
            }
            else
            {
                this.logger.LogInfo("Processing HTTP response " + message.MessageId);
                runscopeMessage.Response = await RunscopeResponse.CreateFromAsync(message.HttpResponseMessage);
            }

            var messagesLink =  new MessagesLink
                                {
                                    Method = HttpMethod.Post,
                                    BucketKey = this.bucketKey,
                                    RunscopeMessage = runscopeMessage
                                };

            var runscopeResponse = await this.httpClient.SendAsync(messagesLink.CreateRequest());

            this.logger.LogDebug(runscopeResponse.IsSuccessStatusCode ? "Message forwarded to Runscope" : "Failed to send request");
        }
    }
}
