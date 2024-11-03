using System.Collections;
using System.Collections.Generic;
using Project.Domain.MapLevel;
using UnityEngine;
namespace Project.Repository{

    [CreateAssetMenu(fileName = "MapTable", menuName = "Database/MapTable")]
    public sealed class ScriptableMapTable : ScriptableObject, IMapTable
    {
        [SerializeField] MapLevelModel[] mapLevelModels;
        private readonly Dictionary<ushort, MapLevelModel> m_mapLevelModelsDictionary = new Dictionary<ushort, MapLevelModel>();

        public int AllLevelsCount => mapLevelModels.Length;

        public IEnumerator Initialize(){
            m_mapLevelModelsDictionary.Clear();

            foreach (MapLevelModel mapLevelModel in mapLevelModels){
                m_mapLevelModelsDictionary.Add(mapLevelModel.LevelId, mapLevelModel);
            }
            yield break;

        }
        public MapLevelModel GetMapLevelModel(ushort id)
        {
            m_mapLevelModelsDictionary.TryGetValue(id, out var mapLevelModel);
            return mapLevelModel;
        }
    }
}