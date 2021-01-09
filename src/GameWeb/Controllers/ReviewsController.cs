using System;
using Microsoft.AspNetCore.Mvc;
using GameWeb.Models;
using GameWeb.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GameWeb.Controllers {
	[Route("api/v1")]
    [ApiController]
	public class ReviewsController : ControllerBase
	{
		private readonly Context _context;
		private readonly IGamesRepository _gamesRepository;

		public ReviewsController(Context context, IGamesRepository gamesRepository)
        {
            _context = context;
			_gamesRepository = gamesRepository;
        }

		[HttpPost("games/{gameId}/reviews")]
        [Authorize]
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
		public async Task<IActionResult> GetReviewsByGameId(long id)
        {
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
		public async Task<IActionResult> GetReviewById(long id)
        {
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

		[HttpPut("games/{gameId}/reviews/{reviewId}")] //sprawdzenie uzytkownika
        [Authorize]
		public async Task<IActionResult> UpdateReviewById(long reviewId, ReviewUpdateDTO review)
		{
			try{
				return Ok(await _gamesRepository.UpdateReviewById(review, reviewId));
			}
			catch(Exception e){
				return NotFound(e.Message);
			}
		}

		[HttpDelete("games/{gameId}/reviews/{reviewId}")] //sprawdzenie uzytkownika
        [Authorize]
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
	}
}
