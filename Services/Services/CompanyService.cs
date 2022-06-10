using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.DataAccess;
using ToysAndGames.Models;
using ToysAndGames.Models.DTO;
using ToysAndGames.Services.Contracts;

namespace ToysAndGames.Services.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ToysAndGamesContext _context;
        public CompanyService(ToysAndGamesContext context)
        {
            _context = context;
        }
        public int AddCompany(CompanyDTO companyDTO)
        {
            if (companyDTO != null)
            {
                Company company = new Company()
                {
                    Name = companyDTO.Name
                };
                _context.Companies.Add(company);
                _context.SaveChanges();
                return company.CompanyId; 
            }
            return -1;
        }

        public bool CompanyExists(int id)
        {
            var company = _context.Companies.FirstOrDefault(c => c.CompanyId == id);
            return company != null;
        }

        public void DeleteCompany(int id)
        {
            Company company = _context.Companies.FirstOrDefault(c => c.CompanyId == id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                _context.SaveChanges();
            }
        }

        public IList<CompanyDTO> GetCompanies()
        {
            List<CompanyDTO> companies = new();
            _context.Companies.ToList().ForEach(c => companies.Add(new CompanyDTO { CompanyId = c.CompanyId, Name = c.Name }));
            return companies;
        }

        public CompanyDTO GetCompanyById(int id)
        {
            Company company = _context.Companies.FirstOrDefault(c => c.CompanyId == id);
            CompanyDTO companyDTO = new();
            if (company != null)
            {
                companyDTO.Name = company.Name;
                companyDTO.CompanyId = company.CompanyId;
            }
            return companyDTO.Name==null?null:companyDTO;
        }

        public void UpdateCompany(int id, CompanyDTO companyDTO)
        {
            Company company = _context.Companies.FirstOrDefault(c => c.CompanyId == id);
            if (company != null)
            {
                company.Name = companyDTO.Name;

                _context.Companies.Update(company);
                _context.SaveChanges();
            }
        }
    }
}
