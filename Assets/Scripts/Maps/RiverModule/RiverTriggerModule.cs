using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverTriggerModule : RiverModule
{
    private bool isTrigger;

    public override void Active()
    {
        if (isTrigger) return;
        EventBus<RiverTriggerEvent>.Raise(new RiverTriggerEvent() { river = GetComponent<River>() });

        isTrigger = true;
    }
}
