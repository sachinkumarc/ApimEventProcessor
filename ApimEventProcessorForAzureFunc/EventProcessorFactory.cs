
namespace ApimEventProcessorForAzureFunc
{
    using System.Net.Http;
    using ApimMessageProcessor;
    using ApimMessageProcessor.RunScopeProcessor;
    public class EventProcessorFactory
    {
        public static IHttpMessageProcessor GetMessageProcessor()
        {
            var logger = new ConsoleLogger(LogLevel.Debug);
            logger.LogDebug("Registering EventProcessor...");

            return new RunscopeHttpMessageProcessor(new HttpClient(), logger);
            // return new ApplicationInsightsHttpMessageProcessor();
        }
    }
}
