using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PortalModule : BlockModule
{
    [SerializeField] private Walkable blockDestination;
    [SerializeField] private Walkable blockMoveto;
    [SerializeField] private float disappearDuriation = 1f;

    [Inject] private GameController _gameController;
    [Inject] private Player _player;
    [Inject] private PlayerSetting _playerSetting;
    public override void Active()
    {
    }

    public override void Active(Player player)
    {
       
        Sequence sequence = DOTween.Sequence().SetAutoKill();
        float time = disappearDuriation / 2f;


        //Forward 1 block
        sequence.Append(player.transform.DOMove(player.transform.position + player.transform.forward, disappearDuriation)
            .SetEase(Ease.Linear));
        //Wait tele player to target block, rotation and Move to block
        sequence.Append(DOVirtual.DelayedCall(time, MoveToBlock));
        
        _gameController.AppendSequence(sequence);

    }

    private void MoveToBlock()
    {
        Vector3 direction = (blockMoveto.GetWalkPoint() - blockDestination.GetWalkPoint()).normalized;
        _gameController.SetPlayerAtBlock(blockDestination);
        _player.SetRotation(direction, blockDestination.transform.up);
        _gameController.SetTargetBlock(blockMoveto);
    }
}
