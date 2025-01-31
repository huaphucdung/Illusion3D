
namespace Project.Screens{
    public interface ILevelSelectionDataSetter{
        void SetModel(MapLevelData model);
        void SetIsUnlocked(bool isUnlocked);
        void SetIsCompleted(bool isCompleted);
        void SetThumbnailGetter(IObservableProperty<UnityEngine.Sprite> thumbnail);
        void SetCancelCallback(System.Action onPopDetail);
        void SetStartCallback(System.Action onStart);
    }
    public sealed class LevelSelectionData : ILevelSelectionDataSetter{
        public MapLevelData Model { get; private set; }
        public bool IsUnlocked { get; private set; }
        public bool IsCompleted { get; private set; }
        public IObservableProperty<UnityEngine.Sprite> Thumbnail { get; private set; }
        public System.Action PopCommand, StartCommand;

        void ILevelSelectionDataSetter.SetIsUnlocked(bool isUnlocked) => IsUnlocked = isUnlocked;
        void ILevelSelectionDataSetter.SetIsCompleted(bool isCompleted) => IsCompleted = isCompleted;
        void ILevelSelectionDataSetter.SetModel(MapLevelData model) => Model = model;
        void ILevelSelectionDataSetter.SetThumbnailGetter(IObservableProperty<UnityEngine.Sprite> thumbnail) => Thumbnail = thumbnail;
        void ILevelSelectionDataSetter.SetCancelCallback(System.Action onPopDetail) => PopCommand = onPopDetail;
        void ILevelSelectionDataSetter.SetStartCallback(System.Action onStart) => StartCommand = onStart;
    }
}