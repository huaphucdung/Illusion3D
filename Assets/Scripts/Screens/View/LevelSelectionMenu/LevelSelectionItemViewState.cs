using System;
using Project.Utilities;
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

        public IAssetGetter<Sprite> LevelThumbnail;
        public string LevelName;

        private bool m_unlockFlag;
        public bool IsUnlocked {
            get => m_unlockFlag;
            set{
                if(m_unlockFlag == value) return;
                m_unlockFlag = value;
                m_levelLockChangeEvent?.Invoke(m_unlockFlag);
            }
        }
        
        protected override void DisposeInternal()
        {
            LevelThumbnail.Dispose();
            ClickEvent = null;
        }

        void ILevelSelectionItemViewState.InvokeButtonClick(){
            ClickEvent?.Invoke();
        }
    }
    
    internal interface ILevelSelectionItemViewState{
        event System.Action<bool> LevelLockChangeEvent;
        void InvokeButtonClick();
    }
}