using System.Threading.Tasks;
using GameWeb.Models;

namespace GameWeb.Repositories
{
    public interface IUsersRepository
    {
        bool ValidateCredentials(LoginDTO login);
        Task<User> GetUserById(long id);
        User GetUserByUserName(string UserName);
        User AddUser(User user);
        void RemoveUserById(long id);
        User UpdateUser(long id, User newUser);
    }
}