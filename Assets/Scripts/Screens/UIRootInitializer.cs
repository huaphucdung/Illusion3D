using System;
using Project.Bootstrapper;
using Project.Domain.MapLevel;
using Project.Domain.Player;
using Project.Events;
using Project.Repository;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityScreenNavigator.Runtime.Core.Modal;
using UnityScreenNavigator.Runtime.Core.Page;
namespace Project.Screens{

    [System.Serializable]
    public class UIRootInitializer : MonoInitializer
    {
        private static bool s_isInitialized = false;
        private static UIRoot s_instance;
        [Header("Page Ids")]
        [SerializeField] string m_mainMenuId;
        [SerializeField] string m_levelSelectionMenuId, m_levelDetailPanelId;
        [SerializeField] string m_pauseMenuId;
        [SerializeField] PageContainer m_pageContainer;
        [SerializeField] ModalContainer m_modalContainer;

        public override void Initialize(IDependencyContainer dependencyContainer)
        {
            if(!s_isInitialized){
                s_instance = CreateUIRoot(dependencyContainer);
                s_isInitialized = true;
            }
        }

        public override void Dispose()
        {
            if(s_isInitialized){
                s_instance.Dispose();
            }
            s_isInitialized = false;
        }

        private UIRoot CreateUIRoot(IDependencyContainer container)
        {
            UIRoot.Builder builder = UIRoot.Builder.New();
            builder.WithEventBinding(CreateEventBinding<UIEvent>());
            builder.WithPopEventBinding(CreateEventBinding<PopUIEvent>());

            IMapRepository mapRepository = container.GetDependency<IMapRepository>();
            IPlayerDataRepository playerDataRepository = container.GetDependency<IPlayerDataRepository>();
            builder.WithScreenService(InitializeUIService(mapRepository, playerDataRepository));
            return builder.GetRootInstance();
        }

        private IEventBinding<TEvent> CreateEventBinding<TEvent>() where TEvent : IEvent
        {
            var binding = new EventBinding<TEvent>((_)=>{});
            EventBus<TEvent>.Register(binding);
            return binding;
        }

        private IUIScreenService InitializeUIService(IMapRepository mapRepos, IPlayerDataRepository playerRepos){
            UINavigatorScreenService service = new UINavigatorScreenService(
                pageContainer: m_pageContainer,
                mainMenuFactory: new MainMenuFactory(m_mainMenuId, new EventMainMenuTransition()),
                levelSelectionMenuFactory: InitializeLevelSelectionMenuFactory(mapRepos, playerRepos),
                pauseMenuFactory: new PauseMenuFactory(m_pauseMenuId, new EventPauseMenuTransition())
            );

            return service;
        }

        private LevelSelectionMenuFactory InitializeLevelSelectionMenuFactory(IMapRepository mapRepos, IPlayerDataRepository playerRepos){
            var useCase = new LevelSelectionMenuUseCase(playerRepos, mapRepos, new LevelDetailFactory(m_levelDetailPanelId), m_modalContainer);
            return new LevelSelectionMenuFactory(m_levelSelectionMenuId, useCase);
        }
    }
}