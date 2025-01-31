using System;
using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Modal;
using UnityScreenNavigator.Runtime.Core.Page;

namespace Project.Screens{
    public sealed class UISystemInstaller : Zenject.MonoInstaller<UISystemInstaller> {
        private const string PageContainerTag = "PageContainer", ModalContainerTag = "ModalContainer";
        [UnityEngine.SerializeField] Canvas m_canvas;
        [Header("Page Ids")]
        [SerializeField] string m_mainMenuPageId; 
        [SerializeField] string m_pauseMenuPageId;
        [SerializeField] string m_levelSelectionMenuPageId;

        [Header("Modal Ids")]
        [SerializeField]string m_levelDetailModalId;

        public override void InstallBindings() {

            Container.Bind<PageContainer>().FromMethod(CreatePageContainer).AsSingle().NonLazy();
            Container.Bind<ModalContainer>().FromMethod(CreateModalContainer).AsSingle().NonLazy();

            BindAllScreenFactories();
            Container.Bind<IUIScreenService>().To<UINavigatorScreenService>().AsSingle();

            Container.Bind<UIRoot>().AsSingle().NonLazy();
            Container.BindDisposableExecutionOrder<UIRoot>(order: 100);
        }

        private PageContainer CreatePageContainer() => CreateContainer<PageContainer>(PageContainerTag);
        private ModalContainer CreateModalContainer() => CreateContainer<ModalContainer>(ModalContainerTag);

        private T CreateContainer<T>(string name) where T : Component
        {
            var root = m_canvas.gameObject;
            foreach(Transform child in root.transform){
                if(child.gameObject.CompareTag(name)){
                    return child.GetComponent<T>();
                }
            }

            var pageContainerObj = new GameObject(name);
            pageContainerObj.tag = name;
            pageContainerObj.transform.SetParent(root.transform, false);
            pageContainerObj.AddComponent<RectTransform>();
            return pageContainerObj.AddComponent<T>();
        }

        private void BindAllScreenFactories()
        {
            Container.Bind<MainMenuFactory>().FromInstance(new MainMenuFactory(m_mainMenuPageId, new EventMainMenuTransition())).AsSingle().NonLazy();
            Container.Bind<ILevelDetailFactory>().To<LevelDetailFactory>().FromInstance(new LevelDetailFactory(m_levelDetailModalId)).AsSingle().NonLazy();

            Container.Bind<ILevelSelectionMenuUseCase>().To<LevelSelectionMenuUseCase>().AsSingle().NonLazy();

            
            Container   .Bind<LevelSelectionMenuFactory>()
                        .AsSingle().WithArguments(m_levelSelectionMenuPageId);
            
            Container.Bind<PauseMenuFactory>().FromInstance(new PauseMenuFactory(m_pauseMenuPageId, new EventPauseMenuTransition())).AsSingle().NonLazy();
        }
    }
}