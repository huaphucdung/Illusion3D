using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Walkable _currentBlock;
    private Walkable _clickedBlock;
    //private Walkable _indicator;

    private readonly List<Walkable> _movePath = new List<Walkable>();
    private readonly HashSet<Walkable> _nextBlocks = new HashSet<Walkable>();
    private readonly HashSet<Walkable> _pastBlocks = new HashSet<Walkable>();
    private readonly Queue<List<Path>> _queue = new Queue<List<Path>>();

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
        if (paths == null || paths.Count == 0) return;

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
        
        _pastBlocks.Add(from);
        foreach(Path path in from.possiblePaths){
            if (!path.active) continue;
            
            _queue.Enqueue(new List<Path> { path });
            _pastBlocks.Add(path.target);
        }

        while(_queue.Count > 0){
            List<Path> currentPath = _queue.Dequeue();
            Walkable currentWalkable = currentPath[^1].target;

            if(currentWalkable == to) return currentPath;
            

            foreach(Path path in currentWalkable.possiblePaths){
                if(!path.active || _pastBlocks.Contains(path.target)) continue;

                // found path
                if(path.target == to){
                    currentPath.Add(path);
                    return currentPath; 
                }

                // create new list with the old paths and new path
                List<Path> newPath = new List<Path>(currentPath) { path }; 
                _queue.Enqueue(newPath);
                // mark as visited
                _pastBlocks.Add(path.target);
            }
        }
        return null;
    }

    private void Clear()
    {
        _movePath.Clear();
        _nextBlocks.Clear();
        _pastBlocks.Clear();
        _queue.Clear();
        IsWalking = false;
    }
    
    private void SetNewCurrentBlock()
    {
        _currentBlock = _clickedBlock;
    }
}
