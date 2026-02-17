using EventsCommandApi.Application.DTOs;
using FluentValidation;

namespace EventsCommandApi.Application.Validations
{
    public sealed class EventRequestValidator : AbstractValidator<EventRequest>
    {
        public EventRequestValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Length is longer than 100");

            RuleFor(x => x.Payload)
                .NotEmpty().WithMessage("Payload is required");
        }
    }
}
