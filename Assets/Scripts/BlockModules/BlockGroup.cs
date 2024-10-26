using AYellowpaper.SerializedCollections;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BlockGroup : MonoBehaviour
{
    [SerializedDictionary("Position and Rotation", "Test")]
    [SerializeField] private SerializedDictionary<PosistionAndRotationTrigger, List<PathLink>> testDic;
    
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
    public void SetPositionAndRotaion(Vector3 postion, Quaternion rotation)
    {
        DOTween.Kill(transform);
        transform.DOMove(postion, 0.2f);
        transform.DORotateQuaternion(rotation, 0.2f);
        Active();
    }

    public void Active()
    {
        foreach (KeyValuePair<PosistionAndRotationTrigger, List<PathLink>> connectPath in testDic)
        {
            PosistionAndRotationTrigger key = connectPath.Key;
            bool isActivePath = key.Equals(transform);

            foreach (PathLink link in connectPath.Value)
            {
                ActiveLink(link, isActivePath);
            }
        }
    }

    private void ActiveLink(PathLink link, bool isActive)
    {
        link.pathFrom.ActivePath(link.pathTo, isActive);
        link.pathTo.ActivePath(link.pathFrom, isActive);
    }

}

[Serializable]
public struct PosistionAndRotationTrigger : IEquatable<PosistionAndRotationTrigger>
{
    public Vector3 position;
    public Quaternion rotation;

    public bool Equals(Transform other){
        return position.Equals(other.position) && rotation.Equals(other.rotation);
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
    public Walkable pathFrom;
    public Walkable pathTo;
}