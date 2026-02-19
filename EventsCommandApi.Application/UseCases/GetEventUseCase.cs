using EventsCommandApi.Application.Common.Interfaces;
using EventsCommandApi.Application.DTOs;
using EventsCommandApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using MediatR;

namespace EventsCommandApi.Application.UseCases
{
    public sealed class GetEventsCommand() : IRequest<IEnumerable<EventResponse>>;

    public sealed class GetEventsHandler : IRequestHandler<GetEventsCommand, IEnumerable<EventResponse>>
    {
        private readonly IEventRepository _repository;
        private readonly ILogger<GetEventsHandler> _logger;
        public GetEventsHandler(IEventRepository repository, ILogger<GetEventsHandler> logger) 
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<IEnumerable<EventResponse>> Handle(GetEventsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting events...");

            var eventList = await _repository.GetAllAsync(cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Retrieved {Count} events", eventList.Count);

            return MapListEvent(eventList);
        }

        private static IEnumerable<EventResponse> MapListEvent(IEnumerable<Event> listEventDomain)
        {
            var result = listEventDomain.Select(e => new EventResponse(
                e.Id ?? string.Empty,
                e.Name,
                e.Payload,
                e.Status.ToString(),
                e.CreatedAt,
                e.UpdatedAt,
                e.Version
            ));

            return result;
        }
    }
}
