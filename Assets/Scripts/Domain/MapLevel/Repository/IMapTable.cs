using System.Collections;

namespace Project.Domain.MapLevel{
    public interface IMapTable{
        IEnumerator Initialize();
        public MapLevelModel GetMapLevelModel(ushort id);
    }
}