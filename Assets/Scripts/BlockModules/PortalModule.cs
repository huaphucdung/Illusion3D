using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalModule : BlockModule
{
    [SerializeField] private Walkable blockDestination;
    [SerializeField] private Walkable blockMoveto;

    public override void Active()
    {
    }

    public override void Active(Player player)
    {
        player.SetPosition(blockDestination);
        player.SetClickedPosition(blockMoveto);
    }
}
