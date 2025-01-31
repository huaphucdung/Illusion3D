using System.Threading.Tasks;
using Project.Domain.Player;

namespace Project.Domain
{
    public sealed class PlayerDataRepository : IPlayerDataRepository
    {
        private PlayerData m_cachedData;
        public PlayerData PlayerData => m_cachedData;

        public async Task<PlayerData> LoadPlayerDataAsync()
        {
            await Task.Yield();
            return m_cachedData;
        }
    }

    internal sealed class MockPlayerDataRepository : IPlayerDataRepository
    {
        public PlayerData PlayerData {get; private set;}

        public Task<PlayerData> LoadPlayerDataAsync()
        {
            PlayerData = new PlayerData(id: 1, currentLevel: 1);
            return Task.FromResult(PlayerData);
        }
    }
}