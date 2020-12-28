using GameWeb.Models;

namespace GameWeb.Services
{
    public interface IAuthService
    {
        TokenDTO GenerateToken(User user);
        void RemoveRefreshTokenById(long id);
    }
}