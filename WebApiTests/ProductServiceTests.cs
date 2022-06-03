
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
                .Returns(new List<ProductDTO> { new ProductDTO(),
                new ProductDTO()});

            //Act
            var result = productsMock.Object.GetProducts();

            //Assert
            Assert.NotEmpty(result);
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));

        }

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

        [Fact]
        public void AddProduct_ReturnsProductId()
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
            _outputHelper.WriteLine(result.ToString());
        }

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
    }
}