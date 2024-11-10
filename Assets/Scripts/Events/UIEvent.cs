namespace Project.Events{
    public readonly struct UIEvent : IEvent
    {
        public readonly GameState GameState;

        public UIEvent(GameState state) => GameState = state;
    }

    [System.Serializable]
    public struct SerializableUIEvent : IEventWrapper
    {
        [UnityEngine.SerializeField] GameState state;
        public readonly IEvent Event => new UIEvent(state);
    }

    public enum GameState{
        MainMenu, GamePlay, Pause, Settings, LevelSelection
    }
}