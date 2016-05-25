using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web.Http;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using Serilog;

namespace ApiHost.Controllers
{
    public class CurrenciesController : ApiController
    {
        private readonly IRequestClient<CurrencyRequest, CurrencyResponse> _client;
        private readonly ILogger _logger;

        public CurrenciesController(IRequestClient<CurrencyRequest, CurrencyResponse> client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<CurrencyResponse> Get()
        {
            var callId = (string) CallContext.LogicalGetData("callId");
            _logger.Information("A call with id {callId} is sent to the worker", callId);
            return await _client.Request(new CurrencyRequest());
        }
    }
}