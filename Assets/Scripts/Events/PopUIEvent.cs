using System;

namespace Project.Events{
    public readonly struct PopUIEvent : IEvent{
        public readonly PopUIEventType PopType;
        public PopUIEvent(PopUIEventType type) => PopType = type;
    }

    [System.Serializable]
    public struct SerializablePopUIEvent : IEventWrapper
    {
        [UnityEngine.SerializeField] PopUIEventType PopType;
        public readonly IEvent Event => new PopUIEvent(PopType);
    }

    public enum PopUIEventType{
        Page, Modal, Sheet
    }
}