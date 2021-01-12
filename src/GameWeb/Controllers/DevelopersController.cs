using System;
using System.Threading.Tasks;
using GameWeb.Models;
using GameWeb.Models.DTO;
using GameWeb.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameWeb.Controllers
{
	[Route("api/v1")]
    [ApiController]
    public class DevelopersController: ControllerBase
    {
        private readonly Context _context;
		private readonly IDevelopersRepository _developersRepository;

		public DevelopersController(Context context, IDevelopersRepository developersRepository)
        {
            _context = context;
			_developersRepository = developersRepository;
        }

        [HttpGet("developers")]
		public IActionResult GetDevelopers(int page, int limit = 10)
        {
			try
			{
				var result = _developersRepository.GetDevelopers(page, limit);
				return Ok(result);
			}
			catch(Exception e)
			{
				return NotFound(e.Message);
			}
		}

        [HttpGet("developers/{id}")]
		public async Task<IActionResult> GetDeveloper(long id)
		{
			try
			{
				var result = await _developersRepository.GetDeveloperById(id);
				return Ok(result);
			}
			catch(Exception e)
			{
				return NotFound(e.Message);
			}
		}

        [HttpPut("developers/{id}")]
        [Authorize(Policy = Policies.Mod)]
		public async Task<IActionResult> UpdateDeveloperInfo(long id, [FromBody]DeveloperDTO developer)
        {
			try
            {
				return Ok(await _developersRepository.UpdateDeveloper(id, developer));
			}
			catch(Exception e)
            {
				return NotFound(e.Message);
			}
		}

        [HttpPost("developers")]
        [Authorize(Policy = Policies.Mod)]
		public async Task<IActionResult> AddDeveloper([FromBody]DeveloperDTO developer)
		{
			try
			{
				return Ok(await _developersRepository.AddDeveloper(developer));
				//return CreatedAtAction(
                //    nameof(GetDeveloper),
                //    result.);
			}
			catch(Exception e)
			{
				return BadRequest(e.Message);
			}
		}

        [HttpDelete("developers/{id}")]
        [Authorize(Policy = Policies.Mod)]
		public async Task<IActionResult> DeleteDeveloper(long id)
		{
			try
			{
				await _developersRepository.DeleteDeveloper(id);
				return NoContent();
			}
			catch(Exception e){
				return NotFound(e.Message);
			}
		}
    }
}