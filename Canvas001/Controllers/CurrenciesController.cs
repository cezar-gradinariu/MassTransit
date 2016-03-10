using System.Threading.Tasks;
using System.Web.Http;
using Contracts;
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

        //public List<ICurrencyInfo> Get()
        //{
        //    var t = Task.Run(async () =>
        //    {
        //        var response = await _client.Request(new CurrencyRequest());
        //        return response.Currencies;
        //    });
        //    t.Wait();
        //    return t.Result;
        //}

        public async Task<CurrencyResponse> Get()
        {
            return await _client.Request(new CurrencyRequest());
        }
    }
}