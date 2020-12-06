using System;

namespace GameWeb.Models {
	public class Review {
		public string ID { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public int Rating { get; set; }
		public DateTime CreationDate { get; set; }
		public User User { get; set; }
		private Game Game { get; set; }
	}

}
