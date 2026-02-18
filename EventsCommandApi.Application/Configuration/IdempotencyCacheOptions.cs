namespace EventsCommandApi.Application.Configuration
{
    public sealed class IdempotencyCacheOptions
    {
        public const string SectionName = "IdempotencyCache";

        public int SizeLimit { get; set; } = 1024;
        public double CompactionPercentage { get; set; } = 0.25;
        public int ExpirationScanFrequencyMinutes { get; set; } = 5;
        public int CacheDurationHours { get; set; } = 24;
    }
}