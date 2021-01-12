using System;
using System.Text.Json.Serialization;
using GameWeb.Models.Entities;

namespace GameWeb.Models
{
	public class RefreshToken
    {
        public long RefreshTokenId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set;}
        public long UserId { get; set; }
		[JsonIgnore]
		public User User { get; set; }
    }
}