namespace Project.Screens.Testing{
    internal sealed class TestPauseMenuTransition : IPauseMenuTransition
    {
        private readonly ApplicationRoot m_root;
        public TestPauseMenuTransition(ApplicationRoot root) => m_root = root;
        public void RestartGame()
        {
        }

        public void ResumeGame()
        {
        }

        public void ReturnMainMenu()
        {
            m_root.ReturnToMainMenu();
        }
    }
}