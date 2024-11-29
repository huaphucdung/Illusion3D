using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class MiniMap : MonoBehaviour
{
    [Header("Point Start:")]
    [SerializeField] private Walkable pointStart;

    [Header("Point Start:")]
    [SerializeField] private PlayableDirector director;

    [Header("Group To Group Connects:")]
    [SerializeField] private List<BlockGroupConnect> groupToGroupConnects;

    public Walkable PointStart => pointStart;
    public PlayableDirector Director => director;

    private static HashSet<PathLink> pathLinksConnect = new HashSet<PathLink>();

    public void ActivePathGroupToGroup(BlockGroup blockGroup)
    {
        pathLinksConnect.Clear();
        foreach (BlockGroupConnect connect in groupToGroupConnects.Where((group) => blockGroup == group.blockGroupFrom || blockGroup == group.blockGroupTo))
        {
            bool isActivePath = connect.posistionAndRotationRequireFrom.Equals(connect.blockGroupFrom.transform)
                && connect.posistionAndRotationRequireTo.Equals(connect.blockGroupTo.transform);

            if (isActivePath) pathLinksConnect.AddRange(connect.pathLinks);
            else
            {
                foreach (PathLink link in connect.pathLinks)
                {
                    blockGroup.ActiveLink(link, false);
                }
            }
        }

        foreach (PathLink link in pathLinksConnect)
        {
            blockGroup.ActiveLink(link, true);
        }
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
}
