using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.DataAccess;
using ToysAndGames.Models;
using ToysAndGames.Models.DTO;

namespace Services.Services
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
            Company company = new Company()
            {
                Name = companyDTO.Name
            };
            _context.Companies.Add(company);
            _context.SaveChanges();
            return company.CompanyId;
        }

        public bool CompanyExists(int id)
        {
            var company = _context.Companies.FirstOrDefault(c => c.CompanyId == id);
            return company != null;
        }

        public void DeleteCompany(int id)
        {
            Company company = _context.Companies.FirstOrDefault(c => c.CompanyId == id);
            if (company!=null)
            {
                _context.Companies.Remove(company);
                _context.SaveChanges(); 
            }
        }

        public IList<CompanyDTO> GetCompanies()
        {
            List<CompanyDTO> companies = new();
            _context.Companies.ToList().ForEach(c=>companies.Add(new CompanyDTO { CompanyId = c.CompanyId, Name = c.Name}));
            return companies;
        }

        public void UpdateCompany(int id, CompanyDTO companyDTO)
        {
            Company company = _context.Companies.FirstOrDefault(c => c.CompanyId == id);
            if (company != null)
            {
                company.Name = companyDTO.Name;

                _context.Update(company);
                _context.SaveChanges();
            }
        }
    }
}
