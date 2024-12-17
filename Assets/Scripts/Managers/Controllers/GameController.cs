using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameController
{
    [Inject] private PlayerSetting _playerSetting;
    [Inject] private Player _player;

    private readonly HashSet<Walkable> _pastBlocks = new HashSet<Walkable>();
    private readonly Queue<List<Path>> _queue = new Queue<List<Path>>();
    private readonly HashSet<BlockGroup> _moveingBlockGroup = new HashSet<BlockGroup>();

    private Walkable _currentBlock;
    private Walkable _clickedBlock;

    private EventBinding<BlockGroupChangePositionAndRoationStartEvent> _changePositionAndRoationStartEvent;
    private EventBinding<BlockGroupChangePositionAndRoationCompleteEvent> _changePositionAndRoationCompleteEvent;

    public bool IsWalking { get; private set; } = false;

    private Sequence _sequeue;

    public GameController(PlayerSetting playerSetting, Player player)
    {
        _playerSetting = playerSetting;
        _player = player;

        _changePositionAndRoationStartEvent = new EventBinding<BlockGroupChangePositionAndRoationStartEvent>(OnBlockGroupChangeStart);
        _changePositionAndRoationCompleteEvent = new EventBinding<BlockGroupChangePositionAndRoationCompleteEvent>(OnBlockGroupChangeComplete);
    }

    public void ActiveControl()
    {
        InputManager.click += OnClick;
        EventBus<BlockGroupChangePositionAndRoationStartEvent>.Register(_changePositionAndRoationStartEvent);
        EventBus<BlockGroupChangePositionAndRoationCompleteEvent>.Register(_changePositionAndRoationCompleteEvent);
    }

    public void DeactiveControl()
    {
        InputManager.click -= OnClick;
        EventBus<BlockGroupChangePositionAndRoationStartEvent>.Deregister(_changePositionAndRoationStartEvent);
        EventBus<BlockGroupChangePositionAndRoationCompleteEvent>.Deregister(_changePositionAndRoationCompleteEvent);
    }

    #region Main Methods
    public void SetPlayerAtBlock(Walkable block)
    {
        _currentBlock = block;
        _player.SetPosition(block.GetWalkPoint());
    }

    public void SetTargetBlock(Walkable block)
    {
        _clickedBlock = block;
        Clear();
        FollowPath(FindPath(from: _currentBlock, to: _clickedBlock));
    }

    private void FollowPath(List<Path> paths)
    {
        if (paths == null || paths.Count == 0) return;
       

        // Lock action and group when player move
        IsWalking = true;
        _player.Animator.ChangeMoving(1);

        _sequeue = DOTween.Sequence().SetAutoKill();
 
        EventBus<BlockGroupLoockAllEvent>.Raise(new BlockGroupLoockAllEvent() { isLock = true });

        Walkable currentWalkable = _currentBlock;

        //Check Do Rotation before before move at player all
        _sequeue.Append(paths[0].command.FirstRotaion(_player, currentWalkable, paths[0].target, _playerSetting));

        foreach (Path path in paths)
        {
            _sequeue.Append(path.command.MovePath(_player, currentWalkable, path.target, _playerSetting).OnStart(() => _player.ActiveDeepFeature(path.activeDeep)))
                    .AppendCallback(
                        () => {
                            _currentBlock?.ActiveLevaveModule(_player);
                            _player.SetParent(path.target.transform);
                            _currentBlock = path.target;
                            _currentBlock.ActiveEnterModule(_player);
                        }
                    );
            currentWalkable = path.target;
        }

        _sequeue.AppendCallback(Clear);
    }

    public void AppendSequence(Sequence sequence)
    {
        _sequeue.Append(sequence);
    } 

    private List<Path> FindPath(Walkable from, Walkable to)
    {

        _pastBlocks.Add(from);
        foreach (Path path in from.possiblePaths)
        {
            if (!path.active) continue;

            _queue.Enqueue(new List<Path> { path });
            _pastBlocks.Add(path.target);
        }

        while (_queue.Count > 0)
        {
            List<Path> currentPath = _queue.Dequeue();
            Walkable currentWalkable = currentPath[^1].target;

            if (currentWalkable == to) return currentPath;


            foreach (Path path in currentWalkable.possiblePaths)
            {
                if (!path.active || _pastBlocks.Contains(path.target)) continue;

                // found path
                if (path.target == to)
                {
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
        _pastBlocks.Clear();
        _queue.Clear();
        // Release action and group when player move
        IsWalking = false;
        _player.Animator.ChangeMoving(0);
        EventBus<BlockGroupLoockAllEvent>.Raise(new BlockGroupLoockAllEvent() { isLock = false });
        _currentBlock?.LockBockGroup();
    }
    #endregion


    #region Callback Methods
    private void OnClick()
    {
        if (IsWalking || _moveingBlockGroup.Count > 0) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Walkable block = hit.transform.GetComponent<Walkable>();
            if (block == null) return;
            SetTargetBlock(block);
        }
    }

    private void OnBlockGroupChangeStart(BlockGroupChangePositionAndRoationStartEvent @event)
    {
        _moveingBlockGroup.Add(@event.blockGroup);
      
    }

    private void OnBlockGroupChangeComplete(BlockGroupChangePositionAndRoationCompleteEvent @event)
    {
        _moveingBlockGroup.Remove(@event.blockGroup);
    }
    #endregion
}
