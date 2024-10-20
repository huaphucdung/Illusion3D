namespace Project.Screens
{
    public sealed class MainMenuViewState : HuyDu_UISystem.AppViewState, IMainMenuInternalState
    {
        public event System.Action StartButtonClickEvent;
        public event System.Action ExitButtonClickEvent;
        protected override void DisposeInternal()
        {
            StartButtonClickEvent = null;
            ExitButtonClickEvent = null;
        }

        void IMainMenuInternalState.InvokeStartEvent(){
            StartButtonClickEvent?.Invoke();
        }

        void IMainMenuInternalState.InvokeExitEvent(){
            ExitButtonClickEvent?.Invoke();
        }
    }

    internal interface IMainMenuInternalState{
        void InvokeStartEvent();
        void InvokeExitEvent();
    }
}