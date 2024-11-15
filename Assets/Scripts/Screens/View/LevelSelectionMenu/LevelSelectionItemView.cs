using System;
using System.Collections;
using HuyDu_UISystem;
using Project.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Screens
{
    public sealed class LevelSelectionItemView : AppView<LevelSelectionItemViewState>
    {
        [SerializeField] Image m_thumbnail;
        [SerializeField] TextMeshProUGUI m_levelName;
        [SerializeField] Animation m_animation;
        [SerializeField] Animation m_completeAnimation;
        [SerializeField] Button m_button;

        private bool m_unlockFlag;
        protected override IEnumerator InitializeInternal(LevelSelectionItemViewState state)
        {
            m_levelName.text = state.LevelName;
            m_unlockFlag = state.IsUnlocked;
            SetupThumbnail(state.LevelThumbnail);

            ILevelSelectionItemViewState internalState = state;
            internalState.LevelLockChangeEvent += OnUnlockFlagChanged;
            m_button.onClick.AddListener(internalState.InvokeButtonClick);

            if (m_unlockFlag == true)
            {
                ToUnlockedStateInstantly(m_animation);
            }

            if(state.Completed == true){
                ToUnlockedStateInstantly(m_completeAnimation);
            }
            yield break;
        }

        private void ToUnlockedStateInstantly(Animation animation)
        {
            AnimationClip clip = animation.clip;
            AnimationState state = animation[clip.name];
            state.time = clip.length;
            animation.Play();
        }

        void OnUnlockFlagChanged(bool isUnlocked)
        {
            if (isUnlocked == true && m_unlockFlag == false)
            {
                StartCoroutine(PlayUnlockAnimation());
            }
            m_unlockFlag = isUnlocked;
        }

        private IEnumerator PlayUnlockAnimation()
        {
            m_animation.Play();
            while (m_animation.isPlaying) yield return null;
        }

        private async void SetupThumbnail(IAssetGetter<Sprite> levelThumbnail)
        {
            m_thumbnail.sprite = await levelThumbnail.GetAsync();
        }
    }
}