using EventsCommandApi.Domain.Entities;

namespace EventsCommandApi.Application.Common.Interfaces
{
    public interface IEventRepository
    {
        Task<IReadOnlyList<Event>> GetAllAsync(CancellationToken ct = default);
        Task<Event?> GetByIdAsync(string id, CancellationToken ct = default);
        Task<string> CreateAsync(Event @event, CancellationToken ct = default);
        Task UpdateAsync(Event @event, CancellationToken ct = default);
        Task DeleteAsync(string id, CancellationToken ct = default);
    }
}
