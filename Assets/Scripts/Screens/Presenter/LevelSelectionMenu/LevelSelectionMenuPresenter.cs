using System.Collections;
using HuyDu_UISystem;
using Project.Domain.MapLevel;
using UnityEngine;

namespace Project.Screens
{
    public sealed class LevelSelectionMenuPresenter : PagePresenter<LevelSelectionPage, LevelSelectionView, LevelSelectionViewState>
    {
        private readonly IMapRepository m_mapRepository;
        private readonly ushort m_currentLevelId;
        public LevelSelectionMenuPresenter(LevelSelectionPage view,ushort currentLevelId, IMapRepository mapRepository) : base(view)
        {
            m_currentLevelId = currentLevelId;
            m_mapRepository = mapRepository;
        }

        protected override IEnumerator ViewDidLoad(LevelSelectionPage view, LevelSelectionViewState viewState)
        {
            yield return m_mapRepository.FetchMapTable();

            ushort i = 0;
            for(;i < m_currentLevelId; ++i){
                MapLevelModel model = m_mapRepository.MapTable.GetMapLevelModel(i);
                var itemState = new LevelSelectionItemViewState();
                itemState.LevelName = model.LevelName;
                itemState.IsUnlocked = true;
                itemState.LevelThumbnail = new Utilities.AddressableAssetGetter<Sprite>(model.ThumbnailAddress);
                viewState.AddItemState(itemState);
            }

            ushort totalLevel =(ushort) m_mapRepository.MapTable.AllLevelsCount;
            for(;i < totalLevel; ++i){
                MapLevelModel model = m_mapRepository.MapTable.GetMapLevelModel(i);
                var itemState = new LevelSelectionItemViewState();
                itemState.LevelName = model.LevelName;
                itemState.IsUnlocked = false;
                itemState.LevelThumbnail = new Utilities.AddressableAssetGetter<Sprite>(model.ThumbnailAddress);
                viewState.AddItemState(itemState);
            }
        }
    }
}