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

		///<summary>
		///Returns list of all developers
		///</summary>
		///<param name ="page">Nr of page</param>
		///<param name ="limit">Limit of objects for one page</param>
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

		///<summary>
		///Returns one developer by Id
		///</summary>
		///<param name ="id">Id of a developer</param>
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

		///<summary>
		///Updates existing developer
		///</summary>
		///<param name ="id">Id of a developer</param>
		///<param name ="developer">New developer info</param>
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

		///<summary>
		///Adds new developer
		///</summary>
		///<param name ="developer">New developer</param>
        [HttpPost("developers")]
        [Authorize(Policy = Policies.Mod)]
		public async Task<IActionResult> AddDeveloper([FromBody]DeveloperDTO developer)
		{
			try
			{
				return Ok(await _developersRepository.AddDeveloper(developer));
			}
			catch(Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		///<summary>
		///Deletes existing developer
		///</summary>
		///<param name ="id">Id of a developer</param>
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