using System;

namespace GameWeb.Models {
	public class Game {
		public long ID { get; set; }
		public string Name { get; set; }
		public DateTime ReleaseDate { get; set; }
		public Developer Developer { get; set; }
		public GameGenreEnum Genre { get; set; }
	}
}
