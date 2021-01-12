using System.Collections.Generic;
using System.Threading.Tasks;
using GameWeb.Models.DTO;
using GameWeb.Models.Entities;

namespace GameWeb.Repositories
{
    public interface IGamesRepository
    {
        Task<long> AddGame(GameDTO game);
        Task<Game> GetGameById(long id);
        Task<List<Game>> GetGameByGenreId(long genreId);
        Task<Game> DeleteGame(long id);
        Task<Game> UpdateGame(long id, GameDTO game);
        Task<Review> GetReviewById(long id);
        Task<List<Review>> GetReviewsByGameId(long gameId, int page, int limit);
        Task<long> AddReview(ReviewDTO review, long gameId, long userId);
		Task<Review> UpdateReviewById(ReviewUpdateDTO updatedReview, long id, long userId);
		Task<Review> DeleteReviewById(long id, long userId);
    }
}