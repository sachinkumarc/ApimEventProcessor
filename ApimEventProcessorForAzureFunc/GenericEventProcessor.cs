
namespace ApimEventProcessorForAzureFunc
{
    using System.Threading.Tasks;
    using ApimMessageProcessor;

    public class GenericEventProcessor
    {
        readonly IHttpMessageProcessor messageProcessor;
        public GenericEventProcessor(IHttpMessageProcessor messageProcessor)
        {
            this.messageProcessor = messageProcessor;
        }

        public async Task ProcessMessage(string message)
        {
            var httpMessage = HttpMessage.Parse(message);

            await this.messageProcessor.ProcessHttpMessage(httpMessage);
        }
    }
}
