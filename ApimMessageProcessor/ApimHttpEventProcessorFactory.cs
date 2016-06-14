namespace ApimMessageProcessor
{
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    ///  Allows the EventProcessor instances to have services injected into the constructor
    /// </summary>
    public class ApimHttpEventProcessorFactory : IEventProcessorFactory
    {
        private IHttpMessageProcessor _HttpMessageProcessor;
        private ILogger _Logger;

        public ApimHttpEventProcessorFactory(IHttpMessageProcessor httpMessageProcessor, ILogger logger)
        {
            _HttpMessageProcessor = httpMessageProcessor;
            _Logger = logger;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return new ApimEventProcessor(_HttpMessageProcessor, _Logger);
        }
    }
}
