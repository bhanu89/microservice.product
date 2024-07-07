using microservice.product.Application.Commands;
using microservice.product.Application.Queries;
using microservice.product.Application.QueryHandlers;
using microservice.product.Data.Entities;
using microservice.product.Data.Repositories;
using microservice.product.Models;

namespace microservice.product.Application.CommandHandlers
{
    public class AddProductCommandHandler : CommandHandler<AddProductCommand, Task>
    {
        private IRepository<ProductEntity> _productRepository;
        private IRepository<CurrencyEntity> _currencyRepository;

        public AddProductCommandHandler(ILogger<AddProductCommandHandler> logger, IRepository<ProductEntity> productRepository,
            IRepository<CurrencyEntity> currencyRepository) : base(logger)
        {
            _productRepository = productRepository;
            _currencyRepository = currencyRepository;
        }

        protected async override Task<Result<Task>> HandleInternalAsync(AddProductCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            var currencyEntity = (await _currencyRepository.GetAsync([command.CurrencyCodeId])).FirstOrDefault() ?? throw new ArgumentException("CurrencyCodeId is invalid");

            var productEntity = new ProductEntity(command);

            await _productRepository.AddAsync(productEntity);

            return new Result<Task>(value: null);
        }
    }
}
