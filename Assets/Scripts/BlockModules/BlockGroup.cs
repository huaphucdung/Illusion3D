using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockGroup : MonoBehaviour
{
    [SerializedDictionary("Position and Rotation", "Test")]
    [SerializeField] private SerializedDictionary<PosistionAndRotationTrigger, List<PathLink>> testDic;
    private Vector3 defaultPostion;
    private Quaternion defaultRotaton;

    private void Start()
    {
        defaultPostion = transform.position;
        defaultRotaton = transform.rotation;
    }

    public void ResetModule()
    {
        transform.position = defaultPostion;
        transform.rotation = defaultRotaton;
    }

    public void Active()
    {
        foreach (KeyValuePair<PosistionAndRotationTrigger, List<PathLink>> connectPath in testDic)
        {
            PosistionAndRotationTrigger key = connectPath.Key;
            bool isActivePath = key.position == transform.position && key.rotation == transform.rotation;

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
public struct PosistionAndRotationTrigger
{
    public Vector3 position;
    public Quaternion rotation;
}

[Serializable]
public class PathLink
{
    public Walkable pathFrom;
    public Walkable pathTo;
}