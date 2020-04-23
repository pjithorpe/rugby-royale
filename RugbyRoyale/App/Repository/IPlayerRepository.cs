using RugbyRoyale.App.Model;
using System.Threading.Tasks;

namespace RugbyRoyale.App.Repository
{
    public interface IPlayerRepository
    {
        Task<bool> ExistsAsync(string playerId);

        Task SaveAsync(Player player);
    }
}
