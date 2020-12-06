using System;

namespace GameWeb.Models {
	public class Rating {
		private string ID { get; set; }
		public User User { get; set; }
		public Game Game { get; set; }
		private int rating { get; set; }
	}

}
