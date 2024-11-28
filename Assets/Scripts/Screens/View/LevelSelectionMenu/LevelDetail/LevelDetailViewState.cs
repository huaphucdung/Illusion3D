namespace Project.Screens{
    public sealed class LevelDetailViewState : HuyDu_UISystem.AppViewState, ILevelDetailViewState
    {
        public event System.Action StartLevelEvent;
        public Utilities.IAssetGetter<UnityEngine.Sprite> LevelThumbnailGetter;
        internal bool IsLocked;
        internal string LevelName;
        internal string LevelDescription;

        protected override void DisposeInternal()
        {
            LevelThumbnailGetter.Dispose();
            StartLevelEvent = null;
        }

        void ILevelDetailViewState.InvokeStartLevel(){
            StartLevelEvent?.Invoke();
        }
    }

    internal interface ILevelDetailViewState {
        void InvokeStartLevel();
    }
}