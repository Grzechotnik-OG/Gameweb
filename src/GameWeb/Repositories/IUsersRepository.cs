using System.Threading.Tasks;
using GameWeb.Models;

namespace GameWeb.Repositories
{
    public interface IUsersRepository
    {
        Task<bool> ValidateCredentials(LoginDTO login);
        Task<User> GetUserById(long id);
    }
}