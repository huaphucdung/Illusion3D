using System.Threading.Tasks;

namespace Project.Domain.Player{
    public interface IPlayerDataRepository{
        Task<PlayerData> LoadPlayerDataAsync();
    }
}