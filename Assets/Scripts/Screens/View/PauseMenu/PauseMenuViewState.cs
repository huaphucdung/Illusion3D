using HuyDu_UISystem;
namespace Project.Screens
{
    public sealed class PauseMenuViewState : AppViewState, IPauseMenuInternalState
    {
        public event System.Action ReturnMainMenuEvent;
        public event System.Action ResumeGameEvent;
        public event System.Action RestartGameEvent;

        protected override void DisposeInternal()
        {
            ReturnMainMenuEvent = null;
            ResumeGameEvent = null;
            RestartGameEvent = null;
        }

        void IPauseMenuInternalState.InvokeReturnMainMenuEvent(){
            ReturnMainMenuEvent?.Invoke();
        }

        void IPauseMenuInternalState.InvokeResumeGameEvent(){
            ResumeGameEvent?.Invoke();
        }

        void IPauseMenuInternalState.InvokeRestartGameEvent(){
            RestartGameEvent?.Invoke();
        }
    }

    internal interface IPauseMenuInternalState
    {
        void InvokeReturnMainMenuEvent();
        void InvokeResumeGameEvent();
        void InvokeRestartGameEvent();
    }
}
