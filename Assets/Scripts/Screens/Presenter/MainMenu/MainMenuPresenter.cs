using System.Collections;
using HuyDu_UISystem;

namespace Project.Screens{
    public sealed class MainMenuPresenter : PagePresenter<MainMenuPage, MainMenuView, MainMenuViewState>
    {
        public MainMenuPresenter(MainMenuPage view) : base(view){}

        protected override IEnumerator ViewDidLoad(MainMenuPage view, MainMenuViewState viewState){
            yield return null;
            viewState.StartButtonClickEvent += OnStartButtonClick;
        }

        protected override IEnumerator ViewWillDestroy(MainMenuPage view, MainMenuViewState viewState){
            viewState.StartButtonClickEvent -= OnStartButtonClick;
            yield return null;
        }

        private void OnStartButtonClick(){
            // TODO: transition to level selection
            UnityEngine.Debug.Log("Go to level selection");
        }
    }
}