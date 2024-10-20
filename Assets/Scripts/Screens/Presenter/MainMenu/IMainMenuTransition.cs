namespace Project.Screens{
    public interface IMainMenuTransition{
        public void ContinueGame();
        public void StartGame();
        public void OpenSettings();
        public void ExitGame();
    }

    public sealed class DebugMainMenuTransition : IMainMenuTransition{
        private readonly IMainMenuTransition m_realService;
        public DebugMainMenuTransition(IMainMenuTransition realService){
            m_realService = realService;
        }
        public void ContinueGame(){
            UnityEngine.Debug.Log("Continue game");
            m_realService.ContinueGame();
        }

        public void StartGame(){
            UnityEngine.Debug.Log("Start game");
            m_realService.StartGame();
        }

        public void OpenSettings(){
            UnityEngine.Debug.Log("Open settings");
            m_realService.OpenSettings();
        }

        public void ExitGame(){
            UnityEngine.Debug.Log("Exit game");
            m_realService.ExitGame();
        }
    }
}