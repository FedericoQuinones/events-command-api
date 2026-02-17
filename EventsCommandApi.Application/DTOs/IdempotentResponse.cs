namespace EventsCommandApi.Application.DTOs
{
    public sealed class IdempotentResponse
    {
        public int StatusCode { get; set; }
        public object? Body { get; set; }
    }
}
