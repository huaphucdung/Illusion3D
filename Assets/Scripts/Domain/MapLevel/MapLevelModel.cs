using Eflatun.SceneReference;
using UnityEngine;


namespace Project.Domain.MapLevel{
    [System.Serializable]
    public sealed class MapLevelModel{
        [SerializeField] ushort levelId;
        [SerializeField] string name;
        [SerializeField] SceneReference sceneReference;
        [SerializeField] string thumbnailAddress;

        public ushort LevelId => levelId;
        public string LevelName => name; 
        public string SceneName => sceneReference.Name;
        public string SceneAddress => sceneReference.Address;
        public string ThumbnailAddress => thumbnailAddress;
    }
}