using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Events
{
    public sealed class SoundEventHooker : MonoBehaviour
    {
        public void PlaySound(int soundId){
            EventBus<PlaySoundEvent>.Raise(new PlaySoundEvent(soundId));
        }
    }
}
