using System.Threading.Tasks;
using GameWeb.Models;

namespace GameWeb.Repositories
{
    public interface IGamesRepository
    {
        Task<long> AddGame(Game game);
        Task<Game> GetGameById(long id);
        Task<Game> DeleteGame(long id);

        Task<Game> UpdateGame(long id,Game game);
    }
}