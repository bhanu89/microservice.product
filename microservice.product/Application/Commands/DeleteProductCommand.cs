namespace microservice.product.Application.Commands
{
    public class DeleteProductCommand : Command
    {
        public long Id { get; private set; }

        public static DeleteProductCommand Create()
        {
            return new DeleteProductCommand();
        }

        public DeleteProductCommand WithId(long Id)
        {
            this.Id = Id;
            return this;
        }
    }
}
