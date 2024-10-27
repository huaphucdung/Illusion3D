using System.Collections.Generic;

namespace Project.Domain.MapLevel{
    public sealed class MapLevelSet{
        public readonly IReadOnlyCollection<MapLevelItem> Items;

        public MapLevelSet(MapLevelItem[] items) => Items = items;
    }
}