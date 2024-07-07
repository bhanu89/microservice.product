namespace microservice.product.Models
{
    public record Result<T>
    {
        public T? Value { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public Result(T value, bool success = true, string errorMessage = "")
        {
            Value = value;
            Success = success;
            ErrorMessage = errorMessage;
        }

        public Result(Exception ex)
        {
            Success = false;
            ErrorMessage = ex.Message;
        }
    }
}
