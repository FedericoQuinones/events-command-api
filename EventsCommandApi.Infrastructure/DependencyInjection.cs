using EventsCommandApi.Application.Common.Interfaces;
using EventsCommandApi.Infrastructure.Repositories;
using EventsCommandApi.Infrastructure.Persistence;
using EventsCommandApi.Application.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace EventsCommandApi.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MongoOptions>(configuration.GetSection("MongoDb").Bind);

            services.AddSingleton<IValidateOptions<MongoOptions>, MongoOptionsValidator>();

            services.AddSingleton<IDbContext, MongoDbContext>();
            services.AddScoped<IEventRepository, EventRepository>();

            return services;
        }
    }
}