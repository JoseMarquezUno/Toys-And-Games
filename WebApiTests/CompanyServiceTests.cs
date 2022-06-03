
namespace WebApiTests
{
    public class CompanyServiceTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public CompanyServiceTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }
        [Fact]
        public void GetCompanies_ReturnsCompanies()
        {
            //Arrange
            var companiesMock = new Mock<ICompanyService>();
                companiesMock.Setup(c=>c.GetCompanies())
                .Returns(new List<CompanyDTO>() { new CompanyDTO(),
                new CompanyDTO()});

            //Act
            var result = companiesMock.Object.GetCompanies();

            //Assert
            Assert.NotEmpty(result);
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GetCompanies_ReturnsEmptyList()
        {
            //Arrange
            var companiesMock = new Mock<ICompanyService>();
            companiesMock
                .Setup(c=>c.GetCompanies())
                .Returns(new List<CompanyDTO>());

            //Act
            var result = companiesMock.Object.GetCompanies();

            //Assert
            Assert.Empty(result);
            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Theory]
        [GetCompanyByIdData]
        public void GetCompanyById_ReturnsCompany_IfCompanyExists(int companyId, bool expected)
        {
            //Arrange
            var companiesMock = new Mock<ICompanyService>();
            companiesMock
                .Setup(c=>c.GetCompanyById(It.IsInRange<int>(1,3,Moq.Range.Inclusive)))
                .Returns(new CompanyDTO());

            //Act
            var result = companiesMock.Object.GetCompanyById(companyId);

            //Assert
            Assert.Equal(expected, result == null);

            _outputHelper.WriteLine(JsonConvert.SerializeObject(result));
        }

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
    }
}
