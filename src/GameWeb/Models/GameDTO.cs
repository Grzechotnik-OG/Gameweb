using System;

namespace GameWeb.Models
{
    public class GameDTO
    {
		public string Name { get; set; }
		public DateTime ReleaseDate { get; set; }
		public long DeveloperId { get; set; }
		public long GenreId { get; set; }
    }
}