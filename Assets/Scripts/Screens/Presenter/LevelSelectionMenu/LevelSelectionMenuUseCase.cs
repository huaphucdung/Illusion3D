using System;
using System.Collections;
using System.Collections.Generic;
using UnityScreenNavigator.Runtime.Core.Modal;
using Project.Domain.MapLevel;

namespace Project.Screens{
    internal interface ILevelSelectionDataSetter{
        void SetModel(MapLevelModel model);
        void SetIsUnlocked(bool isUnlocked);
    }
    public sealed class LevelSelectionData : ILevelSelectionDataSetter{
        public MapLevelModel Model { get; private set; }
        public bool IsUnlocked { get; private set; }

        void ILevelSelectionDataSetter.SetIsUnlocked(bool isUnlocked) => IsUnlocked = isUnlocked;

        void ILevelSelectionDataSetter.SetModel(MapLevelModel model) => Model = model;
    }
    public interface ILevelSelectionMenuUseCase{
        IEnumerator FetchTable();
        IEnumerable<LevelSelectionData> GetAllLevels();
        void ShowLevelDetail(ushort id);
    } 
    public sealed class LevelSelectionMenuUseCase : ILevelSelectionMenuUseCase{
        private readonly Domain.Player.IPlayerDataRepository m_playerRepos;
        private readonly IMapRepository m_mapRepository;
        private readonly ModalContainer m_modalContainer;
        private readonly LevelDetailFactory m_levelDetailModalFactory;

        public LevelSelectionMenuUseCase(
            Domain.Player.IPlayerDataRepository playerRepository,
            IMapRepository mapRepository, 
            LevelDetailFactory levelDetailModalFactory,
            ModalContainer modalContainer) 
        {
            m_playerRepos = playerRepository ?? throw new ArgumentNullException(nameof(playerRepository));
            m_mapRepository = mapRepository ?? throw new ArgumentNullException(nameof(mapRepository));
            m_levelDetailModalFactory = levelDetailModalFactory ?? throw new ArgumentNullException(nameof(levelDetailModalFactory));
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

                setter.SetModel(model);
                setter.SetIsUnlocked(i < playerData.CurrentLevel);
                yield return cache;
            }

        }

        public void ShowLevelDetail(ushort levelId) {
            LevelSelectionData cache = new LevelSelectionData();
            ILevelSelectionDataSetter setter = cache;
            Domain.Player.PlayerData playerData = m_playerRepos.PlayerData;
            MapLevelModel model = m_mapRepository.MapTable.GetMapLevelModel(levelId);
            setter.SetModel(model);
            setter.SetIsUnlocked(levelId < playerData.CurrentLevel);
            
            m_levelDetailModalFactory.PushModalTo(m_modalContainer, cache, playAnimation: true);
        }
    }
}