using System;
using System.Collections.Generic;

namespace GameWeb.Models.Entities
{
	public class Game
	{
		public long GameId { get; set; }
		public string Name { get; set; }
		public DateTime ReleaseDate { get; set; }
		public Developer Developer { get; set; }
		public GameGenre Genre { get; set; }
		public List<Review> Reviews { get; set; }
	}
}
