using System;

namespace GameWeb.Models {
	public class Review {
		public string ID;
		public string Title;
		public string Description;
		public int Rating;
		public DateTime CreationDate;
		public User User;
		private Game game;

	}

}
