using microservice.product.Models;

namespace microservice.product.Application.Queries
{
    public class GetProductsQuery : Query
    {
        internal IEnumerable<long> Ids { get; private set; }
        private string? Name;
        private string? Brand;
        private string? CurrencyCode;
        private decimal? LowerBoundPrice;
        private decimal? UpperBoundPrice;

        public static GetProductsQuery Create()
        {
            return new GetProductsQuery();
        }

        public GetProductsQuery WithIds(IEnumerable<long>? ids)
        {
            if (ids != null && ids.Any())
            {
                Ids = ids;
            }
            return this;
        }

        /* For future implementations
        public GetProductsQuery WithName(string? name)
        {
            if (name !=null && name != string.Empty)
            {
                Name = name;
            }
            return this;
        }

        public GetProductsQuery WithBrand(string? brand) {
            if (brand != null && brand != string.Empty) {
                Brand = brand;
            }
            return this;
        }

        public GetProductsQuery WithCurrencyCode(string? currencyCode)
        {
            if (currencyCode != null && currencyCode != string.Empty)
            {
                CurrencyCode = currencyCode;
            }
            return this;
        }

        public GetProductsQuery WithLowerBoundPrice(decimal? lowerBoundPrice)
        {
            if (lowerBoundPrice.HasValue)
            {
                LowerBoundPrice = lowerBoundPrice;
            }
            return this;
        }

        public GetProductsQuery WithUpperBoundPrice(decimal? upperBoundPrice)
        {
            if (upperBoundPrice.HasValue)
            {
                UpperBoundPrice = upperBoundPrice;
            }
            return this;
        }
        */

    }
}
