using System;
using UnityEngine;

namespace Project.Screens{
    public sealed class LevelDetailViewState : HuyDu_UISystem.AppViewState, ILevelDetailViewState
    {
        public event System.Action StartLevelEvent, BackButtonClickEvent;
        public IObservableProperty<UnityEngine.Sprite> LevelThumbnailGetter;
        internal bool IsLocked;
        internal string LevelName;
        internal string LevelDescription;

        protected override void DisposeInternal()
        {
            StartLevelEvent = null;
            BackButtonClickEvent = null;
        }

        void ILevelDetailViewState.InvokeStartLevel(){
            StartLevelEvent?.Invoke();
        }


        void ILevelDetailViewState.InvokeBackClick(){
            BackButtonClickEvent?.Invoke();
        }

        IDisposable ILevelDetailViewState.AddObserverThumbnail(Action<Sprite> callback)
        {
            callback.Invoke(LevelThumbnailGetter.Value);
            return LevelThumbnailGetter.AddObserver(callback);
        }
    }

    internal interface ILevelDetailViewState {
        void InvokeStartLevel();
        void InvokeBackClick();
        System.IDisposable AddObserverThumbnail(System.Action<UnityEngine.Sprite> callback);
    }
}