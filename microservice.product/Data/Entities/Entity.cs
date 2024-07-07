namespace microservice.product.Data.Entities
{
    public record Entity
    {
        public long Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset LastModified { get; set; }

        public string GetCacheKey()
        {
            return $"{nameof(Entity)}-{Id}";
        }
    }
}
