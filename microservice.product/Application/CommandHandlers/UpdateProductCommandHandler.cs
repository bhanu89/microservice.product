using microservice.product.Application.Commands;
using microservice.product.Data.Entities;
using microservice.product.Data.Repositories;
using microservice.product.Models;

namespace microservice.product.Application.CommandHandlers
{
    public class UpdateProductCommandHandler : CommandHandler<UpdateProductCommand, Task>
    {
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IRepository<CurrencyEntity> _currencyRepository;

        public UpdateProductCommandHandler(ILogger<UpdateProductCommandHandler> logger, IRepository<ProductEntity> productRepository,
            IRepository<CurrencyEntity> currencyRepository) : base(logger)
        {
            _productRepository = productRepository;
            _currencyRepository = currencyRepository;
        }

        protected async override Task<Result<Task>> HandleInternalAsync(UpdateProductCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var currencyEntity = (await _currencyRepository.GetAsync([command.CurrencyCodeId])).FirstOrDefault() ?? throw new ArgumentException("CurrencyCodeId is invalid");

            var productEntity = new ProductEntity(command);

            await _productRepository.UpdateAsync(productEntity);

            return new Result<Task>(value: null);
        }
    }
}
