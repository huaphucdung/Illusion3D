using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverManager : MonoBehaviour
{
    [SerializeField] private List<River> sources = new List<River>();

    private EventBinding<RiverEvent> riverEventBiding;

    private void Start()
    {
        riverEventBiding = new EventBinding<RiverEvent>(ActiveRiverRun);
    }

    [ContextMenu("Test active source")]
    public void ActiveRiverRun()
    {
        foreach(River source in sources)
        {
            source.ActiveRiverWater(true);
        }
    }

    [ContextMenu("Test not active source")]
    public void NotActiveRiverRun()
    {
        foreach (River source in sources)
        {
            source.ActiveRiverWater(false);
        }
    }
}

public struct RiverEvent : IEvent {}