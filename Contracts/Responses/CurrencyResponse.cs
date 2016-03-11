using System.Collections.Generic;

namespace Contracts.Responses
{
    public class CurrencyResponse : ResponseBase
    {
        public List<CurrencyInfo> Currencies { get; set; }
    }
}