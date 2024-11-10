using System.Collections.Generic;
using UnityEngine;

public static class EventBus<T> where T : IEvent {
    static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();
    
    public static void Register(IEventBinding<T> binding) => bindings.Add(binding);
    public static void Deregister(IEventBinding<T> binding) => bindings.Remove(binding);

    public static void Raise(IEvent @event) => Raise((T)@event);

    public static void Raise(T @event) {

        // xài snapshot cho multi-threading thôi: tại k thể register/deregister trong lúc raise
        // nên ông idol mới xài snapshot
        // mốt chắc xài lock chứ k copy nguyên cái hashset
        // var snapshot = new HashSet<IEventBinding<T>>(bindings);

        foreach (var binding in bindings) {
            if (bindings.Contains(binding)) {
                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
        }
    }

    static void Clear() {
        Debug.Log($"Clearing {typeof(T).Name} bindings");
        bindings.Clear();
    }
}
