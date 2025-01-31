using System;
using System.Collections;
using System.Collections.Generic;
using UnityScreenNavigator.Runtime.Core.Modal;
using UnityEngine;
using Project.Domain.MapLevel;

namespace Project.Screens{
    public sealed class LevelSelectionMenuUseCase : ILevelSelectionMenuUseCase{
        private readonly Domain.Player.IPlayerDataRepository m_playerRepos;
        private readonly IMapRepository m_mapRepository;
        private readonly ModalContainer m_modalContainer;
        private readonly ILevelDetailFactory m_levelDetailModalFactory;

        private readonly LevelSelectionData m_currentSelectingLevelData = new LevelSelectionData();

        [Zenject.Inject]
        public LevelSelectionMenuUseCase(
            Domain.Player.IPlayerDataRepository playerRepository,
            IMapRepository mapRepository, 
            ILevelDetailFactory levelDetailModalFactory,
            ModalContainer modalContainer) 
        {
            m_playerRepos = playerRepository;
            m_mapRepository = mapRepository;
            m_levelDetailModalFactory = levelDetailModalFactory;
            m_modalContainer = modalContainer;
        }

        public IEnumerator FetchTable() {
            System.Threading.Tasks.Task task = m_playerRepos.LoadPlayerDataAsync();
            while(!task.IsCompleted) yield return null;
            yield return m_mapRepository.FetchMapTable();
        }

        public IEnumerable<LevelSelectionData> GetAllLevels()
        {
            LevelSelectionData cache = new LevelSelectionData();
            ILevelSelectionDataSetter setter = cache;

            Domain.Player.PlayerData playerData = m_playerRepos.PlayerData;
            for (int i = 0; i < m_mapRepository.MapTable.AllLevelsCount; ++i){
                MapLevelModel model = m_mapRepository.MapTable.GetMapLevelModel((ushort)i);

                setter.SetModel(ConvertFrom(model));
                setter.SetIsUnlocked(i < playerData.CurrentLevel);
                //TODO: get player data to check if level is completed
                setter.SetIsCompleted(false);
                yield return cache;
            }

        }

        private MapLevelData ConvertFrom(MapLevelModel model){
            return new MapLevelData(model.LevelId, model.LevelName, model.ThumbnailAddress);
        }

        public void ShowLevelDetail(ushort levelId) {
            ILevelSelectionDataSetter setter = m_currentSelectingLevelData;

            Domain.Player.PlayerData playerData = m_playerRepos.PlayerData;
            MapLevelModel model = m_mapRepository.MapTable.GetMapLevelModel(levelId);
            setter.SetModel(ConvertFrom(model));
            setter.SetIsUnlocked(levelId < playerData.CurrentLevel);
            setter.SetThumbnailGetter(CreateObservableThumbnail(model.ThumbnailAddress));
            setter.SetStartCallback(StartLevel);
            setter.SetCancelCallback(PopDetail);
            
            m_levelDetailModalFactory.PushModalTo(m_modalContainer, m_currentSelectingLevelData, playAnimation: true);
        }

        private void StartLevel()
        {
            //TODO: Start Level
            Debug.Log($"Start level: {m_currentSelectingLevelData.Model.LevelName}");
        }

        private void PopDetail()
        {
            m_modalContainer.Pop(playAnimation: true);
        }

        public IObservableProperty<Sprite> CreateObservableThumbnail(string address)
        {
            var property = new AddressableObservableProperty<Sprite>(address);
            AddressableReferenceContainer<Sprite>.AddReference(property);
            return property;
        }
    }
}