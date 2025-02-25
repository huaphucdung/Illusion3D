using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class BlockModule : MonoBehaviour
{

    private EventBinding<ResetEvent> resetEventBiding;

    private void Awake()
    {
        resetEventBiding = new EventBinding<ResetEvent>(ResetModule);
    }

    private void OnEnable()
    {
        
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
