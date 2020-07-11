using RugbyRoyale.Entities.Model;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Repository
{
    public interface IPlayerRepository
    {
        Task<bool> ExistsAsync(string playerID);

        Task SaveAsync(Player player);
    }
}