namespace Project.Screens
{
    public sealed class MainMenuViewState : HuyDu_UISystem.AppViewState
    {
        public event System.Action StartButtonClickEvent;
        protected override void DisposeInternal()
        {
            StartButtonClickEvent = null;
        }

        public void OnStartButtonClick(){
            StartButtonClickEvent?.Invoke();
        }
    }
}