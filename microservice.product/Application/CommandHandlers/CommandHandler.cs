using microservice.product.Application.Commands;
using microservice.product.Models;
using System.Diagnostics;

namespace microservice.product.Application.CommandHandlers
{
    public abstract class CommandHandler<TCommand, TResult> where TCommand : Command
    {
        protected ILogger _logger;

        public CommandHandler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<Result<TResult>> HandleAsync(TCommand command)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                return await HandleInternalAsync(command);
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

        protected abstract Task<Result<TResult>> HandleInternalAsync(TCommand command);
    }
}
