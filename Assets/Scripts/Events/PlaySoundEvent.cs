namespace Project.Events{
    public readonly struct PlaySoundEvent : IEvent
    {
        public readonly int SoundId;

        public PlaySoundEvent(int soundId){
            SoundId = soundId;
        }
    }
}