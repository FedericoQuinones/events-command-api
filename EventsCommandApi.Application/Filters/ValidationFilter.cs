using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace EventsCommandApi.Application.Filters
{
    public sealed class ValidationFilter<T> : IAsyncActionFilter where T : class
    {
        private readonly IValidator<T> _validator;

        public ValidationFilter(IValidator<T> validator)
        {
            _validator = validator;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var parameter = context.ActionArguments
                .FirstOrDefault(p => p.Value is T).Value as T;

            if (parameter is null)
            {
                await next();
                return;
            }

            var validationResult = await _validator.ValidateAsync(parameter);
            
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    property = e.PropertyName,
                    error = e.ErrorMessage
                });

                context.Result = new BadRequestObjectResult(errors);
                return;
            }

            await next();
        }
    }
}