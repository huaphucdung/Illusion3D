using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BlockModule : MonoBehaviour
{
    [SerializeField] private List<BlockGroup> groups;

    protected void Active()
    {
        foreach (var group in groups)
        {
            group.Active();
        }
    }
}
