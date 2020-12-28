using Microsoft.AspNetCore.Mvc;
using GameWeb.Models;
using GameWeb.Repositories;
using System.Threading.Tasks;
using GameWeb.Services;
using System;

namespace GameWeb.Controllers {
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
		public async Task<IActionResult> SignIn(LoginDTO login)
		{
			if(!(await _usersRepository.ValidateCredentials(login)))
			{
				return Unauthorized();
			}
			var user = _usersRepository.GetUserByUserName(login.UserName);
			return Ok(await _authService.GenerateToken(user));
		}

		[HttpPost("signUp")]
		public async Task<IActionResult> SignUp(User user)
		{
			return Ok(await _usersRepository.AddUser(user));
		}

		[HttpPost("{id}")]
		public async Task<IActionResult> GetUser(long id)
		{
			return Ok(await _usersRepository.GetUserById(id));
		}

		[HttpPost("logout")]
		public IActionResult Logout()
		{
			var userName = Convert.ToInt64(User.Identity.Name);
    		_authService.RemoveRefreshTokenById(userName);
			return Ok();
		}

		[HttpDelete("delete")]
		public async Task<IActionResult> Delete()
		{
			Logout();
			await _usersRepository.RemoveUserById(User.Identity.Name);
			return NoContent();
		}

		[HttpPut("update")]
		public async Task<IActionResult> UpdateUser(User user)
		{
			var id = User.Identity.Name;
			return Ok(await _usersRepository.UpdateUser(id, user));
		}
	}
}
