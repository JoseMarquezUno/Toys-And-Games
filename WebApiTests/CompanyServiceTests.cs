
using Microsoft.EntityFrameworkCore;
using ToysAndGames.DataAccess;
using ToysAndGames.Models;
using ToysAndGames.Services.Services;
using WebApiTests.Utilities;

namespace WebApiTests
{
    public class CompanyServiceTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public CompanyServiceTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        [Trait("Company", "Interface")]
        [Fact]
        public void GetCompanies_ReturnsCompanies()
        {
            //Arrange
            var companiesMock = new Mock<ICompanyService>();
            companiesMock.Setup(c => c.GetCompanies())
            .Returns(new List<CompanyDTO>() { new CompanyDTO(),
                new CompanyDTO()});

            //Act
            var result = companiesMock.Object.GetCompanies();

            //Assert
            Assert.NotEmpty(result);
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Trait("Company", "Interface")]
        [Fact]
        public void GetCompanies_ReturnsEmptyList()
        {
            //Arrange
            var companiesMock = new Mock<ICompanyService>();
            companiesMock
                .Setup(c => c.GetCompanies())
                .Returns(new List<CompanyDTO>());

            //Act
            var result = companiesMock.Object.GetCompanies();

            //Assert
            Assert.Empty(result);
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Trait("Company", "Interface")]
        [Theory]
        [GetCompanyByIdData]
        public void GetCompanyById_ReturnsCompany_IfCompanyExists(int companyId, bool expected)
        {
            //Arrange
            var companiesMock = new Mock<ICompanyService>();
            companiesMock
                .Setup(c => c.GetCompanyById(It.IsInRange<int>(1, 3, Moq.Range.Inclusive)))
                .Returns(new CompanyDTO());

            //Act
            var result = companiesMock.Object.GetCompanyById(companyId);

            //Assert
            Assert.Equal(expected, result == null);

            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Trait("Company", "Interface")]
        [Fact]
        public void AddCompany_ReturnsCompanyId()
        {
            //Arrange
            CompanyDTO companyDTO = new CompanyDTO()
            {
                Name = "company"
            };

            var companiesMock = new Mock<ICompanyService>();
            companiesMock
                .Setup(c => c.AddCompany(It.IsAny<CompanyDTO>()))
                .Returns(companyDTO.CompanyId);

            //Act
            var result = companiesMock.Object.AddCompany(companyDTO);

            //Assert
            Assert.IsType<int>(result);
            _outputHelper.WriteLine(result.ToString());
        }

        [Trait("Company", "Interface")]
        [Theory]
        [CompanyExistsData]
        public void CompanyExists_ReturnsTrueIfCompanyExists(int companyId, bool expected)
        {
            //Arrange
            var companiesMock = new Mock<ICompanyService>();
            companiesMock
                .Setup(c => c.CompanyExists(It.IsInRange<int>(1, 3, Moq.Range.Inclusive)))
                .Returns(true);

            //Act
            var result = companiesMock.Object.CompanyExists(companyId);

            //Assert
            Assert.Equal(expected, result);
            _outputHelper.WriteLine(result.ToString());
        }

