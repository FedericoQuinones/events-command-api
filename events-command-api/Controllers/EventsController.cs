using EventsCommandApi.Application.Filters;
using EventsCommandApi.Application.UseCases;
using EventsCommandApi.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace EventsCommandApi.Api.Controllers
{
    [ApiController]
    [Route("Event")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IMediator _mediator;
        public EventsController(IMediator mediator, ILogger<EventsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EventResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEvents(CancellationToken ct)
        {
            try
            {
                var command = new GetEventsCommand();
                var response = await _mediator.Send(command, ct);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding the events.");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "An error occurred while creating the event" });
            }
        }

        [HttpPost]
        [ServiceFilter(typeof(IdempotencyFilter))]
        [ServiceFilter(typeof(ValidationFilter<EventRequest>))]
        [ProducesResponseType(typeof(EventResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostEvent([FromBody] EventRequest request, CancellationToken ct)
        {
            try
            {
                var command = new CreateEventCommand(request.Name, request.Payload);
                var response = await _mediator.Send(command, ct);

                return CreatedAtAction(nameof(PostEvent), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event. Name: {EventName}", request.Name);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { error = "An error occurred while creating the event" });
            }
        }
    }
}


