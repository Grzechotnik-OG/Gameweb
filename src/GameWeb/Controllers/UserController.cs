using Microsoft.AspNetCore.Mvc;
using GameWeb.Models;
using GameWeb.Repositories;
using System.Threading.Tasks;
using GameWeb.Services;
using System;
using Microsoft.AspNetCore.Authorization;

namespace GameWeb.Controllers {
	[Route("api/v1/users")]
    [ApiController]
	public class UserController : ControllerBase{
		private readonly Context _context;
		private readonly IUsersRepository _usersRepository;
		private readonly IAuthService _authService;

		public UserController(Context context, IUsersRepository usersRepository, IAuthService authService)
        {
            _context = context;
			_usersRepository = usersRepository;
			_authService = authService;
        }

		[HttpPost("login")]
		public IActionResult SignIn(LoginDTO login)
		{
			if(!(_usersRepository.ValidateCredentials(login)))
			{
				return Unauthorized();
			}
			var user = _usersRepository.GetUserByUserName(login.UserName);
			return Ok(_authService.GenerateToken(user));
		}

		[HttpPost("signUp")]
		public IActionResult SignUp([FromBody]User user, string password)
		{
			return Ok(_usersRepository.AddUser(user, password));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetUser(long id)
		{
			return Ok(await _usersRepository.GetUserById(id));
		}

		[HttpPost("logout")]
		[Authorize]
		public IActionResult Logout()
		{
			var userName = Convert.ToInt64(User.Identity.Name);
    		_authService.RemoveRefreshTokenById(userName);
			return Ok();
		}

		[HttpDelete("delete")]
		public IActionResult Delete()
		{
			Logout();
			_usersRepository.RemoveUserById(Convert.ToInt64(User.Identity.Name));
			return NoContent();
		}

		[HttpPut("update")]
		public IActionResult UpdateUser(User user)
		{
			var id = Convert.ToInt64(User.Identity.Name);
			return Ok(_usersRepository.UpdateUser(user));
		}
	}
}
