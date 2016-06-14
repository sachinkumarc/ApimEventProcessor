
namespace ApimMessageProcessor.ApplicationInsights
{
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using System;
    using System.Threading.Tasks;

    public class ApplicationInsightsHttpMessageProcessor : IHttpMessageProcessor
    {
        readonly TelemetryClient telemetryClient;

        public ApplicationInsightsHttpMessageProcessor()
        {
            TelemetryConfiguration.Active.InstrumentationKey = "7f8f8506-2e41-4b9b-80c5-aa820c805bb1";
            TelemetryConfiguration.Active.TelemetryInitializers.Add(new ExtendedIDTelemetryInitializer());
            TelemetryConfiguration configuration = new TelemetryConfiguration();

            this.telemetryClient = new TelemetryClient();
        }

        public Task ProcessHttpMessage(HttpMessage message)
        {
            var request = new RequestTelemetry();

            if (message.IsRequest)
            {
                request.Name = "Request";

                foreach (var property in message.HttpRequestMessage.Properties)
                {
                    request.Context.Properties[property.Key] = (string)property.Value;
                }

                request.HttpMethod = message.HttpRequestMessage.Method.ToString();
                request.Url = message.HttpRequestMessage.RequestUri;
            }
            else
            {
                request.Name = "Response";

                request.ResponseCode = message.HttpResponseMessage.StatusCode.ToString();                                
            }

            request.Id = message.MessageId.ToString();
            request.Timestamp = DateTime.UtcNow;
            
            this.telemetryClient.TrackRequest(request);

            return Task.FromResult<object>(null);
        }
    }
}
