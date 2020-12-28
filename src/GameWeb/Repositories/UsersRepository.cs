using System;
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

        public async Task<bool> ValidateCredentials(LoginDTO login)
        {
            throw new System.NotImplementedException();
        }
        public async Task<User> GetUserById(long id)
        {
            throw new System.NotImplementedException();
        }

    }
}