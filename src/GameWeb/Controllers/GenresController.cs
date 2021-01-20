using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GameWeb.Models;
using GameWeb.Repositories;
using System;
using Microsoft.AspNetCore.Authorization;

namespace GameWeb.Controllers
{
	[Route("api/v1")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly Context _context;
		private readonly IGameGenresRepository _gameGenresRepository;

		public GenresController(Context context, IGameGenresRepository gameGenresRepository)
        {
            _context = context;
			_gameGenresRepository = gameGenresRepository;
        }

		///<summary>
		///Return all available genres
		///</summary>
		///<param name ="page">Nr of page</param>
		///<param name ="limit">Limit of objects for one page</param>
        [HttpGet("genres")]
		public IActionResult GetGameGenres(int page = 0, int limit = 10)
        {
			try
			{
				var result = _gameGenresRepository.GetGenres(page, limit);
				return Ok(result);
			}
			catch(Exception e)
			{
				return NotFound(e.Message);
			}
		}

		///<summary>
		///Returns one existing genre by Id
		///</summary>
		///<param name ="id">Id of a genre</param>
        [HttpGet("genres/{id}")]
		public async Task<IActionResult> GetGenre(long id)
		{
			try
			{
				var result = await _gameGenresRepository.GetGenreById(id);
				return Ok(result);
			}
			catch(Exception e)
			{
				return NotFound(e.Message);
			}
		}

		///<summary>
		///Updates existing genre by Id
		///</summary>
		///<param name ="id">Id of a review</param>
		///<param name ="name">New genre name</param>
        [HttpPut("genres/{id}")]
        [Authorize(Policy = Policies.Admin)]
		public async Task<IActionResult> UpdateGenreName(long id, string name)
        {
			try
            {
				return Ok(await _gameGenresRepository.UpdateGenre(id, name));
			}
			catch(Exception e)
            {
				return NotFound(e.Message);
			}
		}

		///<summary>
		///Adds new genre
		///</summary>
		///<param name ="name">Name of a new genre</param>
        [HttpPost("genres")]
        [Authorize(Policy = Policies.Admin)]
		public async Task<IActionResult> AddGenre(string name)
		{
			try
			{
				return Ok(await _gameGenresRepository.AddGenre(name));
			}
			catch(Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		///<summary>
		///Deletes genre by Id
		///</summary>
		///<param name ="id">Id of a genre</param>
        [HttpDelete("genres/{id}")]
        [Authorize(Policy = Policies.Admin)]
		public async Task<IActionResult> DeleteGenre(long id)
		{
			try
			{
				await _gameGenresRepository.DeleteGenre(id);
				return NoContent();
			}
			catch(Exception e){
				return NotFound(e.Message);
			}
		}
    }
}