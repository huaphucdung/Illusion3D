namespace Project.Screens{
    public sealed class LevelSelectionMenuFactory : AbstractPageFactory<LevelSelectionPage, LevelSelectionMenuPresenter>
    {
        private readonly ILevelSelectionMenuUseCase m_useCase;

        public LevelSelectionMenuFactory(string pageId, ILevelSelectionMenuUseCase useCase) : base(pageId)
        {
            m_useCase = useCase;
        }
        protected override LevelSelectionMenuPresenter CreatePresenter(LevelSelectionPage page)
        {
            return new LevelSelectionMenuPresenter(page, m_useCase);
        }
    }
}