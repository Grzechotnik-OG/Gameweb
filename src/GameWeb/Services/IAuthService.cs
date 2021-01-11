using GameWeb.Models;

namespace GameWeb.Services
{
    public interface IAuthService
    {
        TokenDTO GenerateTokenDTO(User user);
        RefreshToken GenerateRefreshToken(User user);
        RefreshTokenDTO ExchangeRefreshToken(RefreshTokenDTO message);
        void RemoveRefreshTokenById(long id);
    }
}