using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using FluentValidation;

namespace EventsCommandApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssemblyContaining<DTOs.EventRequest>(ServiceLifetime.Singleton);

            services.AddScoped(typeof(Filters.ValidationFilter<>));
            services.AddScoped<Filters.IdempotencyFilter>();
            
            services.Configure<Configuration.IdempotencyCacheOptions>(options =>
                configuration.GetSection(Configuration.IdempotencyCacheOptions.SectionName).Bind(options));

            var cacheOptions = configuration
                .GetSection(Configuration.IdempotencyCacheOptions.SectionName)
                .Get<Configuration.IdempotencyCacheOptions>() ?? new();

            services.AddMemoryCache(options =>
            {
                options.SizeLimit = cacheOptions.SizeLimit;
                options.CompactionPercentage = cacheOptions.CompactionPercentage;
                options.ExpirationScanFrequency = TimeSpan.FromMinutes(cacheOptions.ExpirationScanFrequencyMinutes);
            });

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<DTOs.EventRequest>());

            return services;
        }
    }
}