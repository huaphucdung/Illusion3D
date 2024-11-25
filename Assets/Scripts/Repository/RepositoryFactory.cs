using System;
using Project.Bootstrapper;
using Project.Domain.MapLevel;
using Project.Domain.Player;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Project.Repository{
    [CreateAssetMenu(fileName = "RepositoryFactory", menuName = "Dependencies/RepositoryFactory")]
    public sealed class RepositoryFactory : ScriptableDependencyFactory
    {
        [SerializeField] private AssetReferenceT<ScriptableMapTable> m_mapTableAsset;
        [SerializeField] string m_playerDataFilePath;
        public override void RegisterDependencies(DependencyContainer container)
        {
            IDependencyRegistry registry = container;
            registry.RegisterDependency<IMapRepository>(new AddressableMapRepository<ScriptableMapTable>(m_mapTableAsset));
            registry.RegisterDependency<IPlayerDataRepository>(new PlayerDataRepositoryProxy(()=>CreateRealPlayerDataRepository(container)));
        }

        private IPlayerDataRepository CreateRealPlayerDataRepository(IDependencyContainer container)
        {
            return new MockPlayerDataRepository(new PlayerData());
            // return new PlayerDataFileRepository(m_playerDataFilePath, container.GetDependency<Persistence.IPersistenceService>());
        }
    }
}