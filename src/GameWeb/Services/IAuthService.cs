using GameWeb.Models;

namespace GameWeb.Services
{
    public interface IAuthService
    {
        TokenDTO GenerateTokenDTO(User user);
        void RemoveRefreshTokenById(long id);
    }
}