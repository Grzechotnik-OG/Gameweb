using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameWeb.Models;
using GameWeb.Models.DTO;
using GameWeb.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameWeb.Repositories
{
    public class GamesRepository : IGamesRepository
    {
        private readonly Context _context;
        private readonly IDevelopersRepository _developersRepository;
        private readonly IGameGenresRepository _genresRepository;
        public GamesRepository(Context context, IDevelopersRepository developersRepository, IGameGenresRepository genresRepository)
        {
            _context = context;
            _genresRepository = genresRepository;
            _developersRepository = developersRepository;
        }

        public async Task<long> AddGame(GameDTO gameDTO)
        {
            var developer = await _developersRepository.GetDeveloperById(gameDTO.DeveloperId);
            var genre = await _genresRepository.GetGenreById(gameDTO.GenreId);
            Game game = new Game()
            {
                Name = gameDTO.Name,
                ReleaseDate = gameDTO.ReleaseDate,
                Developer = developer,
                Genre = genre
            };
            var result = await _context.AddAsync<Game>(game);
            await _context.SaveChangesAsync();
            return result.Entity.GameId;
        }

        public async Task<Game> GetGameById(long id)
        {
            var result = await _context.Games.Include(x => x.Developer)
                        .Include(x => x.Genre)
                        .Where(x => x.GameId == id).FirstOrDefaultAsync();
            if(result == null)
            {
                throw new System.Exception("Not Found!");
            }
            return result;
        }

        public async Task<List<Game>> GetGameByGenreId(long genreId)
        {
            var genre = await _genresRepository.GetGenreById(genreId);
            var result = await _context.Games.Include(x => x.Developer)
                        .Include(x => x.Genre)
                        .Where(x => x.Genre.Equals(genre)).ToListAsync();
            return result;
        }

        public async Task<Game> DeleteGame(long id)
        {
            var result = _context.Games.Remove(await GetGameById(id));
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Game> UpdateGame(long id, GameDTO gameDTO)
        {
            var developer = await _developersRepository.GetDeveloperById(gameDTO.DeveloperId);
            var genre = await _genresRepository.GetGenreById(gameDTO.GenreId);
            Game game = new Game()
            {
                GameId = id,
                Name = gameDTO.Name,
                ReleaseDate = gameDTO.ReleaseDate,
                Developer = developer,
                Genre = genre
            };
            var result = _context.Games.Update(game);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Review> GetReviewById(long id)
        {
            var result = await _context.Reviews.Include(r => r.User).Where(r => r.ReviewId == id).FirstOrDefaultAsync();
            if(result == null)
            {
                throw new System.Exception("Not Found!");
            }
            return result;
        }
        public async Task<List<Review>> GetReviewsByGameId(long gameId, int page, int limit)
        {
            var result = await _context.Games.FindAsync(gameId);
            if(result == null)
            {
                throw new System.Exception("Not Found!");
            }
            var reviews = _context.Reviews
                .Where(r => r.GameId == gameId)
                .OrderByDescending(x => x.CreationDate)
                .Skip(page * limit)
                .Include(r => r.User)
                .Take(limit)
                .ToList();
            return reviews;
        }
        public async Task<long> AddReview(ReviewDTO reviewDTO, long gameId, long userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if(user == null)
            {
                throw new System.Exception("User not Found!");
            }
            Review review = new Review()
            {
                User = user,
                Game = await GetGameById(gameId),
                Description = reviewDTO.Description,
                Title = reviewDTO.Title,
                Rating = reviewDTO.Rating,
                CreationDate = DateTime.Now
            };

            var result = await _context.AddAsync<Review>(review);
            await _context.SaveChangesAsync();
            return result.Entity.ReviewId;
        }
		public async Task<Review> UpdateReviewById(ReviewUpdateDTO updatedReview, long id, long userId)
        {
            var review = await GetReviewById(id);
            if(userId != review.User.UserId)
            {
                throw new MethodAccessException("Method not allowed");
            }
            review.Title = updatedReview.Title;
            review.Description = updatedReview.Description;
            review.Rating = updatedReview.Rating;
            var result = _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
		public async Task<Review> DeleteReviewById(long id, long userId)
        {
            var review = await GetReviewById(id);
            if(userId != review.User.UserId)
            {
                throw new MethodAccessException("Method not allowed");
            }
            var result = _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}