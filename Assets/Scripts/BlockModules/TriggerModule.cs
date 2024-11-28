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
        EventBus<TriggertEvent>.Raise(new TriggertEvent { type = triggerType });
    }
}

public enum TriggerType
{
    End,
    CutScene,
}

public struct TriggertEvent : IEvent
{
    public TriggerType type;
}
