using Microsoft.Extensions.Caching.Memory;

namespace microservice.product.Middleware
{
    public class SlidingRateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _memoryCache;
        private readonly int _limit;
        private readonly TimeSpan _window;

        public SlidingRateLimitMiddleware(RequestDelegate next, IMemoryCache memoryCache, int limit, TimeSpan window)
        {
            _next = next;
            _memoryCache = memoryCache;
            _limit = limit;
            _window = window;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientId = context.Connection.RemoteIpAddress.ToString();

            if (!_memoryCache.TryGetValue(clientId, out List<DateTime> requestTimes))
            {
                requestTimes = new List<DateTime>();
            }

            var now = DateTime.UtcNow;

            requestTimes = requestTimes.Where(x => now - x < _window).ToList();
            requestTimes.Add(now);

            if (requestTimes.Count > _limit)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                return;
            }

            _memoryCache.Set(clientId, requestTimes, _window);

            await _next(context);
        }
    }
}
