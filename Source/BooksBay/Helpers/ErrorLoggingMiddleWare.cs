namespace BooksBay.Helpers
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public readonly ILogger<ErrorLoggingMiddleware> _logger;

        public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError($"BooksBay Error: The following error happened: {e.Message} {e.StackTrace}");
                throw;
            }
        }
    }
}
