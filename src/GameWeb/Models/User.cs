using System;

namespace GameWeb.Models {
	public class User {
		public long UserId { get; set; }
		public string NickName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}
}
