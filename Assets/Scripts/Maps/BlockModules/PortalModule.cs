using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PortalModule : BlockModule
{

    [SerializeField] private Walkable blockDestination;
    [SerializeField] private Walkable blockMoveto;

    [Inject] GameController _gameController;

    public override void Active()
    {
    }

    public override void Active(Player player)
    {
        _gameController.SetPlayerAtBlock(blockDestination);
        _gameController.SetTargetBlock(blockMoveto);
    }
}
