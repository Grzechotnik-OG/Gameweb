using System;
using Microsoft.AspNetCore.Identity;

namespace GameWeb.Models {
	public class User
	{
		public long UserId { get; set; }
		public string NickName { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		public string Salt { get; set; }
	}
}
