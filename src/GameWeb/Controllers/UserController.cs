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

		[HttpPost("login")] //brak
		public IActionResult SignIn(LoginDTO login)
		{
			if(!(_usersRepository.ValidateCredentials(login)))
			{
				return Unauthorized();
			}
			var user = _usersRepository.GetUserByUserName(login.UserName);
			_authService.GenerateRefreshToken(user);
			return Ok(_authService.GenerateTokenDTO(user));
		}

		[AllowAnonymous]
		[HttpPost("refresh-token")]
		public IActionResult Refresh([FromBody] RefreshTokenDTO refreshToken){
			if(!ModelState.IsValid) return BadRequest(ModelState);
			return Ok(_authService.ExchangeRefreshToken(refreshToken));
		}

		[HttpPost("signUp")] //brak
		public IActionResult SignUp([FromBody]UserSignUpDTO user)
		{
			return Ok(_usersRepository.AddUser(user));
		}

		[HttpGet("{id}")] //brak
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

		[HttpDelete("delete")] //token zwykly //sprawdzenie uzytkownika
		public IActionResult Delete()
		{
			Logout();
			_usersRepository.RemoveUserById(Convert.ToInt64(User.Identity.Name));
			return NoContent();
		}

		[HttpPut("update")] //token zwykly //sprawdzenie uzytkownika
		public IActionResult UpdateUser(User user)
		{
			var id = Convert.ToInt64(User.Identity.Name);
			return Ok(_usersRepository.UpdateUser(user));
		}
	}
}
