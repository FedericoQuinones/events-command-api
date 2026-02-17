namespace EventsCommandApi.Application.DTOs
{
    public sealed record EventResponse(
        string Id,
        string Name,
        string Payload,
        string Status,
        DateTime CreatedAt,
        int Version
    );
}