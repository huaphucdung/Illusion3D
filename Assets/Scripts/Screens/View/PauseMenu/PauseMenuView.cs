using System.Collections;
using HuyDu_UISystem;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Screens
{
    public sealed class PauseMenuView : AppView<PauseMenuViewState>
    {
        [SerializeField] private Button returnMainMenuButton;
        [SerializeField] private Button restartGameButton;
        protected override IEnumerator InitializeInternal(PauseMenuViewState state)
        {
            var internalState = (IPauseMenuInternalState)state;
            returnMainMenuButton.onClick.AddListener(internalState.InvokeReturnMainMenuEvent);
            restartGameButton.onClick.AddListener(internalState.InvokeRestartGameEvent);
            yield break;
        }
    }
}
