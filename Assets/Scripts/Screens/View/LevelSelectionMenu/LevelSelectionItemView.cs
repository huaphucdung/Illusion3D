using System;
using System.Collections;
using HuyDu_UISystem;
using Project.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Screens{
    public sealed class LevelSelectionItemView : AppView<LevelSelectionItemViewState>
    {
        [SerializeField] Image m_thumbnail;
        [SerializeField] TextMeshProUGUI m_levelName;
        [SerializeField] Animation m_animation;

        private bool m_unlockFlag;
        protected override IEnumerator InitializeInternal(LevelSelectionItemViewState state)
        {
            m_levelName.text = state.LevelName;
            m_unlockFlag = state.IsUnlocked;
            SetupThumbnail(state.LevelThumbnail);
            yield break;
        }

        internal void CheckUnlockFlag()
        {
            if(m_unlockFlag == true){
                StartCoroutine(PlayUnlockAnimation());
                m_unlockFlag = false;
            }
        }

        private IEnumerator PlayUnlockAnimation(){
            m_animation.Play();
            while(m_animation.isPlaying) yield return null;
        }

        private async void SetupThumbnail(IAssetGetter<Sprite> levelThumbnail)
        {
            m_thumbnail.sprite = await levelThumbnail.GetAsync();
        }
    }
}