        [Trait("Company","Context")]
        [Fact]
        public void UpdateCompany_GivenExistingCompany_UpdatesCompany_UsingContext()
        {
            //Arrange
            var companyDTO = new CompanyDTO()
            {
                CompanyId = 1,
                Name = "Matel"
            };

            var companiesMockSet = DbSetMockUtility.GetQueryableMock(new Company { CompanyId = 1, Name = "Mattel" });

            var mockContext = new Mock<ToysAndGamesContext>();
            mockContext.Setup(m => m.Companies).Returns(companiesMockSet.Object);

            _outputHelper.WriteLine($"Company in mock context: {JsonConvert.SerializeObject(companiesMockSet.Object.First())}");

            CompanyService companyService = new CompanyService(mockContext.Object);

            //Act
            companyService.UpdateCompany(companyDTO.CompanyId, companyDTO);
            var result = mockContext.Object.Companies.FirstOrDefault(c => c.CompanyId == companyDTO.CompanyId);

            //Assert
            Assert.Equal(companyDTO.Name, result.Name);
            companiesMockSet.Verify(c => c.Update(It.IsAny<Company>()), Times.Once);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);
            _outputHelper.WriteLine($"CompanyDTO with updated fields: {JsonConvert.SerializeObject(companyDTO)}");
            _outputHelper.WriteLine($"Company updated in context: {JsonConvert.SerializeObject(result)}");
        }

        [Trait("Company", "Context")]
        [Fact]
        public void UpdateCompany_GivenNonExistingCompany_DoesNotExecuteContextMethods_UsingContext()
        {
            //Arrange
            var companyDTO = new CompanyDTO()
            {
                CompanyId = 10,
                Name = "Matel"
            };

            var companiesMockSet = DbSetMockUtility.GetQueryableMock(new Company { CompanyId = 1, Name = "Mattel" });

            var mockContext = new Mock<ToysAndGamesContext>();
            mockContext.Setup(m => m.Companies).Returns(companiesMockSet.Object);

            CompanyService companyService = new CompanyService(mockContext.Object);

            //Act
            companyService.UpdateCompany(companyDTO.CompanyId, companyDTO);
            var result = mockContext.Object.Companies.FirstOrDefault(c => c.CompanyId == companyDTO.CompanyId);

            //Assert
            companiesMockSet.Verify(c => c.Update(It.IsAny<Company>()), Times.Never);
            mockContext.Verify(c => c.SaveChanges(), Times.Never);
            _outputHelper.WriteLine($"CompanyDTO with updated fields: {JsonConvert.SerializeObject(companyDTO)}");
            _outputHelper.WriteLine($"Company updated: {JsonConvert.SerializeObject(result)}");
        }

        [Trait("Company", "Context")]
        [Fact]
        public void AddCompany_GivenCompanyDTO_ReturnsIntId_UsingContext()
        {
            //Arrange
            var companyDTO = new CompanyDTO()
            {
                Name = "Matel"
            };

            var companiesMockSet = new Mock<DbSet<Company>>();

            var mockContext = new Mock<ToysAndGamesContext>();
           
            mockContext.Setup(m => m.Companies).Returns(companiesMockSet.Object);

            CompanyService companyService = new CompanyService(mockContext.Object);

            //Act
            var result = companyService.AddCompany(companyDTO);

            //Assert
            Assert.IsType<int>(result);
            companiesMockSet.Verify(c => c.Add(It.IsAny<Company>()), Times.Once);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);

            _outputHelper.WriteLine($"CompanyId added: {result}");
        }

        [Trait("Company", "Context")]
        [Fact]
        public void AddCompany_GivenNullObject_ReturnsIdMinus1_UsingContext()
        {
            //Arrange
            CompanyDTO companyDTO = null;
            var companiesMockSet = new Mock<DbSet<Company>>();

            var mockContext = new Mock<ToysAndGamesContext>();

            mockContext.Setup(m => m.Companies).Returns(companiesMockSet.Object);

            CompanyService companyService = new CompanyService(mockContext.Object);

            //Act
            var result = companyService.AddCompany(companyDTO);

            //Assert
            Assert.Equal(-1,result);
            companiesMockSet.Verify(c => c.Add(It.IsAny<Company>()), Times.Never);
            mockContext.Verify(c => c.SaveChanges(), Times.Never);

            _outputHelper.WriteLine($"CompanyId added: {result}");
        }

        [Trait("Company", "Context")]
        [Fact]
        public void DeleteCompany_RemovesCompany_UsingContext()
        {
            //Arrange
            var companyId = 1;

            var companiesMockSet = DbSetMockUtility.GetQueryableMock(new Company { CompanyId = 1, Name = "Mattel" },
                new Company { CompanyId = 2, Name = "Hasbro" });

            var mockContext = new Mock<ToysAndGamesContext>();
            mockContext.Setup(m => m.Companies).Returns(companiesMockSet.Object);

            CompanyService companyService = new CompanyService(mockContext.Object);

            //Act
            companyService.DeleteCompany(companyId);

            //Assert
            companiesMockSet.Verify(c => c.Remove(It.IsAny<Company>()), Times.Once);
            mockContext.Verify(c => c.SaveChanges(), Times.Once);

            _outputHelper.WriteLine($"CompanyId deleted: {companyId}");
        }

        [Trait("Company", "Context")]
        [Fact]
        public void DeleteCompany_GivenNoExistingCompany_DoesNotExecuteContextMethods_UsingContext()
        {
            //Arrange
            var companyId = 10;

            var companiesMockSet = DbSetMockUtility.GetQueryableMock(new Company { CompanyId = 1, Name = "Mattel" },
                new Company { CompanyId = 2, Name = "Hasbro" });

            var mockContext = new Mock<ToysAndGamesContext>();
            mockContext.Setup(m => m.Companies).Returns(companiesMockSet.Object);

            CompanyService companyService = new CompanyService(mockContext.Object);

            //Act
            companyService.DeleteCompany(companyId);

            //Assert
            companiesMockSet.Verify(c => c.Remove(It.IsAny<Company>()), Times.Never);
            mockContext.Verify(c => c.SaveChanges(), Times.Never);

            _outputHelper.WriteLine($"CompanyId deleted: {companyId}");
        }
    }
}
