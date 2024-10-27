namespace Project.Domain.MapLevel{
    public sealed class MapLevelItem{
        public event System.Action<bool> LockChanged;

        public readonly ushort Id;
        public bool IsLocked {get; private set;}
        
        internal void SetLock(bool isLocked) => IsLocked = isLocked;

        public MapLevelItem(ushort id, bool isLocked){
            Id = id;
            IsLocked = isLocked;
        }
    }
}