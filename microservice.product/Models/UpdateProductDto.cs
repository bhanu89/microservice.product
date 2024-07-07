namespace microservice.product.Models
{
    public record UpdateProductDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public required string Brand { get; set; }
        public required decimal Price { get; set; }
        public required long CurrencyCodeId { get; set; }
        public required long Version { get; set; }
    }
}
