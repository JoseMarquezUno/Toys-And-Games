using Moq;
using Newtonsoft.Json;
using ToysAndGames.Models;
using ToysAndGames.Models.DTO;
using ToysAndGames.Services.Contracts;
using WebApiTests.TestData;
using Xunit.Abstractions;

namespace ToysAndGames.WebApiTests
{
    public class ProductServiceTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public ProductServiceTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        [Fact]
        public void GetProducts_ReturnsProducts()
        {
            //Arrange
            var productsMock = new Mock<IProductService>();
            productsMock.Setup(p => p.GetProducts())
                .Returns(new[]{new Product
                {
                    ProductId = 1,
                    Name = "Hot Wheels",
                    Description = "Twin Mill",
                    AgeRestriction = 3,
                    CompanyId = 1,
                    Price = 25,
                    Company = new Company { CompanyId = 1, Name = "Mattel" }
                },
                new Product
                {
                    ProductId = 1,
                    Name = "Monopoly",
                    Description = null,
                    AgeRestriction = null,
                    CompanyId = 2,
                    Price = 250,
                    Company = new Company { CompanyId = 2, Name = "Hasbro" }
                }});

            //Act
            var result = productsMock.Object.GetProducts();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));

        }

        [Fact]
        public void GetProducts_ReturnsEmptyList()
        {
            //Arrange
            var productsMock = new Mock<IProductService>();
            productsMock.Setup(p => p.GetProducts())
                .Returns(new List<Product>());

            //Act
            var result = productsMock.Object.GetProducts();

            //Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Theory]
        [GetProductByIdData]
        public void GetProductById_ReturnsProduct_IfProductIdExists(int id, bool isNull)
        {
            //Arrange
            var productsMock = new Mock<IProductService>();
            productsMock.Setup(p => p.GetProductById(It.IsInRange<int>(1,4,Moq.Range.Inclusive)))
                .Returns(new Product
                {
                    ProductId = id,
                    Name = "Hot Wheels",
                    Description = "Twin Mill",
                    AgeRestriction = 3,
                    CompanyId = 1,
                    Price = 25,
                    Company = new Company { CompanyId = 1, Name = "Mattel" }
                });

            //Act
            var result = productsMock.Object.GetProductById(id);

            //Assert
            Assert.Equal(isNull, result == null);
            if (result != null)
            {
                Assert.Equal(id, result.ProductId); 
            }
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void AddProduct_ReturnsInt()
        {
            //Arrange
            ProductDTO productDTO = new()
            {
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
        }
    }
}