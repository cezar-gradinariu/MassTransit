using System.Threading.Tasks;
using System.Web.Http;
using Contracts.Requests;
using Contracts.Responses;
using MassTransit;
using System.Net.Http;
using System.Net;

namespace ApiHost.Controllers
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
        //public async Task<IHttpActionResult> Get([FromUri] ComplexRequest request)
        public async Task<int> Get([FromUri] ComplexRequest request)
        {
            var result = await _client.Request(new CurrencyRequest())
                .ContinueWith(t =>
                {
                    return t.Result?.Currencies?.Count ?? 0;
                });
            return result;
            //return Ok(result);
        }
    }
}