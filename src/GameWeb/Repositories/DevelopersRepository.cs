using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameWeb.Models;
using GameWeb.Models.DTO;
using GameWeb.Models.Entities;

namespace GameWeb.Repositories
{
    public class DevelopersRepository : IDevelopersRepository
    {
        private readonly Context _context;
        public DevelopersRepository(Context context)
        {
            _context = context;
        }

        public async Task<long> AddDeveloper(DeveloperDTO developer)
        {
            var developerEntity = new Developer()
            {
                Name = developer.Name,
                EstablishmentYear = developer.EstablishmentYear
            };
            var result = await _context.AddAsync<Developer>(developerEntity);
            await _context.SaveChangesAsync();
            return result.Entity.ID;
        }

        public async Task<Developer> DeleteDeveloper(long id)
        {
            var result = _context.Developers.Remove(await GetDeveloperById(id));
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Developer> GetDeveloperById(long id)
        {
            var result = await _context.Developers.FindAsync(id);
            if(result == null)
            {
                throw new System.Exception("Not Found!");
            }
            return result;
        }

        public List<Developer> GetDevelopers(int page, int limit)
        {
            return _context.Developers.ToList()
                .OrderBy(x => x.Name)
                .Skip(page * limit)
                .Take(limit)
                .ToList();
        }

        public async Task<Developer> UpdateDeveloper(long id, DeveloperDTO developer)
        {
            var updatedDeveloper = await GetDeveloperById(id);
            updatedDeveloper.Name = developer.Name;
            updatedDeveloper.EstablishmentYear = developer.EstablishmentYear;
            var result = _context.Developers.Update(updatedDeveloper);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}