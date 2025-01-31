using System;
using System.Collections;

namespace Project.Screens{
    public sealed class LevelDetailPresenter : HuyDu_UISystem.ModalPresenter<LevelDetailModal, LevelDetailView, LevelDetailViewState>
    {
        private LevelSelectionData m_data;
        public LevelDetailPresenter(LevelDetailModal view) : base(view)
        {
        }

        public void Initialize(LevelSelectionData data){
            m_data = data;
            Initialize();
        }

        protected override IEnumerator ViewDidLoad(LevelDetailModal view, LevelDetailViewState viewState)
        {
            var model = m_data.Model;
            viewState.IsLocked = !m_data.IsUnlocked;
            viewState.LevelName = model.LevelName;
            viewState.LevelThumbnailGetter = m_data.Thumbnail;
            viewState.BackButtonClickEvent += OnBackClicked;
            viewState.StartLevelEvent += OnLevelStarted;
            yield break;
        }

        private void OnBackClicked()
        {
            m_data.PopCommand?.Invoke();
        }

        private void OnLevelStarted()
        {
            m_data.StartCommand?.Invoke();
        }
    }
}