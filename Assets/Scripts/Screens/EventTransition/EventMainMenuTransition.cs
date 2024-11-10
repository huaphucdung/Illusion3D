using Project.Events;

namespace Project.Screens{
    public sealed class EventMainMenuTransition : IMainMenuTransition
    {
        public void ContinueGame()
        {
            throw new System.NotImplementedException();
        }

        public void ExitGame()
        {
            throw new System.NotImplementedException();
        }

        public void OpenSettings()
        {
            EventBus<UIEvent>.Raise(new UIEvent(GameState.Settings));
        }

        public void StartGame()
        {
             EventBus<UIEvent>.Raise(new UIEvent(GameState.LevelSelection));
        }
    }
}