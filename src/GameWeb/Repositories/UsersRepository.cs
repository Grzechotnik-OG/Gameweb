using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Linq;
using System.Threading.Tasks;
using GameWeb.Models;

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
            var user = GetUserByUserName(login.UserName);
            string hashedPwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(login.Password,user.Salt,KeyDerivationPrf.HMACSHA1,10000, 256/8));
            if(hashedPwd == user.PasswordHash){
                return true;
            }
            else{
                return false;
            }
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
        public User AddUser(User user){throw new System.NotImplementedException("Not implemented");}
        public void RemoveUserById(long id){throw new System.NotImplementedException("Not implemented");}
        public User UpdateUser(long id, User newUser){throw new System.NotImplementedException("Not implemented");}
    }
}