using ProductManagement.Core.Entities;

namespace ProductManagement.Core.ServicesResult
{
    /// <summary>
    /// Represents the result of a product transaction operation.
    /// </summary>
    public class ProductTransactionServiceResult : Result
    {
        public Product Product { get; set; }
    }
}
