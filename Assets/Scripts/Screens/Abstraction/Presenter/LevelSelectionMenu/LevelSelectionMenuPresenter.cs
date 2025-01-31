using System;
using System.Collections;
using HuyDu_UISystem;

namespace Project.Screens
{
    public sealed class LevelSelectionMenuPresenter : PagePresenter<LevelSelectionPage, LevelSelectionView, LevelSelectionViewState>
    {
        private readonly ILevelSelectionMenuUseCase m_useCase;
        public LevelSelectionMenuPresenter(LevelSelectionPage view, ILevelSelectionMenuUseCase useCase) : base(view)
        {
            m_useCase = useCase;
        }

        protected override IEnumerator ViewDidLoad(LevelSelectionPage view, LevelSelectionViewState viewState)
        {
            yield return m_useCase.FetchTable();

            foreach (LevelSelectionData data in m_useCase.GetAllLevels()){
                var itemState = new LevelSelectionItemViewState();
                SetupLevel(itemState, data.Model, data.IsUnlocked, data.IsCompleted);
                viewState.AddItemState(itemState);
            }
        }

        private void SetupLevel(LevelSelectionItemViewState itemState, MapLevelData model, bool isUnlocked, bool isCompleted)
        {

            ushort levelId = model.LevelId;

            itemState.LevelName = model.LevelName;
            itemState.IsUnlocked = isUnlocked;
            itemState.Completed = isCompleted;

            if(isUnlocked){
                itemState.ClickEvent += () => m_useCase.ShowLevelDetail(levelId);
            }
            else{
                itemState.ClickEvent += () => PopMessage(levelId);
            }
            itemState.LevelThumbnail = m_useCase.CreateObservableThumbnail(model.ThumbnailAddress);
        }

        private void PopMessage(ushort LevelId)
        {
            //TODO: Show popup about can't play this level if not unlocked
        }
    }
}