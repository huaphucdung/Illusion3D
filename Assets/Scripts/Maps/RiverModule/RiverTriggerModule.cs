using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverTriggerModule : RiverModule
{
    [SerializeField] private Material material;
    [SerializeField] private Color color;
    private bool isTrigger;

    public override void Active()
    {
        if (isTrigger) return;
        EventBus<RiverTriggerEvent>.Raise(new RiverTriggerEvent() { river = GetComponent<River>() });
        material.DOColor(color, 1f);
        isTrigger = true;
    }

    protected override void ResetModule()
    {
        base.ResetModule();
        material.color = Color.gray;
        isTrigger = false;
    }
}
