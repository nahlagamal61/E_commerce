namespace E_Commerce_Test.Services
{
    using AutoMapper;
    using E_Commerce_Application.Interfaces.Presistences;
    using E_Commerce_Application.Services;
    using E_commerce_DataModeling.Models;
    using E_commerce_DataModeling.ViewModels;
    using Moq;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class ProductServiceTest
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ProductService _productService;

        public ProductServiceTest()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _mapperMock = new Mock<IMapper>();
            _productService = new ProductService(_productRepoMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddProduct_WithValidProductDto_ShouldReturnProductResponse()
        {
            // Arrange
            var productDto = new Product_Create_DTO
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10
            };

            var product = new Product
            {
                ProductID = Guid.NewGuid(),
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price
            };

            var productResponse = new Product_Response
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };

            _mapperMock.Setup(mapper => mapper.Map<Product>(productDto)).Returns(product);
            _productRepoMock.Setup(repo => repo.AddProduct(product)).ReturnsAsync(product);
            _mapperMock.Setup(mapper => mapper.Map<Product_Response>(product)).Returns(productResponse);

            // Act
            var result = await _productService.AddProduct(productDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productResponse.ProductID, result.ProductID);
            Assert.Equal(productResponse.Name, result.Name);
            Assert.Equal(productResponse.Description, result.Description);
            Assert.Equal(productResponse.Price, result.Price);
        }

        [Fact]
        public async Task GetProductByID_WithValidProductId_ShouldReturnProductResponse()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product
            {
                ProductID = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 10
            };

            var productResponse = new Product_Response
            {
                ProductID = productId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };

            _productRepoMock.Setup(repo => repo.GetProductByID(productId)).ReturnsAsync(product);
            _mapperMock.Setup(mapper => mapper.Map<Product_Response>(product)).Returns(productResponse);

            // Act
            var result = await _productService.GetProductByID(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productResponse.ProductID, result.ProductID);
            Assert.Equal(productResponse.Name, result.Name);
            Assert.Equal(productResponse.Description, result.Description);
            Assert.Equal(productResponse.Price, result.Price);
          
        }

        [Fact]
        public async Task GetProductByID_WithInvalidProductId_ShouldThrowException()
        {
            // Arrange
            var invalidProductId = Guid.NewGuid();
            _productRepoMock.Setup(repo => repo.GetProductByID(invalidProductId)).ReturnsAsync((Product)null);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () =>
                await _productService.GetProductByID(invalidProductId));
            _productRepoMock.Verify(repo => repo.GetProductByID(invalidProductId), Times.Once);
        }

        [Fact]
        public async Task UpdateProduct_WithValidProductIdAndDto_ShouldReturnUpdatedProductResponse()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productDto = new UpdateProduct_VM
            {
                Price = 20,
                StockQuantity = 20,
            };

            var product = new Product
            {
                ProductID = productId,
                Name = "Old Product",
                Description = "Old Description",
                Price = 10
            };

            var updatedProduct = new Product
            {
                ProductID = productId,
                Price = productDto.Price
            };

            var updatedProductResponse = new Product_Response
            {
                ProductID = productId,
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                Price = updatedProduct.Price
            };

            _mapperMock.Setup(mapper => mapper.Map<Product>(productDto)).Returns(updatedProduct);
            _productRepoMock.Setup(repo => repo.GetProductByID(productId)).ReturnsAsync(product);
            _productRepoMock.Setup(repo => repo.UpdateProduct(productId, updatedProduct)).ReturnsAsync(updatedProduct);
            _mapperMock.Setup(mapper => mapper.Map<Product_Response>(updatedProduct)).Returns(updatedProductResponse);

            // Act
            var result = await _productService.UpdateProduct(productId, productDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedProductResponse.ProductID, result.ProductID);
            Assert.Equal(updatedProductResponse.Name, result.Name);
            Assert.Equal(updatedProductResponse.Description, result.Description);
            Assert.Equal(updatedProductResponse.Price, result.Price);
        }

        [Fact]
        public async Task UpdateProduct_WithInvalidProductId_ShouldThrowException()
        {
            // Arrange
            var invalidProductId = Guid.NewGuid();
            var productDto = new UpdateProduct_VM
            {
               StockQuantity = 100, 
                Price = 20
            };

            _productRepoMock.Setup(repo => repo.GetProductByID(invalidProductId)).ReturnsAsync((Product)null);

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(async () =>
                await _productService.UpdateProduct(invalidProductId, productDto));
        }

    }
}

