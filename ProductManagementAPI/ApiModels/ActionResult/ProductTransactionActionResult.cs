using ProductManagement.Core.Entities;

namespace ProductManagement.API.ApiModels.ActionResult
{
    // <summary>
    /// Represents the result of a product-related transaction in the API.
    /// Contains the product details along with the action result status.
    /// </summary>
    public class ProductTransactionActionResult : ActionResult
    {
        public Product Product { get; set; }
    }
}
