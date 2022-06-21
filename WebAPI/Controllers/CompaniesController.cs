using Microsoft.AspNetCore.Mvc;
using ToysAndGames.Services.Contracts;
using WebAPI.DTO;

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
        public async Task<ActionResult<IList<CompanyDTO>>> GetCompanies()
        {
            //TODO: as a good practice use non-blocking/waiting code blocks in other words async/await calls to the controllers/services
            return Ok(await _companyService.GetCompanies());
        }
        [HttpGet]
        [Route("Company/{id}")]
        public async Task<ActionResult<CompanyDTO?>> GetCompany(int id)
        {
            return Ok(await _companyService.GetCompanyById(id));
        }
        [HttpPost]
        [Route("Company")]
        public async Task<ActionResult> AddCompany(CompanyDTO company)
        {
            try
            {
                await _companyService.AddCompany(company);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut]
        [Route("Company/{id}")]
        public async Task<ActionResult<CompanyDTO>> UpdateCompany(int id, CompanyDTO company)
        {
            try
            {
                if (await _companyService.CompanyExists(id))
                {
                    await _companyService.UpdateCompany(id, company);
                    return Ok(company);
                }
                    return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [Route("Company/{id}")]
        public async Task<ActionResult> DeleteCompany(int id)
        {
            try
            {
                if (await _companyService.CompanyExists(id))
                {
                    await _companyService.DeleteCompany(id);
                    return NoContent();
                }
                    return NotFound($"Company with Id: {id} was not found in database");

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
