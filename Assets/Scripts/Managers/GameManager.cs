using ModestTree;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private MapController _mapController;
    [Inject] private CutSceneController _cutSceneController;
    [Inject] private GameController _gameController;

    private EventBinding<TriggertEvent> triggerEventBinding;
    private EventBinding<CutSceneEndEvent> cutSceneEndEventBinding;

    private readonly HashSet<object> tweenObject = new HashSet<object>();

    private void Awake()
    {
        triggerEventBinding = new EventBinding<TriggertEvent>(OnTriggerEvent);
        cutSceneEndEventBinding = new EventBinding<CutSceneEndEvent>(OnCutSceneEnd);

    }

    private void Start()
    {
        Initalize();
    }

    public void Initalize()
    {
        InputManager.Initialize();
        InputManager.ActiveInput(true);

        StartGame();
    }

    private void StartGame()
    {
        //Set Firt postion for player
        _gameController.SetPlayerAtBlock(_mapController.PointStart);
    }

    private void OnEnable()
    {
        _gameController.ActiveControl();
        EventBus<TriggertEvent>.Register(triggerEventBinding);
        EventBus<CutSceneEndEvent>.Register(cutSceneEndEventBinding);

    }

    private void OnDisable()
    {
        _gameController.DeactiveControl();
        EventBus<TriggertEvent>.Deregister(triggerEventBinding);
        EventBus<CutSceneEndEvent>.Deregister(cutSceneEndEventBinding);

    }


    #region Callback Methods
    private void OnTriggerEvent(TriggertEvent @event)
    {
        switch (@event.type)
        {
            case TriggerType.CutScene:
                _cutSceneController?.PlayCutScene(_mapController.PlayableDirector);
                _mapController.NextMap();
                break;
            default:
                break;
        }
    }

    private void OnCutSceneEnd()
    {
        _gameController.SetPlayerAtBlock(_mapController.PointStart);
    }
    #endregion
}
