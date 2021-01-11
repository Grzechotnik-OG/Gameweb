using System;
using System.Collections.Generic;

namespace GameWeb.Models {
	public class User
	{
		public long UserId { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string PasswordHash { get; set; }
		public byte[] Salt { get; set; }
		public string Role { get; set; }
		public List<RefreshToken> RefreshTokens {get; set;}
	}
}
