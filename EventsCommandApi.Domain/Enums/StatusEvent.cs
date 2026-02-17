namespace EventsCommandApi.Domain.Enums
{
    public enum StatusEvent
    {
        New = 0,
        Updated = 1,
        PublishedToIngress = 2,
        Enriched = 3,
        Routed = 4,
        Done = 5,
        Failed = 6,
        DeadLettered = 7,
        Discarded = 8
    }
}