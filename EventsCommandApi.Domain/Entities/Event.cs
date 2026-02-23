using MongoDB.Bson.Serialization.Attributes;
using EventsCommandApi.Domain.Enums;
using MongoDB.Bson;

namespace EventsCommandApi.Domain.Entities
{
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public StatusEvent Status { get; private set; }
        public string Payload { get; private set; } = string.Empty;
        public int Version { get; private set; }
        public int LastPublishedVersion { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        protected Event() { }

        public Event(string name, string payload)
        {
            Id = ObjectId.GenerateNewId().ToString();
            Name = name;
            Payload = payload;
            Status = StatusEvent.New;
            Version = 1;
            LastPublishedVersion = 0;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

        public void UpdatePayload(string payload)
        {
            Payload = payload;
            Status = StatusEvent.Updated;
            Version++;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
