namespace Project.Screens{
    public enum ScreenType{
        NotSupported,
        MainMenu,
        LevelSelectionMenu,
        PauseMenu
    }
    public interface IUIScreenService{
        void ShowScreen(ScreenType type);
        void PopScreen(int count = 1);
    }

    public sealed class NullUIScreenService : IUIScreenService{
        public readonly static NullUIScreenService NullService = new NullUIScreenService();
        private NullUIScreenService() { }
        public void PopScreen(int count = 1){}
        public void ShowScreen(ScreenType type){}
    }
}