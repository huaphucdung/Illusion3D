using System.Collections;

namespace Project.Domain.MapLevel
{
    public class LocalMapLevelRepository : IMapRepository
    {
        private readonly IMapTable m_mapTable;
        public IMapTable MapTable => m_mapTable;

        public LocalMapLevelRepository(IMapTable mapTable){
            m_mapTable = mapTable;
        }

        public void Dispose()
        {
            m_mapTable.CleanUp();
        }

        public IEnumerator FetchMapTable()
        {
            return MapTable.Initialize();
        }
    }
}
