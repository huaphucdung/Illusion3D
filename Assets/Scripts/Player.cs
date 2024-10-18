using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    private readonly List<Walkable> movePath = new List<Walkable>();

    private Walkable _currentBlock;
    private Walkable _clickedBlock;
    //private Walkable _indicator;

    private readonly HashSet<Walkable> _nextBlocks = new HashSet<Walkable>();
    private readonly HashSet<Walkable> _pastBlocks = new HashSet<Walkable>();

    public bool IsWalking { get; private set; } = false;

    public void SetPosition(Walkable block)
    {
        transform.position = block.GetWalkPoint();
        _currentBlock = block;
    }

    public void SetClickedPosition(Walkable block)
    {
        DOTween.Kill(gameObject.transform);
        _clickedBlock = block;
        FindPath();
    }

    private void FindPath()
    {
        if (_currentBlock == null) return;
        Clear();

        foreach (Path path in _currentBlock.possiblePaths)
        {
            if(path.active)
            {
                _nextBlocks.Add(path.target);
                path.target.previousBlock = _currentBlock;
                path.target.pathCommandDo = path.command;
            }
        }

        _pastBlocks.Add(_currentBlock);

        ExplorePath();
        BuildPath();
    }

    private void ExplorePath()
    {
        if(_nextBlocks.Count == 0) return;
        Walkable current = _nextBlocks.First();
        _nextBlocks.Remove(current);

        if (current == _clickedBlock) return;

        foreach(Path path in current.possiblePaths)
        {
            if (_pastBlocks.Contains(path.target) || !path.active) continue;
            _nextBlocks.Add(path.target);
            path.target.previousBlock = current;
            path.target.pathCommandDo = path.command;
        }

        _pastBlocks.Add(current);

        if (_nextBlocks.Count == 0) return;
        ExplorePath();
    }

    private void BuildPath()
    {
        Walkable block = _clickedBlock;
        while(block != _currentBlock)
        {
            movePath.Add(block);
            if (block.previousBlock == null) return;
            block = block.previousBlock;
        }

        FollowPath();
    }

    private void FollowPath()
    {
        Sequence sequeue = DOTween.Sequence();
        IsWalking = true;
        movePath.Reverse();
        foreach(Walkable path in movePath)
        {
            sequeue.Append(path.DoMove(this));

            /*if (!path.dontRotate)
                sequeue.Join(transform.DOLookAt(path.transfrom.position, .1f, AxisConstraint.Y, Vector3.up));*/
        }

        /*if (_clickedBlock.isButton)
        {
            sequeue.AppendCallback(() => GameManager.instance.RotateRightPivot());
        }*/

        sequeue.AppendCallback(Clear);
        sequeue.AppendCallback(SetNewCurrentBlock);
    }

    private void Clear()
    {
        foreach (Walkable path in movePath)
        {
            path.previousBlock = null;
        }
        movePath.Clear();
        _nextBlocks.Clear();
        _pastBlocks.Clear();
        IsWalking = false;
    }
    
    private void SetNewCurrentBlock()
    {
        _currentBlock = _clickedBlock;
    }
}
