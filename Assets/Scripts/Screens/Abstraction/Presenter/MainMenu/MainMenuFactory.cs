namespace Project.Screens{
    public sealed class MainMenuFactory : AbstractPageFactory<MainMenuPage, MainMenuPresenter>
    {
        private readonly IMainMenuTransition m_mainMenuTransition;
        public MainMenuFactory(string pageId, IMainMenuTransition mainMenuTransition) : base(pageId) => m_mainMenuTransition = mainMenuTransition;
        protected override MainMenuPresenter CreatePresenter(MainMenuPage page)
        {
            return new MainMenuPresenter(page, m_mainMenuTransition);
        }
    }
}