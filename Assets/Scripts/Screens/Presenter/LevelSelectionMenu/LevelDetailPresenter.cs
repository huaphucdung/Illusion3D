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
            viewState.LevelThumbnailGetter = new Utilities.AddressableAssetGetter<UnityEngine.Sprite>(model.ThumbnailAddress);
            viewState.StartLevelEvent += OnLevelStarted;
            yield break;
        }

        private void OnLevelStarted()
        {
            UnityEngine.Debug.Log($"Start level: {m_data.Model.LevelName}");
        }
    }
}