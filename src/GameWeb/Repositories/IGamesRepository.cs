using System.Collections.Generic;
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
        Task<Review> GetReviewById(long id);
        Task<List<Review>> GetReviewsByGameId(long gameId);
        Task<long> AddReview(Review review, long gameId, long userId);
		Task<Review> UpdateReviewById(ReviewUpdateDTO updatedReview, long id);
		Task<Review> DeleteReviewById(long id);
    }
}