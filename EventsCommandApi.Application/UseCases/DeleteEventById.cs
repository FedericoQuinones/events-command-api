using EventsCommandApi.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using MediatR;

namespace EventsCommandApi.Application.UseCases
{
    public sealed record DeleteEventByIdCommand(string Id) : IRequest;

    public sealed class DeleteEventByIdHandler : IRequestHandler<DeleteEventByIdCommand>
    {
        private readonly IEventRepository _repository;
        private readonly ILogger<DeleteEventByIdHandler> _logger;
        
        public DeleteEventByIdHandler(IEventRepository repository, ILogger<DeleteEventByIdHandler> logger) 
        { 
            _repository = repository;
            _logger = logger;
        }
        public async Task Handle(DeleteEventByIdCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting event with id {Id}", request.Id);

            var deletedCount = await _repository.DeleteAsync(request.Id, cancellationToken);
            
            if (deletedCount == 0) 
            {
                _logger.LogWarning("Event with id {Id} not found for deletion", request.Id);
                throw new KeyNotFoundException($"Event with id '{request.Id}' not found.");
            }

            _logger.LogInformation("Event with id {Id} deleted successfully", request.Id);
        }
    }
}
