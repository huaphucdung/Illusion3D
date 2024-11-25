using System;

namespace Project.Events{
    public readonly struct PopUIEvent : IEvent{
        public readonly PopUIEventType PopType;
        public readonly int PopCount;
        public PopUIEvent(PopUIEventType type, int count = 1) {
            PopType = type;
            PopCount = count;
        }
    }

    [System.Serializable]
    public struct SerializablePopUIEvent : IEventWrapper
    {
        [UnityEngine.SerializeField] PopUIEventType PopType;
        [UnityEngine.SerializeField] int PopCount;
        public readonly IEvent Event => new PopUIEvent(PopType, PopCount);
    }

    public enum PopUIEventType{
        Page, Modal, Sheet
    }
}