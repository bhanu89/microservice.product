using microservice.product.Models;

namespace microservice.product.Application.Commands
{
    public class UpdateProductCommand : Command
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public string Brand { get; private set; }
        public decimal Price { get; private set; }
        public long CurrencyCodeId { get; set; }
        public long Version { get; private set; }

        public static UpdateProductCommand Create()
        {
            return new UpdateProductCommand();
        }

        public UpdateProductCommand WithProductUpdate(UpdateProductDto updateProductDto)
        {
            Id = updateProductDto.Id;
            Name = updateProductDto.Name;
            Brand = updateProductDto.Brand;
            Price = updateProductDto.Price;
            CurrencyCodeId = updateProductDto.CurrencyCodeId;
            Version = updateProductDto.Version;
            return this;
        }
    }
}
