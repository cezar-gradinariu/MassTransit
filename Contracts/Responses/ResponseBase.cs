namespace Contracts.Responses
{
    public class ResponseBase
    {
        public Error Error { get; set; }
        public Error Validation { get; set; }
    }
}