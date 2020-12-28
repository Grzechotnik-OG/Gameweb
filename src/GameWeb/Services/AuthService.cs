using GameWeb.Models;

namespace GameWeb.Services
{
    public class AuthService : IAuthService
    {
        public TokenDTO GenerateToken(User user)
        {
            throw new System.NotImplementedException("Not implemented");
        }
        public void RemoveRefreshTokenById(long id)
        {
            throw new System.NotImplementedException("Not implemented");
        }
    }
}