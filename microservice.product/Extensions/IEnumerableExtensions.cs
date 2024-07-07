namespace microservice.product.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> input)
        {
            if (input == null) return true;
            return !input.Any();
        }
    }
}
