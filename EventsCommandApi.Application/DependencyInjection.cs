using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace EventsCommandApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<DTOs.EventRequest>(ServiceLifetime.Singleton);

            services.AddScoped(typeof(Filters.ValidationFilter<>));
            services.AddScoped<Filters.IdempotencyFilter>();
            
            services.AddMemoryCache();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<DTOs.EventRequest>());

            return services;
        }
    }
}