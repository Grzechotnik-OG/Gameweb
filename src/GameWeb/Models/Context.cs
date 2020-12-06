using System;
using Microsoft.EntityFrameworkCore;

namespace GameWeb.Models
{
	public class Context : DbContext 
	{
		public Context(DbContextOptions<Context> options): base(options)
    	{ }
		public DbSet<Game> Games { get; set; }
		public DbSet<Developer> Developers { get; set; }
		public DbSet<Review> Reviews { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Rating> Ratings { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Review>()
				.HasOne(r => r.Game)
				.WithMany(g => g.Reviews);
		}	
	}
}
