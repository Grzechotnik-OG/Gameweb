using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GameWeb.Models {
	public class User
	{
		public long UserId { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		[JsonIgnore]
		public string PasswordHash { get; set; }
		[JsonIgnore]
		public byte[] Salt { get; set; }
		[JsonIgnore]
		public string Role { get; set; }
		[JsonIgnore]
		public List<RefreshToken> RefreshTokens {get; set;}
	}
}
