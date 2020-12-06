using System;

namespace GameWeb.Models {
	public class Rating {
		public long ID { get; set; }
		public User User { get; set; }
		public Game Game { get; set; }
		private int rating { get; set; }
	}

}
