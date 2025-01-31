using System.Collections;
using HuyDu_UISystem;

namespace Project.Screens
{
    public sealed class PauseMenuPresenter : PagePresenter<PauseMenuPage, PauseMenuView, PauseMenuViewState>
    {
        private readonly IPauseMenuTransition m_transitionService;
        public PauseMenuPresenter(PauseMenuPage view, IPauseMenuTransition transitionService) : base(view)
        {
            m_transitionService = transitionService;
        }

        protected override IEnumerator ViewDidLoad(PauseMenuPage view, PauseMenuViewState viewState)
        {
            yield return null;
            viewState.ResumeGameEvent += m_transitionService.ResumeGame;
            viewState.RestartGameEvent += m_transitionService.RestartGame;
            viewState.ReturnMainMenuEvent += m_transitionService.ReturnMainMenu;
        }

        protected override IEnumerator ViewWillDestroy(PauseMenuPage view, PauseMenuViewState viewState)
        {
            viewState.ResumeGameEvent -= m_transitionService.ResumeGame;
            viewState.RestartGameEvent -= m_transitionService.RestartGame;
            viewState.ReturnMainMenuEvent -= m_transitionService.ReturnMainMenu;
            yield return null;
        }
    }
}
