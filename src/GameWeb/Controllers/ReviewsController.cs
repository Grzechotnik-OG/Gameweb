using System;
using Microsoft.AspNetCore.Mvc;
using GameWeb.Models;
using GameWeb.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace GameWeb.Controllers {
	[Route("api/v1")]
    [ApiController]
	public class ReviewsController : ControllerBase
	{
		private readonly Context _context;
		private readonly IGamesRepository _gamesRepository;
		private readonly ILogger<ReviewsController> _logger;

		public ReviewsController(Context context, IGamesRepository gamesRepository, ILogger<ReviewsController> logger)
        {
            _context = context;
			_gamesRepository = gamesRepository;
			_logger = logger;
        }

		[HttpPost("games/{gameId}/reviews")]
        [Authorize]
		public async Task<IActionResult> AddReview(Review review, long gameId, long userId)
		{
			try
			{
				var result = await _gamesRepository.AddReview(review, gameId, userId);
				_logger.LogInformation("reached endpoint " + Request.Path);
				return Ok(result);
			}
			catch(Exception e)
			{
				_logger.LogInformation(e, "Bad Request");
				return BadRequest(e.Message);
			}
		}

		[HttpGet("games/{id}/reviews")]
		public async Task<IActionResult> GetReviewsByGameId(long id)
        {
			try
			{
				var result = await _gamesRepository.GetReviewsByGameId(id);
				_logger.LogError("reached endpoint " + Request.Path);
				return Ok(result);
			}
			catch(Exception e)
			{
				_logger.LogError(e, "Not Found request: " + Request.Path);
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
