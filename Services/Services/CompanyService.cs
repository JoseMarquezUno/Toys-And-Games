using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToysAndGames.DataAccess;
using ToysAndGames.Models;
using ToysAndGames.Services.Contracts;
using WebAPI.DTO;

namespace ToysAndGames.Services.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ToysAndGamesContext _context;
        private readonly IMapper _mapper;
        public CompanyService(ToysAndGamesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddCompany(CompanyDTO companyDTO)
        {
            try
            {
                if (companyDTO is not null)
                {
                    Company company = _mapper.Map<Company>(companyDTO);
                    //    new ()
                    //{
                    //    Name = companyDTO.Name
                    //};
                    await _context.Companies.AddAsync(company);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CompanyExists(int id)
        {
            try
            {
                var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
                return company is not null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task DeleteCompany(int id)
        {
            try
            {
                var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
                //TODO: As a recomendation start using the new "is/isnot" conventions which is more readable
                if (company is not null)
                {
                    _context.Companies.Remove(company);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IList<CompanyDTO>> GetCompanies()
        {
            //TODO: Use automapper instead of the manual conversion
            //Kudos for the empty new() declaration! :) 
            List<CompanyDTO> companiesDtos = new();
            try
            {
                var companies = await _context.Companies.ToListAsync();
                companies.ForEach(c => companiesDtos.Add(_mapper.Map<CompanyDTO>(c)
                    //new () { Id = c.Id, Name = c.Name }
                    ));
                return companiesDtos;
            }
            catch (Exception)
            {
                return companiesDtos;
            }
        }

        public async Task<CompanyDTO?> GetCompanyById(int id)
        {
            try
            {
                //TODO: How are you handling exceptions?
                var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
                CompanyDTO companyDTO = _mapper.Map<CompanyDTO?>(company);
                //    new()
                //{
                //    Name = company.Name,
                //    Id = company.Id
                //};
                return companyDTO;
            }
            catch (Exception)
            {
                //Log Error
                return null;
            }
        }


        public async Task UpdateCompany(int id, CompanyDTO companyDTO)
        {
            //TODO: This is not wrong, but usually people use var when getting Queryes, because you could have an IQueryable
            try
            {
                var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
                if (company is not null)
                {
                    company.Name = companyDTO.Name;

                    _context.Companies.Update(company);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
