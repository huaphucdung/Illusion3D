using System;
using Project.Events;

namespace Project.Screens{
    public sealed class UIRoot : IDisposable
    {
        private readonly IDisposable m_eventBinding, m_popEventBinding;
        private readonly IUIScreenService m_screenService;

        [Zenject.Inject]
        public UIRoot(IUIScreenService service){
            m_screenService = service;
            m_eventBinding = new EventBinding<UIEvent>(OnUIEventRaised).RegisterToBus();
            m_popEventBinding = new EventBinding<PopUIEvent>(OnPopUIEventRaised).RegisterToBus();
        }

        public void Dispose()
        {
            m_eventBinding.Dispose();
            m_popEventBinding.Dispose();
        }

        private void OnPopUIEventRaised(PopUIEvent @event){
            if(@event.PopType == PopUIEventType.Page){
                m_screenService.PopScreen();
            }
        }
        private void OnUIEventRaised(UIEvent @event)
        {
            var type = @event.GameState switch
            {
                GameState.GamePlay => ScreenType.NotSupported,
                GameState.Pause => ScreenType.PauseMenu,
                GameState.MainMenu => ScreenType.MainMenu,
                GameState.LevelSelection => ScreenType.LevelSelectionMenu,
                _ => ScreenType.NotSupported,
            };
            m_screenService.ShowScreen(type);
        }
    }
}