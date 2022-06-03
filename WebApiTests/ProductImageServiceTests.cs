
namespace WebApiTests
{
    public class ProductImageServiceTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public ProductImageServiceTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void GetProductImages_ReturnsProductImages(int productId)
        {
            //Arrange
            var productImagesMock = new Mock<IProductImageService>();
            productImagesMock
                .Setup(p => p.GetProductImages(It.IsInRange<int>(1, 4, Moq.Range.Inclusive)))
                .Returns(new List<ProductImageDTO>() { new ProductImageDTO() });

            //Act
            var result = productImagesMock.Object.GetProductImages(productId);

            //Assert
            Assert.NotEmpty(result);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void GetProductImages_ReturnsEmptyListIfProductDoesNotExists(int productId)
        {
            //Arrange
            var productImagesMock = new Mock<IProductImageService>();
            productImagesMock
                .Setup(p => p.GetProductImages(It.IsInRange<int>(-1, 0, Moq.Range.Inclusive)))
                .Returns(new List<ProductImageDTO>());

            //Act
            var result = productImagesMock.Object.GetProductImages(productId);

            //Assert
            Assert.Empty(result);
        }
    }
}
