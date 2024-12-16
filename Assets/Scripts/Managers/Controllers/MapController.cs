using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

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
    private void Awake()
    {
        blockGroupEventBinding = new EventBinding<BlockGroupChangeEvent>(TriggerBlockGrounEvent);
    }

    public void Initialize()
    {
        _currentMiniMap = miniMaps[0];
    }

    public void NextMap()
    {
        if (miniMaps.IndexOf(_currentMiniMap) + 1 >= miniMaps.Count) return;
        _currentMiniMap = miniMaps[miniMaps.IndexOf(_currentMiniMap) + 1];
    }

    private void OnEnable()
    { 
        EventBus<BlockGroupChangeEvent>.Register(blockGroupEventBinding);
    }

    private void OnDisable()
    {
        EventBus<BlockGroupChangeEvent>.Deregister(blockGroupEventBinding);
    }

    public void TriggerBlockGrounEvent(BlockGroupChangeEvent @event)
    {
        _currentMiniMap?.ActivePathGroupToGroup(@event.group);
    }
}

public class BlockGroupChangeEvent : IEvent
{
    public BlockGroup group;
}

