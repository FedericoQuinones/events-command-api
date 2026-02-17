using EventsCommandApi.Application.Common.Interfaces;
using EventsCommandApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using MediatR;

namespace EventsCommandApi.Application.UseCases
{
    public sealed record CreateEventCommand(string Name, string Payload) : IRequest<Event>;

    public sealed class CreateEventHandler : IRequestHandler<CreateEventCommand, Event>
    {
        private readonly IEventRepository _repository;
        private readonly ILogger<CreateEventHandler> _logger;

        public CreateEventHandler(IEventRepository repository, ILogger<CreateEventHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Event> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating event. Name: {EventName}", request.Name);

            var eventEntity = new Event(request.Name, request.Payload);
            var createdId = await _repository.CreateAsync(eventEntity, cancellationToken);

            _logger.LogInformation("Event created successfully. Id: {EventId}, Name: {EventName}",
                createdId, request.Name);

            return eventEntity;
        }
    }
}