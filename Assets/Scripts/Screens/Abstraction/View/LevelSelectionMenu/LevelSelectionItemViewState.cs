using System;
using UnityEngine;

namespace Project.Screens
{
    public sealed class LevelSelectionItemViewState : HuyDu_UISystem.AppViewState, ILevelSelectionItemViewState
    {
        public event Action ClickEvent;
        private event Action<bool> m_levelLockChangeEvent;
         event Action<bool> ILevelSelectionItemViewState.LevelLockChangeEvent{
            add => m_levelLockChangeEvent += value;
            remove => m_levelLockChangeEvent -= value;
        }

        public IObservableProperty<Sprite> LevelThumbnail;
        public string LevelName;
        public bool Completed = true;

        private bool m_unlockFlag;
        public bool IsUnlocked {
            get => m_unlockFlag;
            set{
                m_unlockFlag = value;
                m_levelLockChangeEvent?.Invoke(m_unlockFlag);
            }
        }
        
        protected override void DisposeInternal()
        {
            ClickEvent = null;
        }

        void ILevelSelectionItemViewState.InvokeButtonClick(){
            ClickEvent?.Invoke();
        }

        IDisposable ILevelSelectionItemViewState.RegisterThumbnailLoad(Action<Sprite> callback)
        {
            if(LevelThumbnail.Value != null) callback(LevelThumbnail.Value);
            return LevelThumbnail.AddObserver(callback);
        }
    }
    
    internal interface ILevelSelectionItemViewState{
        event System.Action<bool> LevelLockChangeEvent;
        void InvokeButtonClick();
        IDisposable RegisterThumbnailLoad(Action<Sprite> callback);
    }
}