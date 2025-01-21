using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndModule : BlockModule
{
    public override void Active()
    {
    }

    public override void Active(Player player)
    {
        EventBus<EndGameEvent>.Raise(new EndGameEvent());
    }
}
