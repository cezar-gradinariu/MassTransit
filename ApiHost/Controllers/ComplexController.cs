using System.Threading.Tasks;
using System.Web.Http;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;

namespace Canvas001.Controllers
{
    [RoutePrefix("api/Complex")]
    public class ComplexController : ApiController
    {
        private readonly IRequestClient<CurrencyRequest, CurrencyResponse> _client;

        public ComplexController(IRequestClient<CurrencyRequest, CurrencyResponse> client)
        {
            _client = client;
        }

        [Route("{Name}/{Brand}/{Amount}")]
        [HttpGet]
        public async Task<int> Get([FromUri] ComplexRequest request)
        {
            return await _client.Request(new CurrencyRequest())
                .ContinueWith(t =>
                {
                    if (t.Result != null && t.Result.Currencies != null)
                        return t.Result.Currencies.Count;
                    return 0;
                });
        }
    }
}