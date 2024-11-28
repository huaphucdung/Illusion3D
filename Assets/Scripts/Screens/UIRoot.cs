using System;
using Project.Events;

namespace Project.Screens{
    public sealed class UIRoot : IDisposable
    {
        private IEventBinding<UIEvent> m_eventBinding;
        private IEventBinding<PopUIEvent> m_popEventBinding;
        private IUIScreenService m_screenService;
        private UIRoot(){}

        public void Dispose()
        {
            UnregisterEvents();
        }

        private void Initialize(){
            RegisterEvents();
        }

        private void RegisterEvents(){
            m_eventBinding.OnEvent += OnUIEventRaised;
            m_popEventBinding.OnEvent += OnPopUIEventRaised;
        }

        private void UnregisterEvents(){
            m_eventBinding.OnEvent -= OnUIEventRaised;
            m_popEventBinding.OnEvent -= OnPopUIEventRaised;
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


        #region Builder
        public sealed class Builder{
            private readonly UIRoot m_instance;
            private Builder(){
                m_instance = new UIRoot();
                m_instance.m_eventBinding = EventBinding<UIEvent>.Empty;
                m_instance.m_popEventBinding = EventBinding<PopUIEvent>.Empty;
                m_instance.m_screenService = NullUIScreenService.NullService;
            }
            public static Builder New() => new Builder();
            public Builder WithEventBinding(IEventBinding<UIEvent> binding){
                m_instance.m_eventBinding = binding ?? EventBinding<UIEvent>.Empty;
                return this;
            }
            public Builder WithPopEventBinding(IEventBinding<PopUIEvent> binding){
                m_instance.m_popEventBinding = binding ?? EventBinding<PopUIEvent>.Empty;
                return this;
            }
            public Builder WithScreenService(IUIScreenService service){
                m_instance.m_screenService = service ?? NullUIScreenService.NullService;
                return this;
            }

            public UIRoot GetRootInstance(){
                m_instance.Initialize();
                return m_instance;
            }
        }
#endregion
    }
}