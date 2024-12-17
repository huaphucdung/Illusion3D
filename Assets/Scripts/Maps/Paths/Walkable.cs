using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Walkable : MonoBehaviour
{
    [Header("Path Connects:")]
    public List<Path> possiblePaths = new List<Path>();

    [Header("Build Settings:")]
    [SerializeField] private bool isStair = false;
    [SerializeField] private float walkPointOffset = 0.5f;
    [SerializeField] private float stairPointOffset = 0.0f;

    [Inject(Optional = true)]
    public BlockGroup blockGroup;

   
    private BlockModule blockActiveModule;

    public bool IsStar => isStair;

    private void Start()
    {
        blockActiveModule = GetComponent<BlockModule>();
    }

    public Vector3 GetWalkPoint()
    {
        float offset = isStair ? stairPointOffset : walkPointOffset;
        return transform.position + transform.up * offset;
    }

    public void ActivePath(Walkable target, bool value, Direction direction)
    {
        Path path = possiblePaths.FirstOrDefault(x => x.target == target);
        if (path == null) return;
        path.active = value;
        path.SetDirection(direction);
    }

    public void ActiveEnterModule(Player player)
    {
        blockActiveModule?.Active(player);
    }

    public void ActiveLevaveModule(Player player)
    {
    }

    public void LockBockGroup()
    {
        blockGroup?.Lock(true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(GetWalkPoint(), .1f);

        if (possiblePaths.Count == 0) return;

        foreach (Path path in possiblePaths)
        {
            if (path.target == null) continue;
            Gizmos.color = path.active ? Color.blue : Color.clear;
            path.command.DrawGizmod(this, path);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (possiblePaths.Count == 0) return;

        foreach (Path path in possiblePaths)
        {
            if (path.target == null) continue;
            Gizmos.color = path.active ? Color.green : Color.clear;
            path.command.DrawGizmod(this, path);
        }
    }
}

[Serializable]
public class Path
{
    [Header("Settings:")]
    public Walkable target;
    public bool active = false;
    public bool activeDeep = false;

    [Header("Force Settinngs:")]
    public bool isForceRotation;
    public Direction directionTarget;

    [Header("Command:")]
    [SerializeReference, SubclassSelector]
    public IPathCommand command = new WalkPathCommand();

    public void SetDirection(Direction direction)
    {
        directionTarget = direction;
    }
}


