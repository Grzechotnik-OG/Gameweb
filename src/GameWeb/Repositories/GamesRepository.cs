using System.Linq;
using System.Threading.Tasks;
using GameWeb.Models;

namespace GameWeb.Repositories
{
    public class GamesRepository : IGamesRepository
    {
        private readonly Context _context;
        public GamesRepository(Context context)
        {
            _context = context;
        }

        public async Task<long> AddGame(Game game)
        {
            var result = await _context.AddAsync<Game>(game);
            await _context.SaveChangesAsync();
            return result.Entity.ID;
        }

        public async Task<Game> GetGameById(long id)
        {
            var result = await _context.Games.FindAsync(id);
            if(result == null)
            {
                throw new System.Exception("Not Found!");
            }
            return result;
        }
    }
}