using microservice.product.Data.Entities;

namespace microservice.product.Models
{
    public record ProductDto
    {
        public ProductDto(ProductEntity productEntity, CurrencyEntity currencyEntity)
        {
            this.Id = productEntity.Id;
            this.Name = productEntity.Name;
            this.Brand = productEntity.Brand;
            this.Price = new MoneyDto(productEntity.Price, currencyEntity);
            this.Version = productEntity.Version;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public MoneyDto Price { get; set; }
        public long Version { get; set; }
    }
}
