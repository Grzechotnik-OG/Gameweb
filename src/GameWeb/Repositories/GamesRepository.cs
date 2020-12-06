using System.Collections.Generic;
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
            return result.Entity.GameId;
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

        public async Task<Game> DeleteGame(long id)
        {
            var result = _context.Games.Remove(await GetGameById(id));
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Game> UpdateGame(long id, Game game)
        {
            var result = _context.Games.Update(game);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Review> GetReviewById(long id)
        {
            var result = await _context.Reviews.FindAsync(id);
            if(result == null)
            {
                throw new System.Exception("Not Found!");
            }
            return result;
        }
        public async Task<List<Review>> GetReviewsByGameId(long gameId)
        {
            var result = await _context.Games.FindAsync(gameId);
            if(result == null)
            {
                throw new System.Exception("Not Found!");
            }
            var reviews = _context.Reviews
                .Where(r => r.GameId == gameId)
                .ToList();
            return reviews;
        }
        public async Task<long> AddReview(Review review, long gameId, long userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if(user == null)
            {
                throw new System.Exception("User not Found!");
            }
            review.User = user;
            review.Game = await GetGameById(gameId);
            var result = await _context.AddAsync<Review>(review);
            await _context.SaveChangesAsync();
            return result.Entity.GameId;
        }
		public async Task<Review> UpdateReviewById(ReviewUpdateDTO updatedReview, long id)
        {
            var review = await GetReviewById(id);
            review.Title = updatedReview.Title;
            review.Description = updatedReview.Description;
            review.Rating = updatedReview.Rating;
            var result = _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
		public async Task<Review> DeleteReviewById(long id)
        {
            var result = _context.Reviews.Remove(await GetReviewById(id));
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}