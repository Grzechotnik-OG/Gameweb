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
		public async Task<long> AddGame(Game game) 
		{
			return await _gamesRepository.AddGame(game);
		}

		[HttpPut("games/{id}")]
		public async Task<IActionResult> UpdateGameInfo(long id,Game game) {

			if(id != game.GameId){
				return BadRequest();
			}
			try{
				return Ok(await _gamesRepository.UpdateGame(id,game));
			}
			catch(Exception e){
				return NotFound(e.Message);
			}
		}
		[HttpDelete("games/{id}")]
		public async Task<IActionResult> DeleteGame(long id) {
			try{
				await _gamesRepository.DeleteGame(id);
				return NoContent();
			}
			catch(Exception e){
				return NotFound(e.Message);
			}
		}
		[HttpPost("games/{gameId}/reviews")]
		public async Task<IActionResult> AddReview(Review review, long gameId, long userId)
		{
			try
			{
				var result = await _gamesRepository.AddReview(review, gameId, userId);
				return Ok(result);
			}
			catch(Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpGet("games/{id}/reviews")]
		public async Task<IActionResult> GetReviewsByGameId(long id) {
			try
			{
				var result = await _gamesRepository.GetReviewsByGameId(id);
				return Ok(result);
			}
			catch(Exception e)
			{
				return NotFound(e.Message);
			}
		}

		[HttpGet("games/{gameId}/reviews/{id}")]
		public async Task<IActionResult> GetReviewById(long id) {
			try
			{
				var result = await _gamesRepository.GetReviewById(id);
				return Ok(result);
			}
			catch(Exception e)
			{
				return NotFound(e.Message);
			}
		}

		[HttpPut("games/{gameId}/reviews/{reviewId}")]
		public async Task<IActionResult> UpdateReviewById(long reviewId, ReviewUpdateDTO review) 
		{
			try{
				return Ok(await _gamesRepository.UpdateReviewById(review, reviewId));
			}
			catch(Exception e){
				return NotFound(e.Message);
			}
		}

		[HttpDelete("games/{gameId}/reviews/{reviewId}")]
		public async Task<IActionResult> DeleteReview(long reviewId) 
		{
			try
			{
				await _gamesRepository.DeleteReviewById(reviewId);
				return NoContent();
			}
			catch(Exception e){
				return NotFound(e.Message);
			}
		}
		
		public void AddRating() {
			throw new System.NotImplementedException("Not implemented");
		}
	}
}
