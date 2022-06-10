
using Microsoft.EntityFrameworkCore;
using ToysAndGames.DataAccess;
using ToysAndGames.Models;
using ToysAndGames.Services.Services;
using WebApiTests.Utilities;

namespace ToysAndGames.WebApiTests
{
    public class ProductServiceTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public ProductServiceTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Trait("Product", "Interface")]
        [Fact]
        public void GetProducts_ReturnsProducts()
        {
            //Arrange
            var productsMock = new Mock<IProductService>();
            productsMock.Setup(p => p.GetProducts())
                .Returns(new List<ProductDTO> { new ProductDTO(),
                new ProductDTO()});

            //Act
            var result = productsMock.Object.GetProducts();

            //Assert
            Assert.NotEmpty(result);
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));

        }

        [Trait("Product", "Interface")]
        [Fact]
        public void GetProducts_ReturnsEmptyList()
        {
            //Arrange
            var productsMock = new Mock<IProductService>();
            productsMock.Setup(p => p.GetProducts())
                .Returns(new List<ProductDTO>());

            //Act
            var result = productsMock.Object.GetProducts();

            //Assert
            Assert.Empty(result);
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Trait("Product", "Interface")]
        [Theory]
        [GetProductByIdData]
        public void GetProductById_ReturnsProduct_IfProductIdExists(int id, bool expected)
        {
            //Arrange
            var productsMock = new Mock<IProductService>();
            productsMock.Setup(p => p.GetProductById(It.IsInRange<int>(1, 4, Moq.Range.Inclusive)))
                .Returns(new ProductDTO
                {
                    ProductId = id,
                    Name = "Hot Wheels",
                    Description = "Twin Mill",
                    AgeRestriction = 3,
                    CompanyId = 1,
                    Price = 25,
                    CompanyName = "Mattel"
                });

            //Act
            var result = productsMock.Object.GetProductById(id);

            //Assert
            Assert.Equal(expected, result == null);
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Trait("Product", "Interface")]
        [Fact]
        public void AddProduct_ReturnsProductId()
        {
            //Arrange
            ProductDTO productDTO = new()
            {
                ProductId = 1,
                Name = "Hot Wheels",
                Description = null,
                AgeRestriction = null,
                Price = 25.6m,
                CompanyId = 1
            };

            var productsMock = new Mock<IProductService>();
            productsMock.Setup(p => p.AddProduct(It.IsAny<ProductDTO>()))
                .Returns(productDTO.ProductId);
            //Act
            var result = productsMock.Object.AddProduct(productDTO);

            //Assert
            Assert.IsType<int>(result);
            _outputHelper.WriteLine($"ProductId returned: {result.ToString()}");
        }

        [Trait("Product", "Interface")]
        [Theory]
        [ProductExistsData]
        public void ProductExists_ReturnsTrueIfProductExists(int productId, bool expected)
        {
            //Arrange
            var productsMock = new Mock<IProductService>();
            productsMock
                .Setup(p => p.ProductExists(It.IsInRange<int>(1, 4, Moq.Range.Inclusive)))
                .Returns(true);

            //Act
            var result = productsMock.Object.ProductExists(productId);

            //Assert
            Assert.Equal(expected, result);
            _outputHelper.WriteLine(result.ToString());
        }

        [Trait("Product", "Context")]
        [Fact]
        public void UpdateProduct_GivenExistingProduct_UpdatesProduct_UsingContext()
        {
            //Arrange
            var productDTO = new ProductDTO
            {
                ProductId = 1,
                Name = "Name1",
                AgeRestriction = 55,
                Description = "Desc",
                CompanyId = 1,
                Price = 145.99m
            };


            var productsMockSet = DbSetMockUtility.GetQueryableMock(new Product
            {
                ProductId = 1,
                Name = "Name",
                AgeRestriction = null,
                Description = null,
                CompanyId = 1,
                Price = 100
            });

            var productMockContext = new Mock<ToysAndGamesContext>();
            productMockContext.Setup(m => m.Products).Returns(productsMockSet.Object);

            ProductService productService = new ProductService(productMockContext.Object);

            //Act
            productService.UpdateProduct(productDTO.ProductId, productDTO);
            var result = productMockContext.Object.Products.FirstOrDefault(p => p.ProductId == productDTO.ProductId);

            //Assert
            productsMockSet.Verify(p => p.Update(It.IsAny<Product>()), Times.Once);
            productMockContext.Verify(p => p.SaveChanges(), Times.Once);

            _outputHelper.WriteLine($"ProductDTO with updated fields: {JsonConvert.SerializeObject(productDTO)}");
            _outputHelper.WriteLine($"Product after update: {JsonConvert.SerializeObject(result)}");
        }

        [Trait("Product", "Context")]
        [Fact]
        public void UpdateProduct_GivenNoExistingProduct_DoesNotExecuteContextMethods_UsingContext()
        {
            //Arrange
            var productDTO = new ProductDTO
            {
                ProductId = 10,
                Name = "Name1",
                AgeRestriction = 55,
                Description = "Desc",
                CompanyId = 1,
                Price = 145.99m
            };


            var productsMockSet = DbSetMockUtility.GetQueryableMock(new Product
            {
                ProductId = 1,
                Name = "Name",
                AgeRestriction = null,
                Description = null,
                CompanyId = 1,
                Price = 100
            });

            var productMockContext = new Mock<ToysAndGamesContext>();
            productMockContext.Setup(m => m.Products).Returns(productsMockSet.Object);

            ProductService productService = new ProductService(productMockContext.Object);

            //Act
            productService.UpdateProduct(productDTO.ProductId, productDTO);
            var result = productMockContext.Object.Products.FirstOrDefault(p => p.ProductId == productDTO.ProductId);

            //Assert
            Assert.Null(result);
            productsMockSet.Verify(p => p.Update(It.IsAny<Product>()), Times.Never);
            productMockContext.Verify(p => p.SaveChanges(), Times.Never);

            _outputHelper.WriteLine($"ProductDTO with updated fields: {JsonConvert.SerializeObject(productDTO)}");
            _outputHelper.WriteLine($"Product after update: {JsonConvert.SerializeObject(result)}");
        }

        [Trait("Product", "Context")]
        [Fact]
        public void AddProduct_GivenProductDto_ReturnsIntId_UsingContext()
        {
            //Arrange
            var productDTO = new ProductDTO
            {
                ProductId = 1,
                Name = "Name1",
                AgeRestriction = 55,
                Description = "Desc",
                CompanyId = 1,
                Price = 145.99m
            };


            var productsMockSet = new Mock<DbSet<Product>>();

            var productMockContext = new Mock<ToysAndGamesContext>();
            productMockContext.Setup(m => m.Products).Returns(productsMockSet.Object);

            ProductService productService = new ProductService(productMockContext.Object);

            //Act
            var result = productService.AddProduct(productDTO);

            //Assert
            Assert.IsType<int>(result);
            productsMockSet.Verify(p => p.Add(It.IsAny<Product>()), Times.Once);
            productMockContext.Verify(p => p.SaveChanges(), Times.Once);

            _outputHelper.WriteLine($"ProductId added: {result}");
        }


        [Trait("Product", "Context")]
        [Fact]
        public void AddProduct_GivenNullOLbject_ReturnsIdMinus1_UsingContext()
        {
            //Arrange
            ProductDTO productDTO = null;

            var productsMockSet = new Mock<DbSet<Product>>();

            var productMockContext = new Mock<ToysAndGamesContext>();
            productMockContext.Setup(m => m.Products).Returns(productsMockSet.Object);

            ProductService productService = new ProductService(productMockContext.Object);

            //Act
            var result = productService.AddProduct(productDTO);

            //Assert
            Assert.Equal(-1, result);
            productsMockSet.Verify(p => p.Add(It.IsAny<Product>()), Times.Never);
            productMockContext.Verify(p => p.SaveChanges(), Times.Never);

            _outputHelper.WriteLine($"ProductId added: {result}");
        }

        [Trait("Product", "Context")]
        [Fact]
        public void DeleteProduct_RemovesProduct_UsingContext()
        {
            //Arrange
            var productId = 1;


            var productsMockSet = DbSetMockUtility.GetQueryableMock(new Product
            {
                ProductId = 1,
                Name = "Name",
                AgeRestriction = null,
                Description = null,
                CompanyId = 1,
                Price = 100
            });

            var productMockContext = new Mock<ToysAndGamesContext>();
            productMockContext.Setup(m => m.Products).Returns(productsMockSet.Object);

            ProductService productService = new ProductService(productMockContext.Object);

            //Act
            productService.DeleteProduct(productId);

            //Assert
            productsMockSet.Verify(p => p.Remove(It.IsAny<Product>()), Times.Once);
            productMockContext.Verify(p => p.SaveChanges(), Times.Once);

            _outputHelper.WriteLine($"ProductId deleted: {productId}");
        }

        [Trait("Product", "Context")]
        [Fact]
        public void DeleteProduct_GivenNoExistingProduct_DoesNotRemovesProduct_UsingContext()
        {
            //Arrange
            var productId = 10;


            var productsMockSet = DbSetMockUtility.GetQueryableMock(new Product
            {
                ProductId = 1,
                Name = "Name",
                AgeRestriction = null,
                Description = null,
                CompanyId = 1,
                Price = 100
            });

            var productMockContext = new Mock<ToysAndGamesContext>();
            productMockContext.Setup(m => m.Products).Returns(productsMockSet.Object);

            ProductService productService = new ProductService(productMockContext.Object);

            //Act
            productService.DeleteProduct(productId);

            //Assert
            productsMockSet.Verify(p => p.Remove(It.IsAny<Product>()), Times.Never);
            productMockContext.Verify(p => p.SaveChanges(), Times.Never);

            _outputHelper.WriteLine($"Attempted deletion of ProductId: {productId}");
        }
    }
}