using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerModule : BlockModule
{
    [SerializeField] private TriggerType triggerType;

    public override void Active()
    {
    }

    public override void Active(Player player)
    {
        EventBus<TriggetEvent>.Raise(new TriggetEvent { type = triggerType });
    }
}

public enum TriggerType
{
    End,
    CutScene,
}

public struct TriggetEvent : IEvent
{
    public TriggerType type;
}
