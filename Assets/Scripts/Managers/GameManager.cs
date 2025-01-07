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
    
    private readonly HashSet<object> tweenObject = new HashSet<object>();

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

    public void NextMiniMap()
    {
        _mapController.NextMiniMap();
        _gameController.SetPlayerAtBlock(_mapController.PointStart);
    }

    private void StartGame()
    {
        //Set Firt postion for player
        _gameController.SetPlayerAtBlock(_mapController.PointStart);
    }

    private void OnEnable()
    {
        _gameController.ActiveControl();
    }

    private void OnDisable()
    {
        _gameController.DeactiveControl();
    }
}
