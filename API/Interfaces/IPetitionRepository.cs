using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IPetitionRepository
    {
        void Update(Petition petition);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<Petition>> GetPetitionsAsync();
        Task<Petition> GetPetitionByIdAsync(int id);
        Task<Petition> GetPetitionsByStatusAsync(string status);
        Task<PagedList<PetitionDto>> GetPetitionsAsync(UserParams userParams);
        Task<PetitionDto> GetPetitionAsync(string status);

        Task<Petition> AddPetition(Petition petition);
    }
}