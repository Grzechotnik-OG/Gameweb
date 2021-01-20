using System;
using Microsoft.AspNetCore.Mvc;
using GameWeb.Models;
using GameWeb.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using GameWeb.Models.DTO;

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

		///<summary>
		///Returns game by Id
		///</summary>
		///<param name ="id">Id of a game</param>
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

		///<summary>
		///Returns list of games by genre type
		///</summary>
		///<param name ="genreId">Id of a genre</param>
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

		///<summary>
		///Adds new game
		///</summary>
		///<param name ="game">New game info</param>
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

		///<summary>
		///Updates existing game
		///</summary>
		///<param name ="id">Id of a game</param>
		///<param name ="game">Updated game info</param>
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

		///<summary>
		///Deletes existing game
		///</summary>
		///<param name ="id">Id of a game</param>
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
