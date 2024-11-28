using System;
using System.Collections;
using HuyDu_UISystem;
using Project.Domain.MapLevel;
using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Modal;

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
                if(data.IsUnlocked) SetupForCompletedLevel(itemState, data.Model);
                else SetupForIncompleteLevel(itemState, data.Model);
                viewState.AddItemState(itemState);
            }
        }

        private void SetupForIncompleteLevel(LevelSelectionItemViewState itemState, MapLevelModel model)
        {

            ushort levelId = model.LevelId;

            itemState.LevelName = model.LevelName;
            itemState.IsUnlocked = false;
            itemState.ClickEvent += () => OnLevelItemClicked(levelId);
            itemState.LevelThumbnail = new Utilities.AddressableAssetGetter<Sprite>(model.ThumbnailAddress);
        }

        private void SetupForCompletedLevel(LevelSelectionItemViewState itemState, MapLevelModel model)
        {
            ushort levelId = model.LevelId;

            itemState.LevelName = model.LevelName;
            itemState.IsUnlocked = true;
            itemState.ClickEvent += () => OnLevelItemClicked(levelId);
            itemState.LevelThumbnail = new Utilities.AddressableAssetGetter<Sprite>(model.ThumbnailAddress);
        }

        private void OnLevelItemClicked(ushort id) {
            m_useCase.ShowLevelDetail(id);
        }
    }
}