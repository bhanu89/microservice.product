using microservice.product.Application.Commands;
using microservice.product.Data.Entities;
using microservice.product.Data.Repositories;
using microservice.product.Models;

namespace microservice.product.Application.CommandHandlers
{
    public class DeleteProductCommandHandler : CommandHandler<DeleteProductCommand, Task>
    {
        private IRepository<ProductEntity> _productRepository;

        public DeleteProductCommandHandler(ILogger<DeleteProductCommand> logger, IRepository<ProductEntity> productRepository) : base(logger)
        {
            _productRepository = productRepository;
        }

        protected override async Task<Result<Task>> HandleInternalAsync(DeleteProductCommand command)
        {
            if (command?.Id == null) throw new ArgumentNullException(nameof(command));

            await _productRepository.DeleteAsync(command.Id);
            return new Result<Task>(value: null);
        }
    }
}
