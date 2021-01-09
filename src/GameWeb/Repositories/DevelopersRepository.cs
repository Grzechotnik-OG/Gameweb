using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameWeb.Models;

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
                EstablishmentDate = developer.EstablishmentDate
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

        public List<Developer> GetDevelopers()
        {
            return _context.Developers.ToList();
        }

        public async Task<Developer> UpdateDeveloper(long id, DeveloperDTO developer)
        {
            var updatedDeveloper = await GetDeveloperById(id);
            updatedDeveloper.Name = developer.Name;
            updatedDeveloper.EstablishmentDate = developer.EstablishmentDate;
            var result = _context.Developers.Update(updatedDeveloper);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}