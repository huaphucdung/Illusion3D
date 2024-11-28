using System;
using System.Collections;
using Project.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Screens{
    public sealed class LevelDetailView : HuyDu_UISystem.AppView<LevelDetailViewState>
    {
        [SerializeField] Image levelThumbnail;
        [SerializeField] TextMeshProUGUI levelName; 
        [SerializeField] TextMeshProUGUI levelDescription;
        [SerializeField] Button playButton;
        protected override IEnumerator InitializeInternal(LevelDetailViewState state)
        {
            SetThumbnail(state.LevelThumbnailGetter);
            levelName.text = state.LevelName;
            // levelDescription.text = state.LevelDescription;

            if(state.IsLocked == false){
                playButton.onClick.AddListener(((ILevelDetailViewState)state).InvokeStartLevel);
            }
            else{
                playButton.gameObject.SetActive(false);
            }

            yield break;
        }

        private async void SetThumbnail(IAssetGetter<Sprite> levelThumbnailGetter)
        {
            levelThumbnail.sprite = await levelThumbnailGetter.GetAsync();
        }
    }
}