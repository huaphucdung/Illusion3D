using UnityEngine;

namespace Project.Audio
{
    public sealed class SimpleAudioService : IAudioService
    {
        private readonly AudioSource m_audioSource;
        private readonly AudioClip m_audioClip;
        public SimpleAudioService(AudioSource audioSource, AudioClip audioClip){
            m_audioSource = audioSource;
            m_audioClip = audioClip;
        }
        public void PlaySound(int soundId)
        {
            m_audioSource.PlayOneShot(m_audioClip);
        }
    }
}