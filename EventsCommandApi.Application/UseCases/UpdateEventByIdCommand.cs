using EventsCommandApi.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using MediatR;

namespace EventsCommandApi.Application.UseCases
{
    public sealed record UpdateEventByIdCommand(string Id, string Payload) : IRequest;

    public sealed class UpdateEventByIdCommandHandler : IRequestHandler<UpdateEventByIdCommand>
    {
        private readonly ILogger<UpdateEventByIdCommandHandler> _logger;
        private readonly IEventRepository _repository;
        public UpdateEventByIdCommandHandler(ILogger<UpdateEventByIdCommandHandler> logger, IEventRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task Handle(UpdateEventByIdCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updatating event with id: {Id}; payload: {Payload}", request.Id, request.Payload);

            var requestEvent = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (requestEvent == null)
            {
                _logger.LogWarning("Event with id {Id} not found for update", request.Id);
                throw new KeyNotFoundException($"Event with id '{request.Id}' not found.");
            }

            requestEvent.UpdatePayload(request.Payload);
            await _repository.UpdateAsync(requestEvent, cancellationToken);

            _logger.LogInformation("Event id: {Id} has been updated succesfully", request.Id);
        }
    }
}
