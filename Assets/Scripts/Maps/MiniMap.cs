using AYellowpaper.SerializedCollections;
using Project.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [Header("Point Start:")]
    [SerializeField] private Walkable pointStart;

    [Header("Group To Group Connects:")]
    [SerializeField] private List<BlockGroupConnect> groupToGroupConnects;

    [Header("Map Codition Trigger:")]
    [SerializeField] private MapCoditionTrigger mapCoditionTrigger;

    public Walkable PointStart => pointStart;
    private static HashSet<PathLink> pathLinksConnect = new HashSet<PathLink>();
    private static HashSet<RiverLink> riverLinksConnect = new HashSet<RiverLink>();

    public void ActivePathGroupToGroup(BlockGroup blockGroup)
    {
        pathLinksConnect.Clear();
        riverLinksConnect.Clear();

        foreach (BlockGroupConnect connect in groupToGroupConnects.Where((group) => blockGroup == group.blockGroupFrom || blockGroup == group.blockGroupTo))
        {
            bool isActivePath = connect.posistionAndRotationRequireFrom.Equals(connect.blockGroupFrom.transform)
                && connect.posistionAndRotationRequireTo.Equals(connect.blockGroupTo.transform);

            if (isActivePath)
            {
                pathLinksConnect.AddRange(connect.pathLinks);
                riverLinksConnect.AddRange(connect.riverLinks);
            }
            else
            {
                foreach (PathLink link in connect.pathLinks)
                {
                    link.Active(false);
                }
                foreach (RiverLink link in connect.riverLinks)
                {
                    link.Active(false);
                }
            }
        }

        foreach (PathLink link in pathLinksConnect)
        {
            link.Active(true);
        }

        foreach (RiverLink link in riverLinksConnect)
        {
            link.Active(true);
        }
    }

    public void TriggerRiver(River river)
    {
        if (river == null) return;
        mapCoditionTrigger?.Active(river);
    }
}


[Serializable]
public class BlockGroupConnect
{
    [Header("Group From:")]
    public BlockGroup blockGroupFrom;
    public PosistionAndRotationTrigger posistionAndRotationRequireFrom;

    [Space]

    [Header("Group To:")]
    public BlockGroup blockGroupTo;
    public PosistionAndRotationTrigger posistionAndRotationRequireTo;

    [Header("Path Links:")]
    public List<PathLink> pathLinks;

    [Header("River Links:")]
    public List<RiverLink> riverLinks;
}

[Serializable]
public class MapCoditionTrigger
{
    [SerializedDictionary("Block Ground", "Position And Rotation Trigger")]
    [SerializeField] private SerializedDictionary<BlockGroup, PosistionAndRotationTrigger> blockEventDictionary;
    [SerializedDictionary("Source", "Active")]
    [SerializeField] private SerializedDictionary<River, bool> riverSourceDictionary;
    [Header("Setttings:")]
    [SerializeField] private int amountRiverNeed;

    private readonly HashSet<River> riverTrigger = new HashSet<River>();

    public void Active(River river)
    {
        riverTrigger.Add(river);
        if (riverTrigger.Count < amountRiverNeed) return;

        foreach (KeyValuePair<BlockGroup, PosistionAndRotationTrigger> blockEvent in blockEventDictionary)
        {
            BlockGroup key = blockEvent.Key;
            PosistionAndRotationTrigger value = blockEvent.Value;
            key.SetPositionAndRotaion(value.position, value.rotation);
        }
        EventBus<RiverSourceActiveEvent>.Raise(new RiverSourceActiveEvent { sourceDictionary = riverSourceDictionary });
    }
}