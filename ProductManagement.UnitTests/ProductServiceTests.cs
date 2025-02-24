using Moq;
using ProductManagement.Core.Entities;
using ProductManagement.Core.Exceptions;
using ProductManagement.Core.Interfaces;
using ProductManagement.Core.Interfaces.Repositories;
using ProductManagement.Core.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductUOW> _mockUnitOfWork;
    private readonly Mock<IEFRepository<Product>> _mockProductRepository;
    private readonly Mock<IEFRepository<ReusableProductId>> _mockReusableProductIdRepository;
    private readonly Mock<IAppLogger<ProductService>> _mockLogger;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _mockUnitOfWork = new Mock<IProductUOW>();
        _mockProductRepository = new Mock<IEFRepository<Product>>();
        _mockReusableProductIdRepository = new Mock<IEFRepository<ReusableProductId>>();
        _mockLogger = new Mock<IAppLogger<ProductService>>();

        _mockUnitOfWork.Setup(uow => uow.ProductRepository).Returns(_mockProductRepository.Object);
        _mockUnitOfWork.Setup(uow => uow.ReusableProductIdRepository).Returns(_mockReusableProductIdRepository.Object);
        _mockProductRepository.Setup(repo => repo.GetNextProductIdAsync()).ReturnsAsync("000001");
        _productService = new ProductService(_mockUnitOfWork.Object, _mockLogger.Object);
    }

    // 1. Get All Products - Success
    [Fact]
    public async Task GetAllProductsAsync_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = "000001", Name = "Product A" },
            new Product { Id = "000002", Name = "Product B" }
        };
        _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Contains(result, p => p.Name == "Product A");
        Assert.Contains(result, p => p.Name == "Product B");
    }

    // 2. Get Product by ID - Found
    [Fact]
    public async Task GetProductAsync_ShouldReturnProduct_WhenExists()
    {
        // Arrange
        var product = new Product { Id = "000001", Name = "Test Product" };
        _mockProductRepository.Setup(repo => repo.GetByIdAsync("000001")).ReturnsAsync(product);

        // Act
        var result = await _productService.GetProductAsync("000001");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("000001", result.Id);
    }

    // 3. Get Product by ID - Not Found
    [Fact]
    public async Task GetProductAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        _mockProductRepository.Setup(repo => repo.GetByIdAsync("000999")).ReturnsAsync((Product)null);

        // Act
        var result = await _productService.GetProductAsync("000999");

        // Assert
        Assert.Null(result);
    }

    // 4. Add Product - Null Product
    [Fact]
    public async Task AddProductAsync_ShouldThrowException_WhenProductIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _productService.AddProductAsync(null));
    }

    [Fact]
    public async Task AddProductAsync_ShouldThrowException_WhenProductAlreadyExists()
    {
        // Arrange
        var existingProduct = new Product { Id = "000001", Name = "Duplicate Product" };

        // Mock repository to return an existing product when GetByIdAsync is called
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(existingProduct.Id))
            .ReturnsAsync(existingProduct);

        var newProduct = new Product { Id = "000001", Name = "Duplicate Product" };

        // Act & Assert
        await Assert.ThrowsAsync<ProductAlreadyExistsException>(() => _productService.AddProductAsync(newProduct));
    }

    // 6. Add Product - Success
    [Fact]
    public async Task AddProductAsync_ShouldAddProduct_WhenValid()
    {
        // Arrange
        var newProduct = new Product { Id = "000003", Name = "New Product" };
        _mockProductRepository.Setup(repo => repo.GetByIdAsync("000001")).ReturnsAsync((Product)null);
        _mockProductRepository.Setup(repo => repo.AddAsync(newProduct)).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _productService.AddProductAsync(newProduct);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("000001", result.Product.Id);
    }

    // 7. Update Product - Not Found
    [Fact]
    public async Task UpdateProductAsync_ShouldThrowException_WhenProductNotFound()
    {
        // Arrange
        var product = new Product { Id = "000999", Name = "Updated Product" };
        _mockProductRepository.Setup(repo => repo.GetByIdAsync("000999")).ReturnsAsync((Product)null);

        // Act & Assert
        await Assert.ThrowsAsync<ProductNotFoundException>(() => _productService.UpdateProductAsync(product));
    }

    // 8. Delete Product - Success
    [Fact]
    public async Task DeleteProductAsync_ShouldDeleteProduct_WhenProductExists()
    {
        // Arrange
        var product = new Product { Id = "000001", Name = "Product to Delete" };
        _mockProductRepository.Setup(repo => repo.GetByIdAsync("000001")).ReturnsAsync(product);
        _mockReusableProductIdRepository.Setup(repo => repo.AddAsync(It.IsAny<ReusableProductId>())).Returns(Task.CompletedTask);
        _mockProductRepository.Setup(repo => repo.Delete(product));
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _productService.DeleteProductAsync("000001");

        // Assert
        Assert.NotNull(result);
    }

    // 9. Delete Product - Not Found
    [Fact]
    public async Task DeleteProductAsync_ShouldThrowException_WhenProductNotFound()
    {
        // Arrange
        _mockProductRepository.Setup(repo => repo.GetByIdAsync("000999")).ReturnsAsync((Product)null);

        // Act & Assert
        await Assert.ThrowsAsync<ProductNotFoundException>(() => _productService.DeleteProductAsync("000999"));
    }

    // 10. Add to Stock - Success
    [Fact]
    public async Task AddToStockAsync_ShouldIncreaseStock_WhenProductExists()
    {
        // Arrange
        var product = new Product { Id = "000001", Stock = 10 };
        _mockProductRepository.Setup(repo => repo.GetByIdAsync("000001")).ReturnsAsync(product);
        _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _productService.AddToStockAsync("000001", 5);

        // Assert
        Assert.Equal(15, result.Product.Stock);
    }

    // 11. Decrement Stock - Insufficient Stock
    [Fact]
    public async Task DecrementStockAsync_ShouldReturnMessage_WhenStockInsufficient()
    {
        // Arrange
        var product = new Product { Id = "000001", Stock = 5 };
        _mockProductRepository.Setup(repo => repo.GetByIdAsync("000001")).ReturnsAsync(product);

        // Act
        var result = await _productService.DecrementStockAsync("000001", 10);

        // Assert
        Assert.Contains("Insufficient stock", result.Message);
    }
}