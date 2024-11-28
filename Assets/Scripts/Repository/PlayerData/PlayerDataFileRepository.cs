using System;
using System.Threading.Tasks;
using Project.Domain.Player;
using Project.Persistence;

namespace Project.Repository{
    public sealed class PlayerDataFileRepository : IPlayerDataRepository
    {
        private PlayerData m_cache;
        public PlayerData PlayerData => m_cache;
        readonly IPersistenceService m_persistenceService;
        readonly string m_playerFile;
        public PlayerDataFileRepository(string playerFilePath, IPersistenceService persistenceService){
            m_persistenceService = persistenceService;
            m_playerFile = playerFilePath;
        }
        public async Task<PlayerData> LoadPlayerDataAsync()
        {
            m_cache ??= await m_persistenceService.Load<PlayerData>(m_playerFile);
            return m_cache;
        }
    }

    public sealed class PlayerDataRepositoryProxy : IPlayerDataRepository
    {
        private IPlayerDataRepository m_realRepos;
        private readonly Func<IPlayerDataRepository> m_creator;

        public PlayerDataRepositoryProxy(Func<IPlayerDataRepository> creator) => m_creator = creator;

        private IPlayerDataRepository GetRealRepos()
        {
            m_realRepos ??= m_creator.Invoke();
            return m_realRepos;
        }

        public PlayerData PlayerData => GetRealRepos().PlayerData;

        public Task<PlayerData> LoadPlayerDataAsync() => GetRealRepos().LoadPlayerDataAsync();
    }

    public sealed class MockPlayerDataRepository : IPlayerDataRepository
    {
        private readonly PlayerData m_mockData;
        public PlayerData PlayerData => m_mockData;

        public MockPlayerDataRepository(PlayerData mockData) => m_mockData = mockData;

        public Task<PlayerData> LoadPlayerDataAsync()
        {
            return Task.FromResult(m_mockData);
        }
    }
}