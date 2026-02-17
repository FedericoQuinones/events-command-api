using Microsoft.Extensions.Options;

namespace EventsCommandApi.Application.Configuration
{
    public sealed class MongoOptions
    {
        public string ConnectionString { get; init; } = string.Empty;
        public string Database { get; init; } = string.Empty;
        public string EventsCollection { get; init; } = string.Empty;
    }

    public class MongoOptionsValidator : IValidateOptions<MongoOptions>
    {
        public ValidateOptionsResult Validate(string? name, MongoOptions options)
        {
            if (string.IsNullOrEmpty(options.ConnectionString))
                return ValidateOptionsResult.Fail("ConnectionString cannot be null or empty.");
            if (string.IsNullOrEmpty(options.Database))
                return ValidateOptionsResult.Fail("Database cannot be null or empty.");
            if (string.IsNullOrEmpty(options.EventsCollection))
                return ValidateOptionsResult.Fail("EventsCollection cannot be null or empty.");

            return ValidateOptionsResult.Success;
        }
    }
}