using System;
using Microsoft.AspNetCore.Mvc;
using GameWeb.Models;
using GameWeb.Repositories;

namespace GameWeb.Controllers {
	public class UserController : ControllerBase{
		private readonly Context _context;
		private readonly IUsersRepository _usersRepository;

		public UserController(Context context, IUsersRepository usersRepository)
        {
            _context = context;
			_usersRepository = usersRepository;
        }
		[HttpPost("login")]
		public void SignIn() {
			throw new System.NotImplementedException("Not implemented");
		}
		[HttpPost("login")]
		public void SignUp() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void GetUser() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void Logout() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void Delete() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void UpdateUser() {
			throw new System.NotImplementedException("Not implemented");
		}

	}

}
