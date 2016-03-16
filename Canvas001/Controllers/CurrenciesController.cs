using System.Threading.Tasks;
using System.Web.Http;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;

namespace Canvas001.Controllers
{
    public class CurrenciesController : ApiController
    {
        private readonly IRequestClient<CurrencyRequest, CurrencyResponse> _client;

        public CurrenciesController(IRequestClient<CurrencyRequest, CurrencyResponse> client)
        {
            _client = client;
        }

        public async Task<CurrencyResponse> Get()
        {
            return await _client.Request(new CurrencyRequest());
        }
    }
}