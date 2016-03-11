using System.Collections.Generic;

namespace Contracts.Responses
{
    public class Error
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public List<ErrorInfo> Errors { get; set; }
    }
}