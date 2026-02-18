namespace EventsCommandApi.Application.DTOs
{
    public sealed record EventRequest(
        string Name,
        string Payload
    );
}