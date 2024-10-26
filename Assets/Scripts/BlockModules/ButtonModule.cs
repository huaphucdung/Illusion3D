using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonModule : BlockModule
{
    [SerializedDictionary("Block Ground", "Position And Rotation Trigger")]
    [SerializeField] private SerializedDictionary<BlockGroup, PosistionAndRotationTrigger> blockEventDictionary;

    private bool isPress;
    private EventBinding<ResetEvent> resetEventBiding;

    private void OnEnable()
    {
        resetEventBiding = new EventBinding<ResetEvent>(ResetButton);
        EventBus<ResetEvent>.Register(resetEventBiding);
    }

    private void OnDisable()
    {
        EventBus<ResetEvent>.Deregister(resetEventBiding);
    }

    private void ResetButton()
    {
        isPress = false;
    }

    public override void Active()
    {
        
    }

    public override void Active(Player player) {
        foreach (KeyValuePair<BlockGroup, PosistionAndRotationTrigger> blockEvent in blockEventDictionary)
        {
            BlockGroup key = blockEvent.Key;
            PosistionAndRotationTrigger value = blockEvent.Value;
            key.SetPositionAndRotaion(value.position, value.rotation);
        }
    }
}
