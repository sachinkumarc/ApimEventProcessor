
namespace ApimMessageProcessor
{
    using System.Threading.Tasks;

    /// <summary>
    /// Abstracts away where the HTTP message will be relayed to
    /// </summary>
    public interface IHttpMessageProcessor
    {
        Task ProcessHttpMessage(HttpMessage message);
    }
}
