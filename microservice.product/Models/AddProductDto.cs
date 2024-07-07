namespace microservice.product.Models
{
    public record AddProductDto
    {
        public required string Name { get; set; }
        public required string Brand { get; set; }
        public required decimal Price { get; set; }
        public required long CurrencyCodeId { get; set; }
    }
}
