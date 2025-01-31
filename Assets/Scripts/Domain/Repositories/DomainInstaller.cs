using Project.Domain.Player;
using Project.Domain.MapLevel;
using Zenject;
using UnityEngine;

namespace Project.Domain
{
    public class DomainInstaller : MonoInstaller{
        [SerializeField] ScriptableMapTable m_mapTable;
        public override void InstallBindings()
        {
            Container.Bind<IPlayerDataRepository>().To<MockPlayerDataRepository>().FromInstance(new MockPlayerDataRepository()).AsSingle().NonLazy();
            Container.Bind<IMapRepository>().To<LocalMapLevelRepository>().FromInstance(new LocalMapLevelRepository(m_mapTable)).AsSingle().NonLazy();
        }
    }

}