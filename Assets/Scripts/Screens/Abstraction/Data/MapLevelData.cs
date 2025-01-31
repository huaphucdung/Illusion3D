namespace Project.Screens{
    public readonly struct MapLevelData
    {
        public readonly ushort LevelId ;
        public readonly string LevelName ;
        public readonly string ThumbnailAddress ;

        public MapLevelData(ushort levelId, string levelName, string thumbnailAddress) => (LevelId, LevelName, ThumbnailAddress) = (levelId, levelName, thumbnailAddress);
    }
}