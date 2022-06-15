using Microsoft.EntityFrameworkCore;
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
            //TODO: As a recomendation start using the new "is/isnot" conventions which is more readable
            if (company is not null)
            {
                _context.Companies.Remove(company);
                _context.SaveChanges();
            }
        }

        public async Task<IList<CompanyDTO>> GetCompanies()
        {
            //TODO: Use automapper instead of the manual conversion
            //Kudos for the empty new() declaration! :) 
            var companies = await _context.Companies.ToListAsync();
            List<CompanyDTO> companiesDtos = new();
            companies.ForEach(c => companiesDtos.Add(new CompanyDTO { CompanyId = c.CompanyId, Name = c.Name }));
            return companiesDtos;
        }

        public CompanyDTO? GetCompanyById(int id)
        {
            try
            {
                //TODO: How are you handling exceptions?
                Company company = _context.Companies.First(c => c.CompanyId == id);
                CompanyDTO companyDTO = new() 
                {
                    Name= company.Name,
                    CompanyId = company.CompanyId
                };
                return companyDTO;
            }
            catch (Exception)
            {
                //Log Error
                return null;
            }
        }
        

        public void UpdateCompany(int id, CompanyDTO companyDTO)
        {
            //TODO: This is not wrong, but usually people use var when getting Queryes, because you could have an IQueryable
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
