
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
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
        public async void GetProducts_ReturnsProducts()
        {
            //Arrange
            var productsMock = new Mock<IProductService>();
            productsMock.Setup(p => p.GetProducts())
                .ReturnsAsync(new List<ProductDTO> { new ProductDTO(),
                new ProductDTO()});

            //Act
            var result = await productsMock.Object.GetProducts();

            //Assert
            Assert.NotEmpty(result);
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));

        }

        [Trait("Product", "Interface")]
        [Fact]
        public async void GetProducts_ReturnsEmptyList()
        {
            //Arrange
            var productsMock = new Mock<IProductService>();
            productsMock.Setup(p => p.GetProducts())
                .ReturnsAsync(new List<ProductDTO>());

            //Act
            var result = await productsMock.Object.GetProducts();

            //Assert
            Assert.Empty(result);
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Trait("Product", "Interface")]
        [Theory]
        [GetProductByIdData]
        public async void GetProductById_ReturnsProduct_IfProductIdExists(int id, bool expected)
        {
            //Arrange
            var productsMock = new Mock<IProductService>();
            productsMock.Setup(p => p.GetProductById(It.IsInRange<int>(1, 4, Moq.Range.Inclusive)))
                .ReturnsAsync(new ProductDTO
                {
                    Id = id,
                    Name = "Hot Wheels",
                    Description = "Twin Mill",
                    AgeRestriction = 3,
                    CompanyId = 1,
                    Price = 25,
                    CompanyName = "Mattel"
                });

            //Act
            var result = await productsMock.Object.GetProductById(id);

            //Assert
            Assert.Equal(expected, result == null);
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Trait("Product", "Interface")]
        [Fact]
        public async void AddProduct_AddProductInDB()
        {
            //Arrange
            ProductAddDTO productDTO = new()
            {
                Id = 1,
                Name = "Hot Wheels",
                Description = null,
                AgeRestriction = null,
                Price = 25.6m,
                CompanyId = 1
            };

            var productsMock = new Mock<IProductService>();
            productsMock.Setup(p => p.AddProduct(It.IsAny<ProductAddDTO>()))
                .Returns(Task.CompletedTask);
            //Act
            await productsMock.Object.AddProduct(productDTO);

            //Assert
            productsMock.Verify(p => p.AddProduct(It.IsAny<ProductAddDTO>()), Times.Once);
            _outputHelper.WriteLine("The method AddProduct executed once");
        }

        //TODO: Check if the test is correct
        [Trait("Product", "Interface")]
        [Fact]
        public void AddProduct_ThrowsException()
        {
            //Arrange
            ProductAddDTO? productDTO = null;

            var productsMock = new Mock<IProductService>();
            productsMock.Setup(p => p.AddProduct(It.IsAny<ProductAddDTO>()))
                .ThrowsAsync(new OperationCanceledException());
            //Act
            var response = Assert.ThrowsAsync<OperationCanceledException>(()=>productsMock.Object.AddProduct(productDTO));

            //Assert
            Assert.NotNull(response.Result.Message);
            productsMock.Verify(p => p.AddProduct(It.IsAny<ProductAddDTO>()), Times.Once);
            _outputHelper.WriteLine("The method AddProduct executed at least once");
            _outputHelper.WriteLine(JsonConvert.SerializeObject(response.Result));
        }

        [Trait("Product", "Interface")]
        [Theory]
        [ProductExistsData]
        public async void ProductExists_ReturnsTrueIfProductExists(int productId, bool expected)
        {
            //Arrange
            var productsMock = new Mock<IProductService>();
            productsMock
                .Setup(p => p.ProductExists(It.IsInRange<int>(1, 4, Moq.Range.Inclusive)))
                .ReturnsAsync(true);

            //Act
            var result = await productsMock.Object.ProductExists(productId);

            //Assert
            Assert.Equal(expected, result);
            _outputHelper.WriteLine(result.ToString());
        }

        //TODO: Make async operations work in mock
        [Trait("Product", "Context")]
        [Fact]
        public async void UpdateProduct_GivenExistingProduct_UpdatesProduct_UsingContext()
        {
            //Arrange
            var productDTO = new ProductDTO
            {
                Id = 1,
                Name = "Name1",
                AgeRestriction = 55,
                Description = "Desc",
                CompanyId = 1,
                Price = 145.99m
            };

            var products = new List<Product> {new Product
            {
                Id = 1,
                Name = "Name",
                AgeRestriction = null,
                Description = null,
                CompanyId = 1,
                Price = 100
            } };

            var productsMockSet = products.AsQueryable().BuildMockDbSet();

            var productMockContext = new Mock<ToysAndGamesContext>();
            productMockContext.Setup(m => m.Products).Returns(productsMockSet.Object);

            var mapperMock = new Mock<IMapper>();

            ProductService productService = new (productMockContext.Object,mapperMock.Object);

            //Act
            await productService.UpdateProduct(productDTO.Id, productDTO);
            var result = productMockContext.Object.Products.FirstOrDefault(p => p.Id == productDTO.Id);

            //Assert
            productsMockSet.Verify(p => p.Update(It.IsAny<Product>()), Times.Once);
            productMockContext.Verify(p => p.SaveChangesAsync(CancellationToken.None), Times.Once);

            _outputHelper.WriteLine($"ProductDTO with updated fields: {JsonConvert.SerializeObject(productDTO)}");
            _outputHelper.WriteLine($"Product after update: {JsonConvert.SerializeObject(result)}");
        }

        //TODO: Make async operations work in mock
        [Trait("Product", "Context")]
        [Fact]
        public async void UpdateProduct_GivenNoExistingProduct_DoesNotExecuteContextMethods_UsingContext()
        {
            //Arrange
            var productDTO = new ProductDTO
            {
                Id = 10,
                Name = "Name1",
                AgeRestriction = 55,
                Description = "Desc",
                CompanyId = 1,
                Price = 145.99m
            };

            var products = new List<Product> {new Product
            {
                Id = 1,
                Name = "Name",
                AgeRestriction = null,
                Description = null,
                CompanyId = 1,
                Price = 100
            } };

            var productsMockSet = products.AsQueryable().BuildMockDbSet();

            var productMockContext = new Mock<ToysAndGamesContext>();
            productMockContext.Setup(m => m.Products).Returns(productsMockSet.Object);

            var mapperMock = new Mock<IMapper>();

            ProductService productService = new (productMockContext.Object,mapperMock.Object);

            //Act
            await productService.UpdateProduct(productDTO.Id, productDTO);
            var result = productMockContext.Object.Products.FirstOrDefault(p => p.Id == productDTO.Id);

            //Assert
            Assert.Null(result);
            productsMockSet.Verify(p => p.Update(It.IsAny<Product>()), Times.Never);
            productMockContext.Verify(p => p.SaveChangesAsync(CancellationToken.None), Times.Never);

            _outputHelper.WriteLine($"ProductDTO with updated fields: {JsonConvert.SerializeObject(productDTO)}");
            _outputHelper.WriteLine($"Product after update: {JsonConvert.SerializeObject(result)}");
        }

        [Trait("Product", "Context")]
        [Fact]
        public async void AddProduct_GivenProductDto_AddsProduct_UsingContext()
        {
            //Arrange
            var productDTO = new ProductAddDTO
            {
                Id = 1,
                Name = "Name1",
                AgeRestriction = 55,
                Description = "Desc",
                CompanyId = 1,
                Price = 145.99m
            };


            var productsMockSet = new Mock<DbSet<Product>>();

            var productMockContext = new Mock<ToysAndGamesContext>();
            productMockContext.Setup(m => m.Products).Returns(productsMockSet.Object);

            var mapperMock = new Mock<IMapper>();

            ProductService productService = new (productMockContext.Object, mapperMock.Object);

            //Act
            await productService.AddProduct(productDTO);

            //Assert
            productsMockSet.Verify(p => p.AddAsync(It.IsAny<Product>(), CancellationToken.None), Times.Once);
            productMockContext.Verify(p => p.SaveChangesAsync(CancellationToken.None), Times.Once);

            _outputHelper.WriteLine("Add and SaveChanges executed once from Context");
        }


        [Trait("Product", "Context")]
        [Fact]
        public async void AddProduct_GivenNullObject_DoesNotExecuteContextMethods_UsingContext()
        {
            //Arrange
            ProductAddDTO productDTO = null;

            var productsMockSet = new Mock<DbSet<Product>>();

            var productMockContext = new Mock<ToysAndGamesContext>();
            productMockContext.Setup(m => m.Products).Returns(productsMockSet.Object);

            var mapperMock = new Mock<IMapper>();

            ProductService productService = new (productMockContext.Object, mapperMock.Object);

            //Act
            await productService.AddProduct(productDTO);

            //Assert
            productsMockSet.Verify(p => p.Add(It.IsAny<Product>()), Times.Never);
            productMockContext.Verify(p => p.SaveChanges(), Times.Never);

            _outputHelper.WriteLine("Context methods did not got executed");
        }

        //TODO: Make async operations work in mock
        [Trait("Product", "Context")]
        [Fact]
        public async void DeleteProduct_RemovesProduct_UsingContext()
        {
            //Arrange
            var productId = 1;

            var products = new List<Product> {new Product
            {
                Id = 1,
                Name = "Name",
                AgeRestriction = null,
                Description = null,
                CompanyId = 1,
                Price = 100
            } };

            var productsMockSet = products.AsQueryable().BuildMockDbSet();

            var productMockContext = new Mock<ToysAndGamesContext>();
            productMockContext.Setup(m => m.Products).Returns(productsMockSet.Object);

            var mapperMock = new Mock<IMapper>();

            ProductService productService = new (productMockContext.Object,mapperMock.Object);

            //Act
            await productService.DeleteProduct(productId);

            //Assert
            productsMockSet.Verify(p => p.Remove(It.IsAny<Product>()), Times.Once);
            productMockContext.Verify(p => p.SaveChangesAsync(CancellationToken.None), Times.Once);

            _outputHelper.WriteLine($"ProductId deleted: {productId}");
        }

        //TODO: Make async operations work in mock
        [Trait("Product", "Context")]
        [Fact]
        public async void DeleteProduct_GivenNoExistingProduct_DoesNotRemovesProduct_UsingContext()
        {
            //Arrange
            var productId = 10;

            var products = new List<Product> {new Product
            {
                Id = 1,
                Name = "Name",
                AgeRestriction = null,
                Description = null,
                CompanyId = 1,
                Price = 100
            }};

            var productsMockSet = products.AsQueryable().BuildMockDbSet();

            var productMockContext = new Mock<ToysAndGamesContext>();
            productMockContext.Setup(m => m.Products).Returns(productsMockSet.Object);

            var mapperMock = new Mock<IMapper>();

            ProductService productService = new (productMockContext.Object, mapperMock.Object);

            //Act
            await productService.DeleteProduct(productId);

            //Assert
            productsMockSet.Verify(p => p.Remove(It.IsAny<Product>()), Times.Never);
            productMockContext.Verify(p => p.SaveChangesAsync(CancellationToken.None), Times.Never);

            _outputHelper.WriteLine($"Attempted deletion of ProductId: {productId}");
        }
    }
}