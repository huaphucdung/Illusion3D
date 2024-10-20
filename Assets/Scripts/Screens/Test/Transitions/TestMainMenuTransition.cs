namespace Project.Screens.Testing{
    internal sealed class TestMainMenuTransition : IMainMenuTransition
    {
        private readonly ApplicationRoot m_root;
        public TestMainMenuTransition(ApplicationRoot root){
            m_root = root;
        }
        public void ContinueGame()
        {
            throw new System.NotImplementedException();
        }

        public void ExitGame()
        {
            m_root.ExitGame();
        }

        public void OpenSettings()
        {
            throw new System.NotImplementedException();
        }

        public void StartGame()
        {
            m_root.StartGame();
        }
    }
}