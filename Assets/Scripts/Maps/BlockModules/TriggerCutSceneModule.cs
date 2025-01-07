using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Zenject;

public class TriggerCutSceneModule : BlockModule
{
    [SerializeField] private PlayableDirector cutScene;

    [Inject] private GameManager gameManager;

    private void Start()
    {
        if (cutScene == null) return;
        cutScene.stopped += FinishCutScene;
    }



    public override void Active()
    {
    }

    public override void Active(Player player)
    {
        player.transform.parent = null;
        cutScene?.Play();
    }

    private void FinishCutScene(PlayableDirector director)
    {
        gameManager.NextMiniMap();
    }
}

