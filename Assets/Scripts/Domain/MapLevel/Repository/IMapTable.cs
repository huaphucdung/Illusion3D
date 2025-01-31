using System.Collections;

namespace Project.Domain.MapLevel{
    public interface IMapTable{
        IEnumerator Initialize();
        void CleanUp();
        MapLevelModel GetMapLevelModel(ushort id);
        int AllLevelsCount { get; }
    }
}