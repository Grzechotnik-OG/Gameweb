using System;
using Microsoft.AspNetCore.Mvc;
using GameWeb.Models;

namespace GameWeb.Controllers {
	public class UserController : ControllerBase{
		private readonly Context _context;

		public UserController(Context context)
        {
            _context = context;
        }
		public void SignIn() {
			throw new System.NotImplementedException("Not implemented");
		}
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
