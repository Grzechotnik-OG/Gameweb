using System;
using System.Text.Json.Serialization;

namespace GameWeb.Models
{
	public class ReviewDTO
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public int Rating { get; set; }
	}
}
