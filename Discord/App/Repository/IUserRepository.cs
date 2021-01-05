using RugbyRoyale.Entities.Model;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Repository
{
    public interface IUserRepository
    {
        Task<User> GetAsync(string userID);

        Task<bool> ExistsAsync(string userID);

        Task<bool> SaveAsync(User user);
    }
}