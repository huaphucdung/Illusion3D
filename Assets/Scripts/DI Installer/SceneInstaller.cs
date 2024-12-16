using System.Collections.Generic;
using System;
using UnityEngine;
using Zenject;
using Zenject.Asteroids;

public class SceneInstaller : MonoInstaller
{
    [Header("References:")]
    [SerializeField] private Player player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MapController mapController;

    public override void InstallBindings()
    {
        Container.Bind<Player>().FromComponentOn(player.gameObject).AsSingle().NonLazy();
        Container.Bind<MapController>().FromComponentOn(mapController.gameObject).AsSingle().NonLazy();
        Container.Bind<GameManager>().FromComponentOn(gameManager.gameObject).AsSingle().NonLazy();

        Container.Bind<GameController>().FromNew().AsSingle().NonLazy();
        Container.Bind<CutSceneController>().FromNew().AsSingle().NonLazy();
    }
}
