using System;
using Microsoft.AspNetCore.Mvc;
using GameWeb.Models;

namespace GameWeb.Controllers {
	public class GameController : ControllerBase{
		private readonly Context _context;

		public GameController(Context context)
        {
            _context = context;
        }
		public void GetGame() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void SearchByGenre() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void AddGame() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void AddReview() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void UpdateReview() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void DeleteReview() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void UpdateGameInfo() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void AddRating() {
			throw new System.NotImplementedException("Not implemented");
		}

	}

}
