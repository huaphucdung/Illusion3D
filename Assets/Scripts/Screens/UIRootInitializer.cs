using System;
using Project.Events;
using Project.Repository;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityScreenNavigator.Runtime.Core.Page;
namespace Project.Screens{
    public sealed class UIRootInitializer : MonoBehaviour{
        private static bool m_isInitialized = false;
        private static UIRoot m_instance;
        [Header("Page Ids")]
        [SerializeField] string m_mainMenuId;
        [SerializeField] string m_levelSelectionMenuId;
        [SerializeField] string m_pauseMenuId;
        [SerializeField] Canvas m_mainCanvas;
        [SerializeField] PageContainer m_pageContainer;

        [SerializeField] AssetReferenceT<ScriptableMapTable> mapTableAsset;
        void Awake(){
            if(!m_isInitialized){
                m_instance = CreateUIRoot();
                m_isInitialized = true;
            }
        }

        void OnDestroy(){
            if(m_isInitialized){
                m_instance.Dispose();
            }
        }

        private UIRoot CreateUIRoot()
        {
            UIRoot.Builder builder = UIRoot.Builder.New();
            builder.WithEventBinding(CreateEventBinding<UIEvent>());
            builder.WithPopEventBinding(CreateEventBinding<PopUIEvent>());
            builder.WithScreenService(InitializeUIService());
            return builder.GetRootInstance();
        }

        private IEventBinding<TEvent> CreateEventBinding<TEvent>() where TEvent : IEvent
        {
            var binding = new EventBinding<TEvent>((_)=>{});
            EventBus<TEvent>.Register(binding);
            return binding;
        }

        private IUIScreenService InitializeUIService(){
            UINavigatorScreenService service = new UINavigatorScreenService(
                pageContainer: m_pageContainer,
                mainMenuFactory: new MainMenuFactory(m_mainMenuId, new EventMainMenuTransition()),
                levelSelectionMenuFactory: new LevelSelectionMenuFactory(m_levelSelectionMenuId, 3, new AddressableMapRepository<ScriptableMapTable>(mapTableAsset)),
                pauseMenuFactory: new PauseMenuFactory(m_pauseMenuId, new EventPauseMenuTransition())
            );

            return service;
        }
    }
}