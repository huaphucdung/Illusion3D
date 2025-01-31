namespace Project.Screens{
    public interface ILevelSelectionMenuUseCase{
        System.Collections.IEnumerator FetchTable();
        System.Collections.Generic.IEnumerable<LevelSelectionData> GetAllLevels();
        void ShowLevelDetail(ushort id);

        IObservableProperty<UnityEngine.Sprite> CreateObservableThumbnail(string address);
    } 
}