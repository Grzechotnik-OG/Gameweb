using System.Threading.Tasks;
using GameWeb.Models;

namespace GameWeb.Repositories
{
    public interface IUsersRepository
    {
        bool ValidateCredentials(LoginDTO login);
        Task<User> GetUserById(long id);
        User GetUserByUserName(string UserName);
        Task<long> AddUser(UserSignUpDTO user);
        Task<User> RemoveUserById(long id);
        Task<User> UpdateUser(User newUser);
    }
}