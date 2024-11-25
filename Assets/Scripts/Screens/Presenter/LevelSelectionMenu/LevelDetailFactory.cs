using UnityScreenNavigator.Runtime.Core.Modal;

namespace Project.Screens{

    internal interface ILevelDetailFactory{
        void PushModalTo(ModalContainer container, LevelSelectionData levelData, bool playAnimation = true);
    }
    public sealed class LevelDetailFactory : AbstractModalFactory<LevelDetailModal, LevelDetailPresenter>, ILevelDetailFactory
    {
        private LevelSelectionData m_currentLevelData;
        public LevelDetailFactory(string modalId) : base(modalId)
        {
        }

        public void PushModalTo(ModalContainer container, LevelSelectionData levelData, bool playAnimation = true)
        {
            m_currentLevelData = levelData;
            PushModalTo(container, playAnimation);
        }

        protected override LevelDetailPresenter CreatePresenter(LevelDetailModal modal)
        {
            return new LevelDetailPresenter(modal);
        }

        protected override void OnPresenterCreated(LevelDetailPresenter presenter)
        {
            presenter.Initialize(m_currentLevelData);
        }
    }
}