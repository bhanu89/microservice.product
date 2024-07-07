namespace microservice.product.Data.Entities
{
    public record CurrencyEntity : Entity
    {
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
    }
}
