using System.Collections;
using HuyDu_UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Screens{
    public sealed class MainMenuView : AppView<MainMenuViewState>
    {
        [SerializeField] private Button startButton;
        protected override IEnumerator InitializeInternal(MainMenuViewState state)
        {
            startButton.onClick.AddListener(state.OnStartButtonClick);
            yield break;
        }
    }
}