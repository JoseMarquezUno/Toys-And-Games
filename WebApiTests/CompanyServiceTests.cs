
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using System.Data.Entity.Infrastructure;
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

        //TODO: Lalo changed the signature i need to change the test
        //[Fact]
        //public void GetCompanies_ReturnsCompanies()
        //{
        //    //Arrange
        //    var companiesMock = new Mock<ICompanyService>();
        //    companiesMock.Setup(c => c.GetCompanies())
        //    .Returns(new List<CompanyDTO>() { new CompanyDTO(),
        //        new CompanyDTO()});

        //    //Act
        //    var result = companiesMock.Object.GetCompanies();

        //    //Assert
        //    Assert.NotEmpty(result);
        //    _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        //}

        //TODO: //TODO: Lalo changed the signature i need to change the test
        //[Trait("Company", "Interface")]
        //[Fact]
        //public void GetCompanies_ReturnsEmptyList()
        //{
        //    //Arrange
        //    var companiesMock = new Mock<ICompanyService>();
        //    companiesMock
        //        .Setup(c => c.GetCompanies())
        //        .Returns(new List<CompanyDTO>());

        //    //Act
        //    var result = companiesMock.Object.GetCompanies();

        //    //Assert
        //    Assert.Empty(result);
        //    _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        //}

        [Trait("Company", "Interface")]
        [Theory]
        [GetCompanyByIdData]
        public async void GetCompanyById_ReturnsCompany_IfCompanyExists(int companyId, bool expected)
        {
            //Arrange
            var companiesMock = new Mock<ICompanyService>();
            companiesMock
                .Setup(c => c.GetCompanyById(It.IsInRange<int>(1, 3, Moq.Range.Inclusive)))
                .ReturnsAsync(new CompanyDTO { Id = companyId});

            //Act
            var result = await companiesMock.Object.GetCompanyById(companyId);

            //Assert
            Assert.Equal(expected, result == null);

            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Trait("Company", "Interface")]
        [Fact]
        public async void AddCompany_AddsCompanyInDB()
        {
            //Arrange
            CompanyDTO companyDTO = new CompanyDTO()
            {
                Id = 1,
                Name = "company"
            };

            var companiesMock = new Mock<ICompanyService>();
            companiesMock
                .Setup(c => c.AddCompany(It.IsAny<CompanyDTO>()))
                .Returns(Task.CompletedTask);

            //Act
            await companiesMock.Object.AddCompany(companyDTO);

            //Assert
            companiesMock.Verify(p=>p.AddCompany(It.IsAny<CompanyDTO>()),Times.Once);
            _outputHelper.WriteLine("The method AddCompany executed once");
        }

        [Trait("Company", "Interface")]
        [Theory]
        [CompanyExistsData]
        public async void CompanyExists_ReturnsTrueIfCompanyExists(int companyId, bool expected)
        {
            //Arrange
            var companiesMock = new Mock<ICompanyService>();
            companiesMock
                .Setup(c => c.CompanyExists(It.IsInRange<int>(1, 3, Moq.Range.Inclusive)))
                .ReturnsAsync(true);

            //Act
            var result = await companiesMock.Object.CompanyExists(companyId);

            //Assert
            Assert.Equal(expected, result);
            _outputHelper.WriteLine(result.ToString());
        }

        //TODO: Make async operations work in mock
        [Trait("Company","Context")]
        [Fact]
        public async void UpdateCompany_GivenExistingCompany_UpdatesCompany_UsingContext()
        {
            //Arrange
            var companyDTO = new CompanyDTO()
            {
                Id = 1,
                Name = "Matel"
            };

            var companies = new List<Company> { new Company { Id = 1, Name = "Mattel" } };

            var companiesMockSet = companies.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<ToysAndGamesContext>();
            mockContext.Setup(m => m.Companies).Returns(companiesMockSet.Object);

            var mapperMock = new Mock<IMapper>();

            _outputHelper.WriteLine($"Company in mock context: {JsonConvert.SerializeObject(companiesMockSet.Object.First())}");

            CompanyService companyService = new (mockContext.Object,mapperMock.Object);

            //Act
            await companyService.UpdateCompany(companyDTO.Id, companyDTO);
            var result = mockContext.Object.Companies.FirstOrDefault(c => c.Id == companyDTO.Id);

            //Assert
            Assert.Equal(companyDTO.Name, result.Name);
            companiesMockSet.Verify(c => c.Update(It.IsAny<Company>()), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(CancellationToken.None), Times.Once);
            _outputHelper.WriteLine($"CompanyDTO with updated fields: {JsonConvert.SerializeObject(companyDTO)}");
            _outputHelper.WriteLine($"Company updated in context: {JsonConvert.SerializeObject(result)}");
        }

        //TODO: Make async operations work in mock
        [Trait("Company", "Context")]
        [Fact]
        public async void UpdateCompany_GivenNonExistingCompany_DoesNotExecuteContextMethods_UsingContext()
        {
            //Arrange
            var companyDTO = new CompanyDTO()
            {
                Id = 10,
                Name = "Matel"
            };

            var companies = new List<Company> { new Company { Id = 1, Name = "Mattel" } };

            var companiesMockSet = companies.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<ToysAndGamesContext>();
            mockContext.Setup(m => m.Companies).Returns(companiesMockSet.Object);

            var mapperMock = new Mock<IMapper>();

            CompanyService companyService = new (mockContext.Object,mapperMock.Object);

            //Act
            await companyService.UpdateCompany(companyDTO.Id, companyDTO);
            var result = mockContext.Object.Companies.FirstOrDefault(c => c.Id == companyDTO.Id);

            //Assert
            companiesMockSet.Verify(c => c.Update(It.IsAny<Company>()), Times.Never);
            mockContext.Verify(c => c.SaveChangesAsync(CancellationToken.None), Times.Never);
            _outputHelper.WriteLine($"CompanyDTO with updated fields: {JsonConvert.SerializeObject(companyDTO)}");
            _outputHelper.WriteLine($"Company updated: {JsonConvert.SerializeObject(result)}");
        }

        [Trait("Company", "Context")]
        [Fact]
        public async void AddCompany_GivenCompanyDTO_AddsCompanyToDB_UsingContext()
        {
            //Arrange
            var companyDTO = new CompanyDTO()
            {
                Name = "Matel"
            };

            var companiesMockSet = new Mock<DbSet<Company>>();

            var mockContext = new Mock<ToysAndGamesContext>();

            var mapperMock = new Mock<IMapper>();
           
            mockContext.Setup(m => m.Companies).Returns(companiesMockSet.Object);

            CompanyService companyService = new (mockContext.Object,mapperMock.Object);

            //Act
            await companyService.AddCompany(companyDTO);

            //Assert
            companiesMockSet.Verify(c => c.AddAsync(It.IsAny<Company>(),CancellationToken.None), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(CancellationToken.None), Times.Once);

            _outputHelper.WriteLine("Methods Add and Savechanges executed once");
        }

        [Trait("Company", "Context")]
        [Fact]
        public async void AddCompany_GivenNullObject_DoesNotExecuteContextMethods_UsingContext()
        {
            //Arrange
            CompanyDTO companyDTO = null;
            var companiesMockSet = new Mock<DbSet<Company>>();

            var mockContext = new Mock<ToysAndGamesContext>();

            var mapperMock = new Mock<IMapper>();

            mockContext.Setup(m => m.Companies).Returns(companiesMockSet.Object);

            CompanyService companyService = new (mockContext.Object,mapperMock.Object);

            //Act
            await companyService.AddCompany(companyDTO);

            //Assert
            companiesMockSet.Verify(c => c.AddAsync(It.IsAny<Company>(),CancellationToken.None), Times.Never);
            mockContext.Verify(c => c.SaveChangesAsync(CancellationToken.None), Times.Never);

            _outputHelper.WriteLine("Db methods didn't got executed");
        }

        //TODO: Make async operations work in mock
        [Trait("Company", "Context")]
        [Fact]
        public async void DeleteCompany_RemovesCompany_UsingContext()
        {
            //Arrange
            var companyId = 1;

            var companies = new List<Company> { new Company { Id = 1, Name = "Mattel" },
                new Company { Id = 2, Name = "Hasbro" } };


            var companiesMockSet = companies.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<ToysAndGamesContext>();

            var mapperMock = new Mock<IMapper>();

            mockContext.Setup(m => m.Companies).Returns(companiesMockSet.Object);

            CompanyService companyService = new (mockContext.Object,mapperMock.Object);

            //Act
            await companyService.DeleteCompany(companyId);

            //Assert
            companiesMockSet.Verify(c => c.Remove(It.IsAny<Company>()), Times.Once);
            mockContext.Verify(c => c.SaveChangesAsync(CancellationToken.None), Times.Once);

            _outputHelper.WriteLine($"CompanyId deleted: {companyId}");
        }

        //TODO: Make async operations work in mock
        [Trait("Company", "Context")]
        [Fact]
        public async void DeleteCompany_GivenNoExistingCompany_DoesNotExecuteContextMethods_UsingContext()
        {
            //Arrange
            var companyId = 10;

            var companies = new List<Company> {new Company { Id = 1, Name = "Mattel" },
                new Company { Id = 2, Name = "Hasbro" } };

            var companiesMockSet = companies.AsQueryable().BuildMockDbSet();                      

            var mockContext = new Mock<ToysAndGamesContext>();

            var mapperMock = new Mock<IMapper>();

            //mockContext.Setup(m => m.Companies).Returns(companiesMockSet.Object);
            mockContext.Setup(m => m.Companies).Returns(companiesMockSet.Object);

            CompanyService companyService = new (mockContext.Object,mapperMock.Object);

            //Act
            await companyService.DeleteCompany(companyId);

            //Assert
            companiesMockSet.Verify(c => c.Remove(It.IsAny<Company>()), Times.Never);
            mockContext.Verify(c => c.SaveChangesAsync(CancellationToken.None), Times.Never);

            _outputHelper.WriteLine($"Attempted deletion of CompanyId: {companyId}");
        }
    }
}
