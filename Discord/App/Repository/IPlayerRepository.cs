using RugbyRoyale.Entities.Model;
using System.Threading.Tasks;

namespace RugbyRoyale.Discord.App.Repository
{
    public interface IPlayerRepository
    {
        Task<bool> ExistsAsync(string playerId);

        Task SaveAsync(Player player);
    }
}