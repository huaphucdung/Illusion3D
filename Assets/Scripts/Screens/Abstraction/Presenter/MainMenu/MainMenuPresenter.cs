using System.Collections;
using HuyDu_UISystem;

namespace Project.Screens{
    public sealed class MainMenuPresenter : PagePresenter<MainMenuPage, MainMenuView, MainMenuViewState>
    {
        private readonly IMainMenuTransition m_transitionService;
        public MainMenuPresenter(MainMenuPage view, IMainMenuTransition transitionService) : base(view){
            m_transitionService = transitionService;
        }

        protected override IEnumerator ViewDidLoad(MainMenuPage view, MainMenuViewState viewState){
            yield return null;
            viewState.StartButtonClickEvent += m_transitionService.StartGame;
            viewState.ExitButtonClickEvent += m_transitionService.ExitGame;
        }

        protected override IEnumerator ViewWillDestroy(MainMenuPage view, MainMenuViewState viewState){
            viewState.StartButtonClickEvent -= m_transitionService.StartGame;
            viewState.ExitButtonClickEvent -= m_transitionService.ExitGame;
            yield return null;
        }
    }
}