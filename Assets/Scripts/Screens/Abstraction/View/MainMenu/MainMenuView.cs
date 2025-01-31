using System.Collections;
using HuyDu_UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Screens{
    public sealed class MainMenuView : AppView<MainMenuViewState>
    {
        [SerializeField] Button startButton;
        [SerializeField] Button exitButton;
        protected override IEnumerator InitializeInternal(MainMenuViewState state)
        {
            var internalState = (IMainMenuInternalState)state;
            startButton.onClick.AddListener(internalState.InvokeStartEvent);
            exitButton.onClick.AddListener(internalState.InvokeExitEvent);
            yield break;
        }
    }
}