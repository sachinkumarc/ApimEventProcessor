
namespace ApimEventProcessorForAzureFunc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using ApimMessageProcessor;
    using ApimMessageProcessor.RunScopeProcessor;
    using ApimMessageProcessor.ApplicationInsights;
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
