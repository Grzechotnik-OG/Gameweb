using System;
using Microsoft.AspNetCore.Mvc;
using GameWeb.Models;
using GameWeb.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace GameWeb.Controllers
{
	[Route("api/v1")]
    [ApiController]
	public class GamesController : ControllerBase
	{
		private readonly Context _context;
		private readonly IGamesRepository _gamesRepository;

		public GamesController(Context context, IGamesRepository gamesRepository)
        {
            _context = context;
			_gamesRepository = gamesRepository;
        }

		[HttpGet("games/{id}")]
		public async Task<IActionResult> GetGame(long id)
		{
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

		[HttpGet("games/genres/{genreId}")]
		public async Task<IActionResult> SearchByGenre(long genreId)
		{
			try
			{
				var result = await _gamesRepository.GetGameByGenreId(genreId);
				return Ok(result);
			}
			catch(Exception e)
			{
				return NotFound(e.Message);
			}
		}

		[HttpPost("games")]
		[Authorize(Policy = Policies.Mod)]
		public async Task<IActionResult> AddGame(GameDTO game)
		{
			try
			{
				return Ok(await _gamesRepository.AddGame(game));
			}
			catch(Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPut("games/{id}")]
		[Authorize(Policy = Policies.Mod)]
		public async Task<IActionResult> UpdateGameInfo(long id, GameDTO game)
		{
			try
			{
				return Ok(await _gamesRepository.UpdateGame(id, game));
			}
			catch(Exception e){
				return NotFound(e.Message);
			}
		}

		[HttpDelete("games/{id}")]
		[Authorize(Policy = Policies.Mod)]
		public async Task<IActionResult> DeleteGame(long id)
		{
			try
			{
				await _gamesRepository.DeleteGame(id);
				return NoContent();
			}
			catch(Exception e){
				return NotFound(e.Message);
			}
		}
	}
}
