using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockModule : MonoBehaviour
{
    private EventBinding<ResetEvent> resetEventBiding;
    private void OnEnable()
    {
        resetEventBiding = new EventBinding<ResetEvent>(ResetModule);
        EventBus<ResetEvent>.Register(resetEventBiding);
    }

    private void OnDisable()
    {
        EventBus<ResetEvent>.Deregister(resetEventBiding);
    }

    protected virtual void ResetModule()
    {
    }

    public abstract void Active();

    public abstract void Active(Player player);
}
