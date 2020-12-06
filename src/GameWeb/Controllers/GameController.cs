using System;
using Microsoft.AspNetCore.Mvc;
using GameWeb.Models;
using GameWeb.Repositories;
using System.Threading.Tasks;

namespace GameWeb.Controllers {
	[Route("api/v1")]
    [ApiController]
	public class GameController : ControllerBase{
		private readonly Context _context;
		private readonly IGamesRepository _gamesRepository;

		public GameController(Context context, IGamesRepository gamesRepository)
        {
            _context = context;
			_gamesRepository = gamesRepository;
        }
		[HttpGet("games/{id}")]
		public async Task<IActionResult> GetGame(long id) {
			try
			{
				var result = await _gamesRepository.GetGameById(id);
				return Ok(result);
			}
			catch(Exception e)
			{
				return NotFound(e.Message);
			}
		}
		public void SearchByGenre() {
			throw new System.NotImplementedException("Not implemented");
		}
		[HttpPost("games")]
		public async Task<long> AddGame(Game game) {
			return await _gamesRepository.AddGame(game);
		}
		public void AddReview() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void UpdateReview() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void DeleteReview() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void UpdateGameInfo() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void AddRating() {
			throw new System.NotImplementedException("Not implemented");
		}
	}
}
