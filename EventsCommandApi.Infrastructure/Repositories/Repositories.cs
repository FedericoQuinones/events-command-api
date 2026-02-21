using EventsCommandApi.Application.Common.Interfaces;
using EventsCommandApi.Infrastructure.Persistence;
using EventsCommandApi.Application.Configuration;
using EventsCommandApi.Domain.Entities;
using Microsoft.Extensions.Options;

namespace EventsCommandApi.Infrastructure.Repositories
{
    public sealed class EventRepository : IEventRepository
    {
        private readonly IDbContext _context;
        private readonly string _collectionName;

        public EventRepository(IDbContext context, IOptions<MongoOptions> options)
        {
            _context = context;
            _collectionName = options.Value.EventsCollection;
        }

        public async Task<IReadOnlyList<Event>> GetAllAsync(CancellationToken ct = default)
            => await _context.FindAllAsync<Event>(_collectionName, ct);

        public async Task<Event?> GetByIdAsync(string id, CancellationToken ct = default)
        {
            return await _context.FindByIdAsync<Event>(_collectionName, id, ct);
        }

        public async Task<string> CreateAsync(Event @event, CancellationToken ct = default)
            => await _context.InsertAsync(_collectionName, @event, ct);

        public async Task UpdateAsync(Event @event, CancellationToken ct = default)
            => await _context.UpdateAsync(_collectionName, @event.Id, @event, ct);

        public async Task DeleteAsync(string id, CancellationToken ct = default)
        {
            await _context.DeleteAsync<Event>(_collectionName, id, ct);
        }
    }
}
