using Microsoft.AspNetCore.Mvc;
using ToysAndGames.Models.DTO;
using ToysAndGames.Services.Contracts;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }
        [HttpGet]
        public IList<CompanyDTO> GetCompanies()
        {
           return _companyService.GetCompanies();
        }
        [HttpGet]
        [Route("Company/{id}")]
        public IActionResult GetCompany(int id)
        {
            return Ok(_companyService.GetCompanyById(id));
        }
        [HttpPost]
        [Route("Company")]
        public IActionResult AddCompany(CompanyDTO company)
        {
            int id = _companyService.AddCompany(company);
            return CreatedAtAction(nameof(GetCompany),
                new { id = id },
                company);
        }
        [HttpPut]
        [Route("Company/{id}")]
        public IActionResult UpdateCompany(int id, CompanyDTO company)
        {
            if (_companyService.CompanyExists(id))
            {
                _companyService.UpdateCompany(id, company);
            }
            else
            {
                return NotFound();
            }
            return Ok(company);
        }

        [HttpDelete]
        [Route("Company/{id}")]
        public IActionResult DeleteCompany(int id)
        {
            if (_companyService.CompanyExists(id))
            {
                _companyService.DeleteCompany(id);
            }
            else
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
