using Project.Events;

namespace Project.Screens{
    public sealed class EventPauseMenuTransition : IPauseMenuTransition
    {
        public void RestartGame()
        {
            throw new System.NotImplementedException();
        }

        public void ResumeGame()
        {
            throw new System.NotImplementedException();
        }

        public void ReturnMainMenu()
        {
            EventBus<UIEvent>.Raise(new UIEvent(GameState.MainMenu));
        }
    }
}