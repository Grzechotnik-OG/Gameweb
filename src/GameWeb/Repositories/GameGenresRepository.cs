using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameWeb.Models;

namespace GameWeb.Repositories
{
    public class GameGenresRepository : IGameGenresRepository
    {
        private readonly Context _context;
        public GameGenresRepository(Context context)
        {
            _context = context;
        }

        public async Task<long> AddGenre(string name)
        {
            var genreEntity = new GameGenre()
            {
                Name = name
            };
            var result = await _context.AddAsync<GameGenre>(genreEntity);
            await _context.SaveChangesAsync();
            return result.Entity.GameGenreId;
        }

        public async Task<GameGenre> DeleteGenre(long id)
        {
            var result = _context.Genres.Remove(await GetGenreById(id));
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<GameGenre> GetGenreById(long id)
        {
            var result = await _context.Genres.FindAsync(id);
            if(result == null)
            {
                throw new System.Exception("Not Found!");
            }
            return result;
        }

        public List<GameGenre> GetGenres()
        {
            return _context.Genres.ToList();
        }

        public async Task<GameGenre> UpdateGenre(long id, string name)
        {
            var genre = await GetGenreById(id);
            genre.Name = name;
            var result = _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}