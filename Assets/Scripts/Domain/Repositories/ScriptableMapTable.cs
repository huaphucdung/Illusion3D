using System.Collections;
using System.Collections.Generic;

namespace Project.Domain.MapLevel{
    [UnityEngine.CreateAssetMenu(fileName = "MapTable", menuName = "Database/MapTable")]
    public class ScriptableMapTable : UnityEngine.ScriptableObject, IMapTable
    {
        [UnityEngine.SerializeField] private MapLevelModel[] m_mapLevels;
        private readonly Dictionary<ushort, MapLevelModel> m_mapLevelsById = new Dictionary<ushort, MapLevelModel>();
        public int AllLevelsCount => m_mapLevels.Length;

        public void CleanUp()
        {
            m_mapLevelsById.Clear();
        }

        public MapLevelModel GetMapLevelModel(ushort id)
        {
            return m_mapLevelsById[id];
        }

        public IEnumerator Initialize()
        {
            for(int i = 0; i < m_mapLevels.Length; i++){
                m_mapLevelsById.Add(m_mapLevels[i].LevelId, m_mapLevels[i]);
            }
            yield break;
        }
    }
}