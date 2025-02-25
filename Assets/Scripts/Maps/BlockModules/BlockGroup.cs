using AYellowpaper.SerializedCollections;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BlockGroup : MonoBehaviour
{
    [SerializeField] private PathToPath pathToPath;
    [SerializeField] private RiverToRiver riverToRiver;

    [Header("Settings:")]
    [SerializeField] private bool isLock;
    [SerializeField] private bool isCannotLockWhenPlayerOn = false;
    [SerializeField] private float moveDuriation = 2f;
    public bool IsLock => isLock;

    private Vector3 defaultPostion;
    private Quaternion defaultRotaton;
    private EventBinding<ResetEvent> resetEventBinding;
    private EventBinding<BlockGroupLoockAllEvent> blockGroupEventBinding;

    public event Action<bool> lockAction;

    private void Start()
    {
        defaultPostion = transform.position;
        defaultRotaton = transform.rotation;
    }

    private void Awake()
    {
        resetEventBinding = new EventBinding<ResetEvent>(ResetModule);
        blockGroupEventBinding = new EventBinding<BlockGroupLoockAllEvent>(Lock);
    }

    private void OnEnable()
    {
        EventBus<ResetEvent>.Register(resetEventBinding);
        EventBus<BlockGroupLoockAllEvent>.Register(blockGroupEventBinding);
    }

    private void OnDisable()
    {
        EventBus<ResetEvent>.Deregister(resetEventBinding);
        EventBus<BlockGroupLoockAllEvent>.Deregister(blockGroupEventBinding);
    }

    private void ResetModule()
    {
        transform.SetPositionAndRotation(defaultPostion, defaultRotaton);
    }

    private void Lock(BlockGroupLoockAllEvent @event)
    {
        isLock = @event.isLock;
    }

    public void Lock(bool value)
    {
        LockNotCallAction(value);
        lockAction?.Invoke(IsLock);
    }

    public void LockNotCallAction(bool value)
    {
        isLock = value ^ isCannotLockWhenPlayerOn;
    }

    public void SetPositionAndRotaion(Vector3 position, Vector3 rotation)
    {
        DisablePathAll();

        //Call event to add this block group in list lock group
        OnChangePositionAndRotationStart();

        Sequence sequence = DOTween.Sequence();
        if (transform.localPosition != position)
            sequence.Append(transform.DOLocalMove(position, moveDuriation).SetEase(Ease.Linear));
        if (transform.localEulerAngles != rotation)
            sequence.Append(transform.DOLocalRotate(rotation, moveDuriation).SetEase(Ease.Linear));
        sequence.AppendCallback(Active);
        
        //Call event to remove this block group out list lock group
        sequence.AppendCallback(OnChangePositionAndRotationComplete);
    }

    private void OnChangePositionAndRotationStart()
    {
        EventBus<BlockGroupChangePositionAndRoationStartEvent>.Raise(new BlockGroupChangePositionAndRoationStartEvent() { blockGroup = this });
    }

    private void OnChangePositionAndRotationComplete()
    {
        EventBus<BlockGroupChangePositionAndRoationCompleteEvent>.Raise(new BlockGroupChangePositionAndRoationCompleteEvent() { blockGroup = this });
    }

    public void Active()
    {
        pathToPath.Active(transform);
        riverToRiver.Active(transform);

        //Active connect Group to Group
        EventBus<BlockGroupChangeEvent>.Raise(new BlockGroupChangeEvent() { group = this});
    }

    public void DisablePathAll()
    {
        pathToPath.Deactive();
        riverToRiver.Deactive();
        EventBus<BlockGroupChangeEvent>.Raise(new BlockGroupChangeEvent() { group = this });
    }
}

[Serializable]
public class PathToPath
{
    [SerializedDictionary("Position and Rotation", "Path To Path")]
    [SerializeField] private SerializedDictionary<PosistionAndRotationTrigger, List<PathLink>> pathToPathDictionary;

    public void Active(Transform transform)
    {
        foreach (KeyValuePair<PosistionAndRotationTrigger, List<PathLink>> connectPath in pathToPathDictionary)
        {
            PosistionAndRotationTrigger key = connectPath.Key;
            bool isActivePath = key.Equals(transform);
            foreach (PathLink link in connectPath.Value)
            {
                link.Active(isActivePath);
            }
        }
    }

    public void Deactive()
    {
        foreach (KeyValuePair<PosistionAndRotationTrigger, List<PathLink>> connectPath in pathToPathDictionary)
        {
            foreach (PathLink link in connectPath.Value)
            {
                link.Active(false);
            }
        }
    }
}

[Serializable]
public class RiverToRiver
{
    [SerializedDictionary("Position and Rotation", "Path To Path")]
    [SerializeField] private SerializedDictionary<PosistionAndRotationTrigger, List<RiverLink>> riverToRiverDictionary;

    public void Active(Transform transform)
    {
        foreach (KeyValuePair<PosistionAndRotationTrigger, List<RiverLink>> connectPath in riverToRiverDictionary)
        {
            PosistionAndRotationTrigger key = connectPath.Key;
            bool isActivePath = key.Equals(transform);
            foreach (RiverLink link in connectPath.Value)
            {
                link.Active(isActivePath);
            }
        }
    }

    public void Deactive()
    {
        foreach (KeyValuePair<PosistionAndRotationTrigger, List<RiverLink>> connectPath in riverToRiverDictionary)
        {
            foreach (RiverLink link in connectPath.Value)
            {
                link.Active(false);
            }
        }
    }
}



[Serializable]
public struct PosistionAndRotationTrigger : IEquatable<PosistionAndRotationTrigger>
{
    public Vector3 position;
    public Vector3 rotation;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(Transform other){
        Quaternion rotation1 = Quaternion.Euler(rotation);
        Quaternion rotation2 = Quaternion.Euler(other.localEulerAngles);
        return position == other.localPosition && (Quaternion.Angle(rotation1, rotation2) < 0.01f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(PosistionAndRotationTrigger other)
    {
        return position.Equals(other.position) && rotation.Equals(other.rotation);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode()
    {
        return position.GetHashCode() ^ rotation.GetHashCode();
    }
}

[Serializable]
public class PathLink
{
    [Header("Path From")]
    public Walkable pathFrom;
    public Direction fromDirection;
    
    [Header("Path To")]
    public Walkable pathTo;
    public Direction toDirection;
    public void Active(bool value)
    {
        pathFrom.ActivePath(pathTo, value, fromDirection);
        pathTo.ActivePath(pathFrom, value, toDirection);
    }
}

[Serializable]
public class RiverLink
{
    [Header("River From")]
    public River riverFrom;
    [Header("River To")]
    public River riverTo;

    public void Active(bool value)
    {
        riverFrom.ActivePath(riverTo, value);
        riverTo.ActivePath(riverFrom, value);
    }
}

public struct BlockGroupLoockAllEvent: IEvent
{
    public bool isLock;
}

public struct BlockGroupChangePositionAndRoationCompleteEvent : IEvent
{
    public BlockGroup blockGroup;
}

public struct BlockGroupChangePositionAndRoationStartEvent : IEvent
{
    public BlockGroup blockGroup;
}