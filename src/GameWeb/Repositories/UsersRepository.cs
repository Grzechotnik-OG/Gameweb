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

        public async Task<TokenDTO> Login(LoginDTO login)
        {
            throw new System.NotImplementedException();
        }


    }
}