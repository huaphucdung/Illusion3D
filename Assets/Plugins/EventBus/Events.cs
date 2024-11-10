public interface IEvent { }

public struct ResetEvent : IEvent { }

public interface IEventWrapper {
    IEvent Event { get; }
}