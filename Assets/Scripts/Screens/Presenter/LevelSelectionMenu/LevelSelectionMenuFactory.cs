namespace Project.Screens{
    public sealed class LevelSelectionMenuFactory : AbstractScreenFactory<LevelSelectionPage, LevelSelectionMenuPresenter>
    {
        private readonly ushort m_currentLevelId;
        private readonly Domain.MapLevel.IMapRepository m_mapRepository;

        public LevelSelectionMenuFactory(string pageId, ushort currentLevelId, Domain.MapLevel.IMapRepository mapRepository) : base(pageId)
        {
            m_currentLevelId = currentLevelId;
            m_mapRepository = mapRepository;
        }
        protected override LevelSelectionMenuPresenter CreatePresenter(LevelSelectionPage page)
        {
            return new LevelSelectionMenuPresenter(page, m_currentLevelId, m_mapRepository);
        }
    }
}