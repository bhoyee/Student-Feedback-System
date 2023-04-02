using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class PetitionRepository : IPetitionRepository
    {
        public readonly DataContext _context;
        public readonly IMapper _mapper;

        public PetitionRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }


        public async Task<Petition> GetPetitionByIdAsync(int id)
        {
            return await _context.Petitions.FindAsync(id);
        }

        public async Task<Petition> GetPetitionsByStatusAsync(string status)
        {
               return await _context.Petitions
                .Include(s => s.Status)
                .SingleOrDefaultAsync(x => x.Status == status); 
        }

        public async Task<PagedList<PetitionDto>> GetPetitionsAsync(UserParams userParams)
        {
              var query = _context.Petitions
                .ProjectTo<PetitionDto>(_mapper.ConfigurationProvider)
                .AsNoTracking();
            return await PagedList<PetitionDto>.CreateAsync(query, userParams.pageNumber, userParams.PageSize);
        }

        public async Task<PetitionDto> GetPetitionAsync(string status)
        {
              return await _context.Petitions
                .Where(x => x.Status == status)
                .ProjectTo<PetitionDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Petition>> GetPetitionsAsync()
        {
           return await _context.Petitions.ToListAsync();

           
                
        }

        public async Task<bool> SaveAllAsync()
        {
           return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Petition petition)
        {
            _context.Entry(petition).State = EntityState.Modified;
        }

        public async Task<Petition> AddPetition(Petition petition)
        {
            var result = await _context.Petitions.AddAsync(petition);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

    }
}