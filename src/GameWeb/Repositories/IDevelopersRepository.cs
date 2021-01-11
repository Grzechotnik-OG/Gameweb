using System.Collections.Generic;
using System.Threading.Tasks;
using GameWeb.Models;

namespace GameWeb.Repositories
{
    public interface IDevelopersRepository
    {
        Task<long> AddDeveloper(DeveloperDTO developer);
        List<Developer> GetDevelopers(int page, int limit);
        Task<Developer> GetDeveloperById(long id);
        Task<Developer> UpdateDeveloper(long id, DeveloperDTO developer);
        Task<Developer> DeleteDeveloper(long id);
    }
}