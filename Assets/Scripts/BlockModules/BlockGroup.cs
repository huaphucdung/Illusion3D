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

    
    private Vector3 defaultPostion;
    private Quaternion defaultRotaton;
    private EventBinding<ResetEvent> resetEventBiding;

    private void Start()
    {
        defaultPostion = transform.position;
        defaultRotaton = transform.rotation;
    }

    private void OnEnable()
    {
        resetEventBiding = new EventBinding<ResetEvent>(ResetModule);
        EventBus<ResetEvent>.Register(resetEventBiding);
    }

    private void OnDisable()
    {
        EventBus<ResetEvent>.Deregister(resetEventBiding);
    }

    private void ResetModule()
    {
        transform.SetPositionAndRotation(defaultPostion, defaultRotaton);
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
        link.pathFrom.ActiveTopDeepLayer(link.activeDeepLayerPathFrom);
        link.pathTo.ActiveTopDeepLayer(link.activeDeepLayerPathTo);

        link.pathFrom.ActivePath(link.pathTo, isActive, link.activeDeep);
        link.pathTo.ActivePath(link.pathFrom, isActive, link.activeDeep);
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
    public bool activeDeepLayerPathFrom;
    [Header("Path From")]
    public Walkable pathTo;
    public bool activeDeepLayerPathTo;
    [Header("Settings:")]
    public bool activeDeep;
}