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
        Clear();
        FollowPath(FindPath(from: _currentBlock, to: _clickedBlock));
    }

    private void FollowPath(List<Path> paths){
        if(paths == null || paths.Count == 0) {
            Debug.Log("No path found");
            return;
        }

        Sequence sequeue = DOTween.Sequence();
        IsWalking = true;
        Walkable currentWalkable = _currentBlock;
        foreach(Path path in paths){
            sequeue.Append(path.command.MovePath(this, currentWalkable, path.target))
                    .AppendCallback(
                        () => { // update current block
                            transform.parent = path.target.transform;
                            _currentBlock = path.target;
                            _currentBlock.ActiveModule(this);
                        }
                    );
            currentWalkable = path.target;
        }

        sequeue.AppendCallback(Clear).AppendCallback(SetNewCurrentBlock);
    }
    private List<Path> FindPath(Walkable from, Walkable to){
        Queue<List<Path>> queue = new Queue<List<Path>>();

        _pastBlocks.Add(from);
        foreach(Path path in from.possiblePaths){
            if(path.active == true){
                queue.Enqueue(new List<Path> { path });
                _pastBlocks.Add(path.target);
            }
        }


        while(queue.Count > 0){
            List<Path> currentPath = queue.Dequeue();

            Walkable currentWalkable = currentPath[^1].target;

            if(currentWalkable == to){
                return currentPath;
            }

            foreach(Path path in currentWalkable.possiblePaths){
                if(path.active == false || _pastBlocks.Contains(path.target)) continue;

                // found path
                if(path.target == to){
                    currentPath.Add(path);
                    return currentPath; 
                }

                // create new list with the old paths and new path
                List<Path> newPath = new List<Path>(currentPath) { path }; 
                queue.Enqueue(newPath);
                // mark as visited
                _pastBlocks.Add(path.target);
            }
        }

        return null;
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
            sequeue.AppendCallback(() =>
            {
                transform.parent = path.transform;
                _currentBlock = path;
                _currentBlock.ActiveModule(this);
            });
        }

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
