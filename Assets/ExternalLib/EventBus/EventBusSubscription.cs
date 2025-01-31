using System;
using System.Collections.Generic;

namespace EventBus{
    internal static class SubscriptionPool<T> where T : IEvent{
        private static readonly Queue<EventBusSubscription<T>> m_pool = new Queue<EventBusSubscription<T>>();
        public static EventBusSubscription<T> Get(){
            if(m_pool.Count > 0){
                return m_pool.Dequeue();
            }
            return new EventBusSubscription<T>();
        }

        public static void Return(EventBusSubscription<T> subscription) => m_pool.Enqueue(subscription);
    }
    internal class EventBusSubscription<T> : IDisposable where T : IEvent{
        public IEventBinding<T> Binding {get; private set;}

        void IDisposable.Dispose(){
            if(Binding != null){
                EventBus<T>.Deregister(Binding);
                Binding = null;
                SubscriptionPool<T>.Return(this);
            }
        }

        public void SetBinding(IEventBinding<T> binding){
            if(Binding != null) throw new InvalidOperationException("Binding is already set.");
            Binding = binding;
        }
    }
}