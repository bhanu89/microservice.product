using microservice.product.Application.Queries;
using microservice.product.Data.Entities;
using microservice.product.Data.Repositories;
using microservice.product.Extensions;
using microservice.product.Models;

namespace microservice.product.Application.QueryHandlers
{
    public class GetProdcutsQueryHandler : QueryHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private IRepository<ProductEntity> _productRepository;
        private IRepository<CurrencyEntity> _currencyRepository;

        public GetProdcutsQueryHandler(ILogger<GetProdcutsQueryHandler> logger, IRepository<ProductEntity> productRepository,
            IRepository<CurrencyEntity> currencyRepository) : base(logger)
        {
            _productRepository = productRepository;
            _currencyRepository = currencyRepository;
        }

        protected async override Task<Result<IEnumerable<ProductDto>>> HandleInternalAsync(GetProductsQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            
            var response = new List<ProductDto>();

            var productEntities = await _productRepository.GetAsync(query.Ids);

            var currencyIdsToFetch = productEntities.Select(x => x.CurrencyCodeId).ToList();
            var currencyEntities = await _currencyRepository.GetAsync(currencyIdsToFetch);
            Dictionary<long, CurrencyEntity> currencyLookup = currencyEntities.ToDictionary(c => c.Id, c => c);

            foreach(var productEntity in productEntities)
            {
                response.Add(new ProductDto(productEntity, currencyLookup[productEntity.CurrencyCodeId]));
            }

            return new Result<IEnumerable<ProductDto>>(response);
        }
    }
}
