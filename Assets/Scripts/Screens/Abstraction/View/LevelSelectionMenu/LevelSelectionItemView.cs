using System;
using System.Collections;
using HuyDu_UISystem;
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
        [SerializeField] GameObject m_completeMark;

        private bool m_unlockFlag;
        protected override IEnumerator InitializeInternal(LevelSelectionItemViewState state)
        {
            m_levelName.text = state.LevelName;
            m_unlockFlag = state.IsUnlocked;
            m_completeMark.SetActive(state.Completed);
            
            ILevelSelectionItemViewState internalState = state;
            internalState.RegisterThumbnailLoad(SetupThumbnail).AddTo(gameObject);
            internalState.LevelLockChangeEvent += OnUnlockFlagChanged;
            m_button.onClick.AddListener(internalState.InvokeButtonClick);

            if (m_unlockFlag == true)
            {
                SkipToUnlockedState(m_animation);
            }

            if(state.Completed == true){
                SkipToUnlockedState(m_completeAnimation);
            }
            yield break;
        }

        private void SkipToUnlockedState(Animation animation)
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

        private void SetupThumbnail(Sprite levelThumbnail)
        {
            m_thumbnail.sprite = levelThumbnail;
        }
    }
}