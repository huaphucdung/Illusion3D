using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;
using Zenject.ReflectionBaking.Mono.Cecil;

public class MapController : MonoBehaviour
{
    [Header("Mini Map:")]
    [SerializeField] private List<MiniMap> miniMaps;
    
    [Inject] private Player _player;
    [Inject] private GameController _gameController;

    public MiniMap _currentMiniMap;

    public Walkable PointStart => _currentMiniMap.PointStart;
    public PlayableDirector PlayableDirector => _currentMiniMap.PlayableDirector;
    
    private EventBinding<BlockGroupChangeEvent> blockGroupEventBinding;  
    private EventBinding<RiverSourceActiveEvent> riverSourceActiveEventBinding;

    private EventBinding<RiverTriggerEvent> riverTriggerEventBinding;


    public RiverManager RiverManager { get; private set; }

    private void Awake()
    {
        RiverManager = GetComponent<RiverManager>();

        blockGroupEventBinding = new EventBinding<BlockGroupChangeEvent>(TriggerBlockGrounEvent);
        riverSourceActiveEventBinding = new EventBinding<RiverSourceActiveEvent>(TriggerRiverSourceActiveEvent);
        riverTriggerEventBinding = new EventBinding<RiverTriggerEvent>(RiverTrigerEvent);
    }

    private void Start()
    {
        RiverManager?.ActiveNewRun();
    }

    public void Initialize()
    {
        _currentMiniMap = miniMaps[0];
        RiverManager?.ActiveNewRun();
    }

    public void NextMap()
    {
        if (miniMaps.IndexOf(_currentMiniMap) + 1 >= miniMaps.Count) return;
        _currentMiniMap = miniMaps[miniMaps.IndexOf(_currentMiniMap) + 1];
    }

    private void OnEnable()
    { 
        EventBus<BlockGroupChangeEvent>.Register(blockGroupEventBinding);
        EventBus<RiverSourceActiveEvent>.Register(riverSourceActiveEventBinding);
        EventBus<RiverTriggerEvent>.Register(riverTriggerEventBinding);
    }

    private void OnDisable()
    {
        EventBus<BlockGroupChangeEvent>.Deregister(blockGroupEventBinding);
        EventBus<RiverSourceActiveEvent>.Deregister(riverSourceActiveEventBinding);
        EventBus<RiverTriggerEvent>.Deregister(riverTriggerEventBinding);
    }

    private void TriggerBlockGrounEvent(BlockGroupChangeEvent @event)
    {
        _currentMiniMap?.ActivePathGroupToGroup(@event.group);
        RiverManager?.ActiveNewRun();
    }

    private void TriggerRiverSourceActiveEvent(RiverSourceActiveEvent @event)
    {
        RiverManager?.ChangeRiverSources(@event.sourceDictionary);
    }

    private void RiverTrigerEvent(RiverTriggerEvent @event)
    {
        _currentMiniMap?.TriggerRiver(@event.river);
    }
}

public class BlockGroupChangeEvent : IEvent
{
    public BlockGroup group;
}

public class RiverSourceActiveEvent : IEvent
{
    public Dictionary<River, bool> sourceDictionary;
}

public class RiverTriggerEvent : IEvent { public River river; }

