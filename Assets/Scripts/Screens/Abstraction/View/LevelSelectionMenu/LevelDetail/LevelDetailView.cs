using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Screens{
    public sealed class LevelDetailView : HuyDu_UISystem.AppView<LevelDetailViewState>
    {
        [SerializeField] Image levelThumbnail;
        [SerializeField] TextMeshProUGUI levelName; 
        [SerializeField] TextMeshProUGUI levelDescription;
        [SerializeField] Button playButton, backButton;
        protected override IEnumerator InitializeInternal(LevelDetailViewState state)
        {
            state.LevelThumbnailGetter.AddObserver(SetThumbnail).AddTo(gameObject);
            levelName.text = state.LevelName;
            // levelDescription.text = state.LevelDescription;

            
            if(backButton !=null){
                backButton.onClick.AddListener(((ILevelDetailViewState)state).InvokeBackClick);
            }
            if(state.IsLocked == false){
                playButton.onClick.AddListener(((ILevelDetailViewState)state).InvokeStartLevel);
            }
            else{
                playButton.gameObject.SetActive(false);
            }

            yield break;
        }

        void SetThumbnail(Sprite sprite){
            levelThumbnail.sprite = sprite;
        }
    }
}