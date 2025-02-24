using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.API.ApiModels.ActionResult;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Interfaces.Services;

namespace ProductManagement.API.Controllers
{
    /// <summary>
    /// API controller for managing products.
    /// Provides endpoints to create, update, delete, and retrieve products.
    /// </summary>
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the products list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Product>> List()
            => await _productService.GetAllProductsAsync();

        /// <summary>
        /// Gets the product object based on product ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<Product> GetById(string id)
            => await _productService.GetProductAsync(id);

        /// <summary>
        /// Generates a new product ID and adds it.
        /// </summary>
        [HttpPost]
        public async Task<ProductTransactionActionResult> Post([FromBody] ProductDTO product)
            => _mapper.Map<ProductTransactionActionResult>(await _productService.AddProductAsync(_mapper.Map<Product>(product)));

        /// <summary>
        /// Updates an existing product ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ProductTransactionActionResult> Put(string id, [FromBody]ProductDTO product)
        {
            var productEntity = _mapper.Map<Product>(product);
            productEntity.Id = id;
            return _mapper.Map<ProductTransactionActionResult>(await _productService.UpdateProductAsync(productEntity));
        }

        /// <summary>
        /// Deletes an existing product ID.
        /// </summary>
        [HttpDelete("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ProductTransactionActionResult> Delete(string id)
            => _mapper.Map<ProductTransactionActionResult>(await _productService.DeleteProductAsync(id));

        /// <summary>
        /// Adds the quantity to the existing stock of a product.
        /// </summary>
        [HttpPut("add-to-stock/{id}/{quantity}")]
        public async Task<ProductTransactionActionResult> AddToStock(string id, int quantity)
            => _mapper.Map<ProductTransactionActionResult>(await _productService.AddToStockAsync(id, quantity));

        /// <summary>
        /// Removes the quantity from the stock of a product.
        /// </summary>
        [HttpPut("decrement-stock/{id}/{quantity}")]
        public async Task<ProductTransactionActionResult> DecrementStock(string id, int quantity)
            => _mapper.Map<ProductTransactionActionResult>(await _productService.DecrementStockAsync(id, quantity));
    }
}