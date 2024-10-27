using System;
using System.Collections;
using Project.Domain.MapLevel;
using Project.Repository;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityScreenNavigator.Runtime.Core.Page;

namespace Project.Screens.Testing{
    [RequireComponent(typeof(PageContainer))]
    internal sealed class ApplicationRoot : MonoBehaviour{
        [SerializeField] private string MainMenuKey = "UIScreens/MainMenu";
        [SerializeField] AssetReferenceT<ScriptableMapTable> mapTableAsset;
        private PageContainer m_UIPageContainer;
        private IMainMenuTransition m_mainMenuTransition;
        private IPauseMenuTransition m_pauseMenuTransition;
        private IMapRepository mapRepository;

        void Awake(){
            m_UIPageContainer = GetComponent<PageContainer>();
            mapRepository = new AddressableMapRepository<ScriptableMapTable>(mapTableAsset);
        }

        IEnumerator Start(){
            m_mainMenuTransition = new DebugMainMenuTransition(new TestMainMenuTransition(this));
            m_pauseMenuTransition = new DebugPauseMenuTransition(new TestPauseMenuTransition(this));
            yield return mapRepository.FetchMapTable();

            OnAppStarted();
        }

        void OnDestroy(){
            mapRepository?.Dispose();
        }

        private void OnAppStarted()
        {
            m_UIPageContainer.Push<MainMenuPage>(MainMenuKey, playAnimation: true, stack: true, onLoad: OnMainMenuLoaded);
        }

        private void OnMainMenuLoaded((string pageId, MainMenuPage page) tuple)
        {
            MainMenuPresenter presenter = new MainMenuPresenter(tuple.page, m_mainMenuTransition);
            presenter.Initialize();
        }

        internal void StartGame()
        {
            MapLevelModel mapLevelModel = mapRepository.MapTable.GetMapLevelModel(0);
            if(mapLevelModel != null){
                Addressables.LoadSceneAsync(mapLevelModel.Address);
            }
            // OpenPauseMenu();
        }

        internal void ExitGame()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        internal void ReturnToMainMenu()
        {
            m_UIPageContainer.Pop(playAnimation: true, popCount: 1);
        }

        internal void OpenPauseMenu(){
            m_UIPageContainer.Push<PauseMenuPage>("UIScreens/PauseMenu", playAnimation: true, stack: true, onLoad: OnPauseMenuLoaded);
        }

        private void OnPauseMenuLoaded((string pageId, PauseMenuPage page) tuple)
        {
            PauseMenuPresenter presenter = new PauseMenuPresenter(tuple.page, m_pauseMenuTransition);
            presenter.Initialize();
        }
    }
}