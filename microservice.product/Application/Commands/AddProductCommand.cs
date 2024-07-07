using microservice.product.Models;

namespace microservice.product.Application.Commands
{
    public class AddProductCommand : Command
    {
        public string Name { get; private set; }
        public string Brand { get; private set; }
        public decimal Price { get; private set; }
        public long CurrencyCodeId { get; set; }

        public static AddProductCommand Create()
        { 
            return new AddProductCommand(); 
        }

        public AddProductCommand WithProduct(AddProductDto product)
        {
            Name = product.Name;
            Brand = product.Brand;
            Price = product.Price;
            CurrencyCodeId = product.CurrencyCodeId;
            return this;
        }
    }
}
