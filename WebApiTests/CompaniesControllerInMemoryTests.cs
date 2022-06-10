using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiTests.Utilities;

namespace WebApiTests
{
    public class CompaniesControllerInMemoryTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper _outputHelper;
        private readonly HttpClient _client;
        public CompaniesControllerInMemoryTests(ITestOutputHelper outputHelper, CustomWebApplicationFactory<Program> _factory)
        {
            _outputHelper = outputHelper;
            _client = _factory.CreateClient();
        }

        [Trait("IntegrationTestInMemory", "CompaniesController")]
        [Fact]
        public async void EnsureCompanyAdd()
        {
            //Arrange
            var companyDTO = new CompanyDTO
            {
                Name = "Lego"
            };

            var jsonObject = JsonConvert.SerializeObject(companyDTO);
            var httpContent = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            //Act
            var companiesBeforeAddResponse = await _client.GetAsync("/api/Companies");
            var companyAddResponse = await _client.PostAsync("/api/Companies/Company", httpContent);
            var companiesAfterAddResponse = await _client.GetAsync("/api/Companies");
            var getAddedCompany = companyAddResponse.Headers.Location;
            var companyGetResponse = await _client.GetAsync(getAddedCompany);

            //Assert
            var companiesBeforeAdd = JsonConvert.DeserializeObject<List<CompanyDTO>>(companiesBeforeAddResponse.Content.ReadAsStringAsync().Result);
            var companiesAfterAdd = JsonConvert.DeserializeObject<List<CompanyDTO>>(companiesAfterAddResponse.Content.ReadAsStringAsync().Result);
            var companyGet = JsonConvert.DeserializeObject<CompanyDTO>(companyGetResponse.Content.ReadAsStringAsync().Result);

            Assert.NotEqual(companiesBeforeAdd.Count, companiesAfterAdd.Count);
            Assert.NotNull(companyGet);
            Assert.Equal(companyDTO.Name, companyGet.Name);

            _outputHelper.WriteLine($"Companies before addition: {JsonConvert.SerializeObject(companiesBeforeAdd)}");
            _outputHelper.WriteLine($"CompanyDTO to be added: {JsonConvert.SerializeObject(companyDTO)}");
            _outputHelper.WriteLine($"Companies after addition: {JsonConvert.SerializeObject(companiesAfterAdd)}");
            _outputHelper.WriteLine($"CompanyDTO added GET result: {JsonConvert.SerializeObject(companyGet)}");
        }

        [Trait("IntegrationTestInMemory", "CompaniesController")]
        [Fact]
        public async void EnsureCompanyDeletion()
        {
            //Arrange
            var companyId = 1;

            //Act
            var companiesBeforeDeleteResponse = await _client.GetAsync("/api/Companies");
            var companyDeleteResponse = await _client.DeleteAsync("/api/Companies/Company/" + companyId);
            var companiesAfterDeleteResponse = await _client.GetAsync("/api/Companies");
            var companyGetResponse = await _client.GetAsync("/api/Companies/Company/" + companyId);

            //Assert
            var companiesBeforeDelete = JsonConvert.DeserializeObject<List<CompanyDTO>>(companiesBeforeDeleteResponse.Content.ReadAsStringAsync().Result);
            var companyDeleteStatus = companyDeleteResponse.StatusCode;
            var companiesAfterDelete = JsonConvert.DeserializeObject<List<CompanyDTO>>(companiesAfterDeleteResponse.Content.ReadAsStringAsync().Result);
            var companyGet = JsonConvert.DeserializeObject<CompanyDTO>(companyGetResponse.Content.ReadAsStringAsync().Result);

            Assert.NotEqual(companiesBeforeDelete.Count, companiesAfterDelete.Count);
            Assert.Null(companyGet);
            Assert.Equal(System.Net.HttpStatusCode.NoContent, companyDeleteStatus);

            _outputHelper.WriteLine($"Companies before deletion: {JsonConvert.SerializeObject(companiesBeforeDelete)}");
            _outputHelper.WriteLine($"CompanyId to be deleted: {companyId}");
            _outputHelper.WriteLine($"Companies after deletion: {JsonConvert.SerializeObject(companiesAfterDelete)}");
            _outputHelper.WriteLine($"StatusCode of GetCompany with companyId of deleted company: {companyDeleteStatus}");
        }

        [Trait("IntegrationTestInMemory", "CompaniesController")]
        [Fact]
        public async void EnsureCompanyUpdate()
        {
            //Arrange
            var companyDTO = new CompanyDTO
            {
                Name = "Matel"
            };
            var companyId = 1;
            var jsonObject = JsonConvert.SerializeObject(companyDTO);
            var httpContent =  new StringContent(jsonObject,Encoding.UTF8,"application/json");

            //Act
            var companyBeforeUpdateResponse = await _client.GetAsync("/api/Companies/Company/" + companyId);
            var updateCompanyResponse = await _client.PutAsync("/api/Companies/Company/" + companyId, httpContent);
            var companyAfterUpdateResponse = await _client.GetAsync("/api/Companies/Company/" + companyId);

            //Assert
            var companyBeforeUpdate = JsonConvert.DeserializeObject<CompanyDTO>(companyBeforeUpdateResponse.Content.ReadAsStringAsync().Result);
            var companyUpdated = JsonConvert.DeserializeObject<CompanyDTO>(updateCompanyResponse.Content.ReadAsStringAsync().Result);
            var companyAfterUpdate = JsonConvert.DeserializeObject<CompanyDTO>(companyAfterUpdateResponse.Content.ReadAsStringAsync().Result);

            Assert.NotNull(companyBeforeUpdate);
            Assert.Equal(companyUpdated.Name, companyAfterUpdate.Name);
            Assert.NotEqual(companyBeforeUpdate, companyAfterUpdate);

            _outputHelper.WriteLine($"Company before update: {JsonConvert.SerializeObject(companyBeforeUpdate)}");
            _outputHelper.WriteLine($"CompanyDTO to be updated: {JsonConvert.SerializeObject(companyDTO)}");
            _outputHelper.WriteLine($"Company updated: {JsonConvert.SerializeObject(companyUpdated)}");
            _outputHelper.WriteLine($"Company after update: {JsonConvert.SerializeObject(companyAfterUpdate)}");
        }
    }
}
