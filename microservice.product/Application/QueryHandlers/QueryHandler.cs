using microservice.product.Application.Queries;
using microservice.product.Models;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace microservice.product.Application.QueryHandlers
{
    public abstract class QueryHandler<TQuery, TResult> where TQuery : Query
    {
        protected ILogger _logger;

        public QueryHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<Result<TResult>> HandleAsync(TQuery query)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                return await HandleInternalAsync(query);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Encountered error in {GetType().Name}", ex);
                return new Result<TResult>(ex);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation($"{GetType().Name} took {stopwatch.ElapsedMilliseconds}ms to execute");
            }
        }

        protected abstract Task<Result<TResult>> HandleInternalAsync(TQuery query);
    }
}
