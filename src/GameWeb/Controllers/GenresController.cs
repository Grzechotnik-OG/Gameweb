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