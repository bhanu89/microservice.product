using microservice.product.Application.Commands;

namespace microservice.product.Data.Entities
{
    public record ProductEntity : Entity
    {
        // required for Dapper
        public ProductEntity() { }

        public ProductEntity(AddProductCommand addProduct)
        {
            Name = addProduct.Name;
            Brand = addProduct.Brand;
            CurrencyCodeId = addProduct.CurrencyCodeId;
            Price = addProduct.Price;
        }

        public ProductEntity(UpdateProductCommand updateProduct)
        {
            Id = updateProduct.Id;
            Name = updateProduct.Name;
            Brand = updateProduct.Brand;
            CurrencyCodeId = updateProduct.CurrencyCodeId;
            Price = updateProduct.Price;
            Version = updateProduct.Version;
        }

        public string Name { get; set; }
        public string Brand { get; set; }
        public long Version { get; set; }
        public decimal Price { get; set; }
        public long CurrencyCodeId { get; set; }
        public bool Deleted { get; set; }
    }
}
