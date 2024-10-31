using Eflatun.SceneReference;
using UnityEngine;


namespace Project.Domain.MapLevel{
    [System.Serializable]
    public sealed class MapLevelModel{
        [SerializeField] ushort levelId;
        [SerializeField] SceneReference sceneReference;
        [SerializeField] bool isLocked;

        public ushort LevelId => levelId; 
        public string SceneName => sceneReference.Name;
        public string SceneAddress => sceneReference.Address;
        public bool IsLocked => isLocked;
    }
}