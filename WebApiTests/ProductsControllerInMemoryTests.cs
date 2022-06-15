using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiTests.Utilities;

namespace WebApiTests
{
    public class ProductsControllerInMemoryTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly HttpClient _client;
        public ProductsControllerInMemoryTests(ITestOutputHelper outputHelper, CustomWebApplicationFactory<Program> _factory)
        {
            _outputHelper = outputHelper;
            _client = _factory.CreateClient();
        }

        [Trait("IntegrationTestInMemory", "ProductsController")]
        [Fact]
        public async void EnsureProductAdd()
        {
            //Arrange
            var productDTO = new ProductDTO
            {
                Name = "Lego Porsche",
                AgeRestriction = 99,
                Description = "Porsche made of legos",
                Price = 950.60m,
                CompanyId = 1
            };

            var jsonObject = JsonConvert.SerializeObject(productDTO);
            var httpContent = new StringContent(jsonObject,Encoding.UTF8,"application/json");

            //Act
            var productsBeforeAddResponse = await _client.GetAsync("/api/Products");
            var productAddResponse = await _client.PostAsync("/api/Products/Product", httpContent);
            var productsAfterAddResponse = await _client.GetAsync("/api/Products");
            var getAddedProduct = productAddResponse.Headers.Location;
            var getProductAddedResponse = await _client.GetAsync(getAddedProduct);

            //Assert
            var productsBeforeAdd = JsonConvert.DeserializeObject<List<ProductDTO>>(productsBeforeAddResponse.Content.ReadAsStringAsync().Result);
            var productGet = JsonConvert.DeserializeObject<ProductDTO>(getProductAddedResponse.Content.ReadAsStringAsync().Result);
            var productsAfterAdd = JsonConvert.DeserializeObject<List<ProductDTO>>(productsAfterAddResponse.Content.ReadAsStringAsync().Result);

            Assert.NotEqual(productsBeforeAdd.Count,productsAfterAdd.Count);
            Assert.NotNull(productGet);

            _outputHelper.WriteLine($"Products before add: {JsonConvert.SerializeObject(productsBeforeAdd)}");
            _outputHelper.WriteLine($"ProductDTO to be added: {JsonConvert.SerializeObject(productDTO)}");
            _outputHelper.WriteLine($"Products after add: {JsonConvert.SerializeObject(productsAfterAdd)}");
            _outputHelper.WriteLine($"ProductDTO added GET result: {JsonConvert.SerializeObject(productGet)}");
        }

        [Trait("IntegrationTestInMemory", "ProductsController")]
        [Fact]
        public async void EnsureProductDelete()
        {
            //Arrange
            var productId = 1;

            //Act
            var productsBeforeDeleteResponse = await _client.GetAsync("/api/Products");
            var productDeleteResponse = await _client.DeleteAsync($"/api/Products/Product/{productId}" );
            var productsAfterDeleteResponse = await _client.GetAsync("/api/Products");
            var productGetResponse = await _client.GetAsync("/api/Products/Product/" + productId);

            //Assert
            var productsBeforeDelete = JsonConvert.DeserializeObject<List<ProductDTO>>(productsBeforeDeleteResponse.Content.ReadAsStringAsync().Result);
            var productDeleteStatusCode = productDeleteResponse.StatusCode;
            var productsAfterDelete = JsonConvert.DeserializeObject<List<ProductDTO>>(productsAfterDeleteResponse.Content.ReadAsStringAsync().Result);
            var productGet = JsonConvert.DeserializeObject<ProductDTO>(productGetResponse.Content.ReadAsStringAsync().Result);

            Assert.NotEqual(productsBeforeDelete.Count, productsAfterDelete.Count);
            Assert.Null(productGet);
            Assert.Equal(System.Net.HttpStatusCode.NoContent, productDeleteStatusCode);

            _outputHelper.WriteLine($"Products before delete: {JsonConvert.SerializeObject(productsBeforeDelete)}");
            _outputHelper.WriteLine($"ProductId to be deleted: {productId}");
            _outputHelper.WriteLine($"Products after delete: {JsonConvert.SerializeObject(productsAfterDelete)}");
            _outputHelper.WriteLine($"StatusCode of GetProduct with productId of deleted product: {productDeleteStatusCode}");

        }

        [Trait("IntegrationTestInMemory", "ProductsController")]
        [Fact]
        public async void EnsureProductUpdate()
        {
            //Arrange
            var productId = 1;

            var productResponse = await _client.GetAsync("/api/Products/Product/" + productId);
            var product = JsonConvert.DeserializeObject<ProductDTO>(productResponse.Content.ReadAsStringAsync().Result);
            product.Description = null;
            product.AgeRestriction = null;
            product.Price = 500.25m;

            var jsonObject = JsonConvert.SerializeObject(product);
            var httpContent = new StringContent(jsonObject,Encoding.UTF8,"application/json");

            //Act
            var productBeforeUpdateResponse = await _client.GetAsync("/api/Products/Product/" + productId);
            var productUpdateResponse = await _client.PutAsync("/api/Products/Product/" + productId, httpContent);
            var productAfterUpdateResponse = await _client.GetAsync("api/Products/Product/" + productId);

            //Assert
            var productBeforeUpdate = JsonConvert.DeserializeObject<ProductDTO>(productBeforeUpdateResponse.Content.ReadAsStringAsync().Result);
            var productUpdate = JsonConvert.DeserializeObject<ProductDTO>(productUpdateResponse.Content.ReadAsStringAsync().Result);
            var productAfterUpdate = JsonConvert.DeserializeObject<ProductDTO>(productAfterUpdateResponse.Content.ReadAsStringAsync().Result);

            Assert.NotNull(product);
            Assert.NotEqual(productBeforeUpdate, productAfterUpdate);
            Assert.Equal(product.Description, productAfterUpdate.Description);

            _outputHelper.WriteLine($"ProductDTO with updated fields: {JsonConvert.SerializeObject(product)}");
            _outputHelper.WriteLine($"Product before update: {JsonConvert.SerializeObject(productBeforeUpdate)}");
            _outputHelper.WriteLine($"Product updated: {JsonConvert.SerializeObject(productUpdate)}");
            _outputHelper.WriteLine($"Product after update: {JsonConvert.SerializeObject(productAfterUpdate)}");
        }
    }
}
