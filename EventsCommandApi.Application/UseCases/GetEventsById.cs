using EventsCommandApi.Application.Common.Interfaces;
using EventsCommandApi.Application.DTOs;
using EventsCommandApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using MediatR;

namespace EventsCommandApi.Application.UseCases
{
    public sealed record GetEventsByIdCommand(string Id) : IRequest<EventResponse>;

    public sealed class GetEventByIdHandler : IRequestHandler<GetEventsByIdCommand, EventResponse>
    {
        private readonly ILogger<GetEventByIdHandler> _logger;
        private readonly IEventRepository _repository;
        public GetEventByIdHandler(ILogger<GetEventByIdHandler> logger, IEventRepository repository) 
        {
            _logger = logger;
            _repository = repository;
        }
        public async Task<EventResponse> Handle(GetEventsByIdCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting event by id {Id}", request.Id);

            var eventById = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (eventById == null)
            {
                _logger.LogWarning("Event with id {Id} not found", request.Id);
                throw new KeyNotFoundException($"Event with id '{request.Id}' not found.");
            }

            return MapToEventResponse(eventById);
        }

        private static EventResponse MapToEventResponse(Event @event)
        {
            return new EventResponse(
                @event.Id ?? string.Empty,
                @event.Name,
                @event.Payload,
                @event.Status.ToString(),
                @event.CreatedAt,
                @event.UpdatedAt,
                @event.Version
            );
        }
    }
}
