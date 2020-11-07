using System;
using Microsoft.EntityFrameworkCore;

namespace GameWeb.Models{
	public class Context : DbContext {
		public Context(DbContextOptions<Context> options): base(options)
    	{ }
		private DbSet<Game> Games {get;set;}
		private DbSet<Developer> Developers{get;set;}
		private DbSet<Review> Reviews{get;set;}
		private DbSet<User> Users{get;set;}
		private DbSet<Rating> Ratings{get;set;}

	}
}
