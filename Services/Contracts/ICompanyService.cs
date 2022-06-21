using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.DTO;

namespace ToysAndGames.Services.Contracts
{
    public interface ICompanyService
    {
        Task<IList<CompanyDTO>> GetCompanies();
        Task AddCompany(CompanyDTO companyDTO);
        Task UpdateCompany(int id, CompanyDTO companyDTO);
        Task DeleteCompany(int id);
        Task<bool> CompanyExists(int id);
        Task<CompanyDTO?> GetCompanyById(int id);
    }
}
