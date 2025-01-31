using System;
using System.Collections.Generic;
using UnityEngine;
namespace EventBus
{
    public class MonoEventInvoker : MonoBehaviour
    {
        private static readonly Dictionary<Type, Action<IEvent>> s_EventInvokers = new Dictionary<Type, Action<IEvent>>();
        
        [SerializeField] bool invokeOnStart;
        [SerializeReference, SubclassSelector] private IEventWrapper serializedEvent;

        private System.Collections.IEnumerator Start()
        {
            yield return new WaitForSeconds(1f);
            if (invokeOnStart) InvokeEvent();
        }

        public void InvokeEvent()
        {
            if (serializedEvent == null || serializedEvent.Event == null) return;

            Type eventType = serializedEvent.Event.GetType();

            if (!s_EventInvokers.TryGetValue(eventType, out Action<IEvent> invoker))
            {
                Type eventBusType = typeof(EventBus<>).MakeGenericType(eventType);

                var raiseMethod = eventBusType.GetMethod("Raise", new[] { typeof(IEvent) });
                if (raiseMethod != null)
                {
                    invoker = (Action<IEvent>)Delegate.CreateDelegate(typeof(Action<IEvent>), null, raiseMethod);
                    s_EventInvokers[eventType] = invoker;
                }
                else
                {
                    Debug.LogError($"Failed to create invoker for event type {eventType}");
                    return;
                }
            }

            invoker.Invoke(serializedEvent.Event);
        }

    }
}