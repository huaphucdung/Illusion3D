using System;
using System.Collections;
using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Page;

namespace Project.Screens.Testing{
    [RequireComponent(typeof(PageContainer))]
    internal sealed class ApplicationRoot : MonoBehaviour{
        [SerializeField] private string MainMenuKey = "UIScreens/MainMenu";
        private PageContainer m_UIPageContainer;
        private IMainMenuTransition m_mainMenuTransition;
        private IPauseMenuTransition m_pauseMenuTransition;

        void Awake(){
            m_UIPageContainer = GetComponent<PageContainer>();
        }

        void Start(){
            m_mainMenuTransition = new DebugMainMenuTransition(new TestMainMenuTransition(this));
            m_pauseMenuTransition = new DebugPauseMenuTransition(new TestPauseMenuTransition(this));
            

            OnAppStarted();
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
            OpenPauseMenu();
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