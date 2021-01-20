using System;
using GameWeb.Models.Entities;
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
		public DbSet<RefreshToken> RefreshTokens { get; set; }
		public DbSet<GameGenre> Genres { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Review>()
				.HasOne(r => r.Game)
				.WithMany(g => g.Reviews);

			modelBuilder.Entity<Game>()
				.HasOne(r => r.Developer)
				.WithMany();

			modelBuilder.Entity<Game>()
				.HasOne(r => r.Genre)
				.WithMany();
		}
	}
}
