using Microsoft.Extensions.Caching.Memory;
using EventsCommandApi.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsCommandApi.Application.Filters
{
    public sealed class IdempotencyFilter : IAsyncActionFilter
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(24);

        public IdempotencyFilter(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKey) 
                || string.IsNullOrWhiteSpace(idempotencyKey))
            {
                context.Result = new BadRequestObjectResult(new 
                { 
                    error = "Idempotency-Key header is required" 
                });
                return;
            }

            var key = $"idempotency:{idempotencyKey}";

            if (_cache.TryGetValue(key, out IdempotentResponse? cachedResponse))
            {
                context.Result = new ObjectResult(cachedResponse!.Body)
                {
                    StatusCode = cachedResponse.StatusCode
                };
                context.HttpContext.Response.Headers.Append("X-Idempotent-Replayed", "true");
                return;
            }

            var executedContext = await next();

            if (executedContext.Result is ObjectResult objectResult && objectResult.StatusCode == 201)
            {
                var response = new IdempotentResponse
                {
                    StatusCode = objectResult.StatusCode.Value,
                    Body = objectResult.Value
                };

                _cache.Set(key, response, _cacheDuration);
            }
        }
    }
}