using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Walkable : MonoBehaviour
{
    [Header("Path Connects:")]
    public List<Path> possiblePaths = new List<Path>();

    [Header("Build Settings:")]
    [SerializeField] private bool isStair = false;
    [SerializeField] private float walkPointOffset = 0.5f;
    [SerializeField] private float stairPointOffset = 0.0f;

    [Header("Module:")]
    [SerializeField] private BlockModule blockActiveModule;

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

    public void ActivePath(Walkable target, bool value)
    {
        Path path = possiblePaths.FirstOrDefault(x => x.target == target);
        if (path == null) return;
        path.active = value;
    }

    public void ActiveModule(Player player)
    {
        blockActiveModule?.Active(player);
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
            path.command.DrawGizmod(this, path.target);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (possiblePaths.Count == 0) return;

        foreach (Path path in possiblePaths)
        {
            if (path.target == null) continue;
            Gizmos.color = path.active ? Color.green : Color.clear;
            path.command.DrawGizmod(this, path.target);
        }
    }
}

[Serializable]
public class Path
{
    public Walkable target;
    public bool active = false;
    [SerializeReference, SubclassSelector]
    public IPathCommand command = new WalkPathCommand();
}


