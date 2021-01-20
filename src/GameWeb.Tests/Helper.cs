using System;
using System.Security.Cryptography;
using GameWeb.Models;
using GameWeb.Models.DTO;
using GameWeb.Models.Entities;
using GameWeb.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace GameWeb.Tests
{
    public class Helper
    {
        public static void InitializeDbForTests(Context context)
        {
            IUsersRepository userRepo = new UsersRepository(context);

            Developer dev = new Developer()
            {
                Name = "test",
                EstablishmentYear = 2000
            };
            GameGenre genre = new GameGenre()
            {
                Name = "test",
            };

            context.Add<Developer>(dev);
            context.Add<GameGenre>(genre);
            context.SaveChanges();

            Game game = new Game()
            {
                Name = "test",
                ReleaseDate = DateTime.Now,
                Developer = dev,
                Genre = genre
            };
            context.Add<Game>(game);
            context.SaveChanges();

            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashedPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2("haslo123", salt, KeyDerivationPrf.HMACSHA1, 10000, 256/8));

            User entityUser = new User()
            {
                UserName = "Moderator",
                Email = "test@test.com",
                Salt = salt,
                PasswordHash = hashedPwd,
                Role = "Mod"
            };
            context.Add<User>(entityUser);
            context.SaveChanges();

            User regularUser = new User()
            {
                UserName = "Regular",
                Email = "testUser@test.com",
                Salt = salt,
                PasswordHash = hashedPwd,
                Role = "User"
            };
            context.Add<User>(regularUser);
            context.SaveChanges();
        }
    }
}