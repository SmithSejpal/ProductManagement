namespace ProductManagement.Core.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested product is not found.
    /// </summary>
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string message) : base(message) { }
    }
}
