
namespace Project.Screens{

    public interface IPauseMenuTransition{
        void ReturnMainMenu();
        void ResumeGame();
        void RestartGame();
    }

    internal sealed class DebugPauseMenuTransition : IPauseMenuTransition{
        private readonly IPauseMenuTransition m_realService;

        public DebugPauseMenuTransition(IPauseMenuTransition realService){
            m_realService = realService;
        }

        public void ReturnMainMenu(){
            UnityEngine.Debug.Log("Return main menu");
            m_realService.ReturnMainMenu();
        }

        public void ResumeGame(){
            UnityEngine.Debug.Log("Resume game");
            m_realService.ResumeGame();
        }

        public void RestartGame(){
            UnityEngine.Debug.Log("Restart game");
            m_realService.RestartGame();
        }
    }
}
