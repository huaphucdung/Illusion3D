using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MapController : MonoBehaviour
{
    [Header("Mini Map:")]
    [SerializeField] private List<MiniMap> miniMaps;

    private EventBinding<TriggertEvent> triggerEvenBinding;
    private EventBinding<BlockGroupEvent> blockGroupEvenBinding;
    private PlayableDirector _director;
    private Player _player;
    public MiniMap _currentMiniMap;
    private void Awake()
    {
        triggerEvenBinding = new EventBinding<TriggertEvent>(TriggerEvent);
        blockGroupEvenBinding = new EventBinding<BlockGroupEvent>(TriggerBlockGrounEvent);
    }

    public void Initiliaze(Player player)
    {
        _player = player;
        //_currentMiniMap = miniMaps[0];
        _player.SetPosition(_currentMiniMap.PointStart);
    }


    private void OnEnable()
    {
        EventBus<TriggertEvent>.Register(triggerEvenBinding);
        EventBus<BlockGroupEvent>.Register(blockGroupEvenBinding);
    }

    private void OnDisable()
    {
        EventBus<TriggertEvent>.Deregister(triggerEvenBinding);
        EventBus<BlockGroupEvent>.Deregister(blockGroupEvenBinding);
    }

    private void TriggerEvent(TriggertEvent @event)
    {
        switch (@event.type)
        {
            case TriggerType.CutScene:
                PlayCutScene();
                break;
            default:
                EndMap();
                break;
        }
    }

    public void TriggerBlockGrounEvent(BlockGroupEvent @event)
    {
        _currentMiniMap?.ActivePathGroupToGroup(@event.group);
    }

    private void PlayCutScene()
    {
        int miniMapInded = miniMaps.IndexOf(_currentMiniMap);
        if (miniMapInded >= miniMaps.Count - 1) return;
        _player.transform.SetParent(null);

        if (_director) _director.stopped -= OnCutSceneEnd;
        _director = _currentMiniMap.Director;
        _director.stopped += OnCutSceneEnd;

        _director?.Play();

        _currentMiniMap = miniMaps[miniMapInded + 1];
    }

    private void EndMap()
    {

    }

    private void OnCutSceneEnd(PlayableDirector pd)
    {
        _player.SetPosition(_currentMiniMap.PointStart);
    }
}

public class BlockGroupEvent : IEvent
{
    public BlockGroup group;
}