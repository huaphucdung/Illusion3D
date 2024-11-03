using System;
using Project.Events;

namespace Project.Audio{
    public sealed class AudioEventHandler : System.IDisposable{
        private readonly IAudioService m_audioService;
        private readonly IEventBinding<PlaySoundEvent> m_playSoundEventBinding;

        public AudioEventHandler(IAudioService audioService, IEventBinding<PlaySoundEvent> playSoundEventBinding){
            m_audioService = audioService;
            m_playSoundEventBinding = playSoundEventBinding;
            m_playSoundEventBinding.OnEvent += OnPlaySoundEvent;
        }

        private void OnPlaySoundEvent(PlaySoundEvent @event)
        {
            m_audioService.PlaySound(@event.SoundId);
        }

        public void Dispose()
        {
            m_playSoundEventBinding.OnEvent -= OnPlaySoundEvent;
        }
    }
}