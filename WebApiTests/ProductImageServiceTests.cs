
using Microsoft.EntityFrameworkCore;
using ToysAndGames.DataAccess;
using ToysAndGames.Models;
using ToysAndGames.Services.Services;

namespace WebApiTests
{
    public class ProductImageServiceTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public ProductImageServiceTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Trait("ProductImage","Interface")]
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public async void GetProductImages_ReturnsProductImages(int productId)
        {
            //Arrange
            var productImagesMock = new Mock<IProductImageService>();
            productImagesMock
                .Setup(p => p.GetProductImages(It.IsInRange<int>(1, 4, Moq.Range.Inclusive)))
                .ReturnsAsync(new List<ProductImageDTO>() { new ProductImageDTO() });

            //Act
            var result = await productImagesMock.Object.GetProductImages(productId);

            //Assert
            Assert.NotEmpty(result);
        }

        [Trait("ProductImage", "Interface")]
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async void GetProductImages_ReturnsEmptyListIfProductDoesNotExists(int productId)
        {
            //Arrange
            var productImagesMock = new Mock<IProductImageService>();
            productImagesMock
                .Setup(p => p.GetProductImages(It.IsInRange<int>(-1, 0, Moq.Range.Inclusive)))
                .ReturnsAsync(new List<ProductImageDTO>());

            //Act
            var result = await productImagesMock.Object.GetProductImages(productId);

            //Assert
            Assert.Empty(result);
        }
    }
}
