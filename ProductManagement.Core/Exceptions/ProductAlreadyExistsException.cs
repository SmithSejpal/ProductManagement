namespace ProductManagement.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when attempting to create a product that already exists.
    /// </summary>
    public class ProductAlreadyExistsException : Exception
    {
        public ProductAlreadyExistsException(string message) : base(message) { }
    }
}
