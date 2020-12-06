using System;
using System.Text.Json.Serialization;

namespace GameWeb.Models {
	public class Review {
		public long ReviewId { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int Rating { get; set; }
		public DateTime CreationDate { get; set; }
		public User User { get; set; }
		public long GameId { get; set; }
		[JsonIgnore]
		public Game Game { get; set; }
	}
}
