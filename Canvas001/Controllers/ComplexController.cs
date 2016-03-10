using System.Threading.Tasks;
using System.Web.Http;
using Canvas001.Attributes;
using Contracts;
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

        //[Route("{Name}/{Brand}/{Amount}")]
        //[HttpGet]
        //[Validation]
        //public int Get([FromUri] ComplexRequest request)
        //{
        //    var t = Task.Run(async () =>
        //    {
        //        var response = await _client.Request(new CurrencyRequest());
        //        return response.Currencies;
        //    });
        //    t.Wait();
        //    var x = t.Result;

        //    return x.Count;
        //}

        [Route("{Name}/{Brand}/{Amount}")]
        [HttpGet]
        [Validation]
        public async Task<int> Get([FromUri] ComplexRequest request)
        {
            return await _client.Request(new CurrencyRequest())
                                .ContinueWith(t => t.Result.Currencies.Count);
        }
    }
}