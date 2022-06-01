using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.Models.DTO;

namespace Services.Contracts
{
    public interface ICompanyService
    {
        IList<CompanyDTO> GetCompanies();
        int AddCompany(CompanyDTO companyDTO);
        void UpdateCompany(int id, CompanyDTO companyDTO);
        void DeleteCompany(int id);
        bool CompanyExists(int id);
    }
}
