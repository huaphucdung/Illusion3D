using System;
using UnityScreenNavigator.Runtime.Core.Page;
namespace Project.Screens{
    public sealed class UINavigatorScreenService : IUIScreenService
    {
        private readonly PageContainer m_pageContainer;
        private readonly MainMenuFactory m_mainMenuFactory;
        private readonly LevelSelectionMenuFactory m_levelSelectionMenuFactory;
        private readonly PauseMenuFactory m_pauseMenuFactory;

        public UINavigatorScreenService(PageContainer pageContainer, MainMenuFactory mainMenuFactory, LevelSelectionMenuFactory levelSelectionMenuFactory, PauseMenuFactory pauseMenuFactory){
            m_pageContainer = pageContainer;
            m_mainMenuFactory = mainMenuFactory;
            m_levelSelectionMenuFactory = levelSelectionMenuFactory;
            m_pauseMenuFactory = pauseMenuFactory;
        }

        public void PopScreen(int count = 1)
        {
            count = Math.Min(count, m_pageContainer.Pages.Count);
            m_pageContainer.Pop(playAnimation: true, popCount: count);
        }

        public void ShowScreen(ScreenType type)
        {
            switch(type){
                case ScreenType.MainMenu: ShowMainMenu(); break;
                case ScreenType.LevelSelectionMenu: ShowLevelSelectionMenu(); break;
                case ScreenType.PauseMenu: ShowPauseMenu(); break;
            }  
        }

        private void ShowPauseMenu()
        {
            m_pauseMenuFactory.PushScreenTo(m_pageContainer);
        }

        private void ShowLevelSelectionMenu()
        {
            m_levelSelectionMenuFactory.PushScreenTo(m_pageContainer);
        }

        private void ShowMainMenu()
        {
            m_mainMenuFactory.PushScreenTo(m_pageContainer);
        }
    }
}