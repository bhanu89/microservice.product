using microservice.product.Data.Entities;

namespace microservice.product.Models
{
    public record MoneyDto
    {
        public MoneyDto(decimal price, CurrencyEntity currencyEntity)
        {
            this.Id = currencyEntity.Id;
            this.Amount = price;
            this.CurrencyCode = currencyEntity.CurrencyCode;
            this.CurrencySymbol = currencyEntity.CurrencySymbol;
        }

        public long Id { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal Amount { get; set; }
    }
}
