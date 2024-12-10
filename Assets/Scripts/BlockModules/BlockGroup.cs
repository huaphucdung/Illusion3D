using AYellowpaper.SerializedCollections;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BlockGroup : MonoBehaviour
{
    [SerializedDictionary("Position and Rotation", "Path To Path")]
    [SerializeField] private SerializedDictionary<PosistionAndRotationTrigger, List<PathLink>> pathToPathDictionary;
    [SerializeField] private float moveDuriation = 2f;

    [Header("Settings:")]
    [SerializeField] private bool isLock;
    [SerializeField] private bool isCannotLockWhenPlayerOn = false;
    public bool IsLock => isLock;

    private Vector3 defaultPostion;
    private Quaternion defaultRotaton;
    private EventBinding<ResetEvent> resetEventBinding;
    private EventBinding<BlockGroundEvent> blockGroupEventBinding;

    public event Action<bool> lockAction;

    private void Start()
    {
        defaultPostion = transform.position;
        defaultRotaton = transform.rotation;
    }

    private void Awake()
    {
        resetEventBinding = new EventBinding<ResetEvent>(ResetModule);
        blockGroupEventBinding = new EventBinding<BlockGroundEvent>(Lock);
    }

    private void OnEnable()
    {
        EventBus<ResetEvent>.Register(resetEventBinding);
        EventBus<BlockGroundEvent>.Register(blockGroupEventBinding);
    }

    private void OnDisable()
    {
        EventBus<ResetEvent>.Deregister(resetEventBinding);
        EventBus<BlockGroundEvent>.Deregister(blockGroupEventBinding);
    }

    private void ResetModule()
    {
        transform.SetPositionAndRotation(defaultPostion, defaultRotaton);
    }

    private void Lock(BlockGroundEvent @event)
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
        Sequence sequence = DOTween.Sequence();
        if (transform.localPosition != position)
            sequence.Append(transform.DOLocalMove(position, moveDuriation).SetEase(Ease.Linear));
        if (transform.localEulerAngles != rotation)
            sequence.Append(transform.DOLocalRotate(rotation, moveDuriation).SetEase(Ease.Linear));
        sequence.AppendCallback(Active);
    }

    public void Active()
    {
        foreach (KeyValuePair<PosistionAndRotationTrigger, List<PathLink>> connectPath in pathToPathDictionary)
        {
            PosistionAndRotationTrigger key = connectPath.Key;
            bool isActivePath = key.Equals(transform);
            foreach (PathLink link in connectPath.Value)
            {
                ActiveLink(link, isActivePath);
            }
        }

        //Active connect Group to Group
        EventBus<BlockGroupEvent>.Raise(new BlockGroupEvent() { group = this});
    }

    private void DisablePathAll()
    {
        foreach (KeyValuePair<PosistionAndRotationTrigger, List<PathLink>> connectPath in pathToPathDictionary)
        {
            foreach (PathLink link in connectPath.Value)
            {
                ActiveLink(link, false);
            }
        }
    }

    public void ActiveLink(PathLink link, bool isActive)
    {
       
        link.pathFrom.ActivePath(link.pathTo, isActive);
        link.pathTo.ActivePath(link.pathFrom, isActive);
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
    [Header("Path To")]
    public Walkable pathTo;
}

public struct BlockGroundEvent: IEvent
{
    public bool isLock;
}