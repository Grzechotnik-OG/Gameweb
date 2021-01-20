using System;
using System.Linq;
using System.Threading.Tasks;
using GameWeb.Models;
using Microsoft.EntityFrameworkCore;
using GameWeb.Models.DTO;
using GameWeb.Models.Entities;

namespace GameWeb.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly Context _context;
        public UsersRepository(Context context)
        {
            _context = context;
        }

        public bool ValidateCredentials(LoginDTO login)
        {
            User user;
            try
            {
                user = GetUserByUserName(login.UserName);
            }
            catch(Exception)
            {
                return false;
            }
            return BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash);
            //string hashedPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(login.Password,user.Salt,KeyDerivationPrf.HMACSHA1,10000, 256/8));
            //if(hashedPwd == user.PasswordHash){
            //    return true;
            //}
            //else{
            //    return false;
            //}
        }

        public async Task<User> GetUserById(long id)
        {
            var result = await _context.Users.FindAsync(id);
            if(result == null)
            {
                throw new System.Exception("Not Found!");
            }
            return result;
        }

        public User GetUserByUserName(string UserName)
        {
            var result = _context.Users.Where(a => a.UserName == UserName).FirstOrDefault();
            if(result == null)
            {
                throw new System.Exception("Not Found!");
            }
            return result;
        }

        public User GetUserByRefreshToken(string RefreshToken)
        {
            var result = _context.RefreshTokens.Where(a => a.Token == RefreshToken).Include(r => r.User).FirstOrDefault();
            if(result == null)
            {
                throw new System.Exception("Not Found!");
            }
            return result.User;
        }

        public async Task<long> AddUser(UserSignUpDTO user)
        {
            var userInDb =_context.Users.Where(x => x.UserName.Equals(user.UserName) || x.Email.Equals(user.Email)).FirstOrDefault();
            if(userInDb != null)
            {
                throw new Exception("User already exists"); // coś tu nie do końca działa
            }

            string hashedPwd = BCrypt.Net.BCrypt.HashPassword(user.Password);

            User entityUser = new User()
            {
                UserName = user.UserName,
                Email = user.Email,
                PasswordHash = hashedPwd,
                Role = "User"
            };
            var result = await _context.AddAsync<User>(entityUser);
            await _context.SaveChangesAsync();
            return result.Entity.UserId;
        }
        public async Task<User> RemoveUserById(long id)
        {
            var user = await GetUserById(id);
            var result = _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        public async Task<User> UpdateUser(User newUser, long userId)
        {
            var result = _context.Users.Update(newUser);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
    }
}