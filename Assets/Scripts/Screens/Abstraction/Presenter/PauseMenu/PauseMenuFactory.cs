namespace Project.Screens
{
    public sealed class PauseMenuFactory : AbstractPageFactory<PauseMenuPage, PauseMenuPresenter>
    {
        private readonly IPauseMenuTransition m_pauseMenuTransition;
        public PauseMenuFactory(string pageId, IPauseMenuTransition pauseMenuTransition) : base(pageId) => m_pauseMenuTransition = pauseMenuTransition;
        protected override PauseMenuPresenter CreatePresenter(PauseMenuPage page)
        {
            return new PauseMenuPresenter(page, m_pauseMenuTransition);
        }
    }
}