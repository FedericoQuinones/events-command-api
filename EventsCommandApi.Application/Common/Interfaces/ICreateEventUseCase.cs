using EventsCommandApi.Application.DTOs;

namespace EventsCommandApi.Application.Common.Interfaces
{
    public interface ICreateEventUseCase
    {
        Task<EventResponse> ExecuteAsync(EventRequest request, CancellationToken ct = default);
    }
}