using Microsoft.AspNetCore.Mvc;
using GameWeb.Models;
using GameWeb.Repositories;
using System.Threading.Tasks;
using GameWeb.Services;
using System;
using Microsoft.AspNetCore.Authorization;
using GameWeb.Models.Entities;
using GameWeb.Models.DTO;
using System.Linq;

namespace GameWeb.Controllers
{
	[Route("api/v1/users")]
    [ApiController]
	public class UserController : ControllerBase
	{
		private readonly Context _context;
		private readonly IUsersRepository _usersRepository;
		private readonly IAuthService _authService;

		public UserController(Context context, IUsersRepository usersRepository, IAuthService authService)
        {
            _context = context;
			_usersRepository = usersRepository;
			_authService = authService;
        }
		///<summary>
		///Login as a user
		///</summary>
		[HttpPost("login")]
		public IActionResult SignIn(LoginDTO login)
		{
			if(!(_usersRepository.ValidateCredentials(login)))
			{
				return Unauthorized();
			}
			var user = _usersRepository.GetUserByUserName(login.UserName);
			return Ok(_authService.GenerateTokenDTO(user));
		}

		///<summary>
		///Refreshes access token
		///</summary>
		[Authorize(Policy = Policies.RefreshToken)]
		[HttpGet("refresh-token")]
		public IActionResult Refresh()
		{
			long userId = Convert.ToInt64(User.Identity.Name);
			string role = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;
			if(!ModelState.IsValid) return BadRequest(ModelState);
			return Ok(_authService.ExchangeToken(userId,role));
		}

		[AllowAnonymous]
		[HttpPost("signUp")]
		public async Task<IActionResult> SignUp([FromBody]UserSignUpDTO user)
		{
			try
			{
				return Ok(await _usersRepository.AddUser(user));
			}
			catch(Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetUser(long id)
		{
			return Ok(await _usersRepository.GetUserById(id));
		}

		[HttpDelete("delete")]
		[Authorize]
		public IActionResult Delete()
		{
			//Logout();
			_usersRepository.RemoveUserById(Convert.ToInt64(User.Identity.Name));
			return NoContent();
		}

		[HttpPut("update")]
		[Authorize]
		public IActionResult UpdateUser(User user)
		{
			var id = Convert.ToInt64(User.Identity.Name);
			return Ok(_usersRepository.UpdateUser(user, id));
		}
	}
}
