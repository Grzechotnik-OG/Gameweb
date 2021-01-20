using GameWeb.Models;
using GameWeb.Models.DTO;
using GameWeb.Models.Entities;

namespace GameWeb.Services
{
    public interface IAuthService
    {
        TokenDTO GenerateTokenDTO(User user);
        string ExchangeToken(long userId, string role);
    }
}