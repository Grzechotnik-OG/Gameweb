using System.Collections.Generic;
using System.Threading.Tasks;
using GameWeb.Models.Entities;

namespace GameWeb.Repositories
{
    public interface IGameGenresRepository
    {
        Task<long> AddGenre(string name);
        List<GameGenre> GetGenres(int page, int limit);
        Task<GameGenre> GetGenreById(long id);
        Task<GameGenre> UpdateGenre(long id, string name);
        Task<GameGenre> DeleteGenre(long id);
    }
}