using AYellowpaper.SerializedCollections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ButtonModule : BlockModule
{
    [Header("Camera Shake Settings:")]
    [SerializeField] private float strenght = .5f;
    [SerializeField] private float duriation = 1;

    [Header("References:")]
    [SerializeField] private GameObject buttonObject;

    [SerializedDictionary("Block Ground", "Position And Rotation Trigger")]
    [SerializeField] private SerializedDictionary<BlockGroup, PosistionAndRotationTrigger> blockEventDictionary;

    [SerializedDictionary("Source", "Active")]
    [SerializeField] private SerializedDictionary<River, bool> riverSourceDictionary;

    
    private bool canPress;
    private EventBinding<ResetEvent> resetEventBiding;

    private void Awake()
    {
        resetEventBiding = new EventBinding<ResetEvent>(ResetButton);
        canPress = true;
    }

    private void OnEnable()
    {
        EventBus<ResetEvent>.Register(resetEventBiding);
    }

    private void OnDisable()
    {
        EventBus<ResetEvent>.Deregister(resetEventBiding);
    }

    private void ResetButton()
    {
        canPress = true;
        buttonObject.transform.localPosition = new Vector3(0, 0.5f, 0);
    }

    public override void Active()
    {
        
    }

    public override void Active(Player player) {
        if (!canPress) return;

        foreach (KeyValuePair<BlockGroup, PosistionAndRotationTrigger> blockEvent in blockEventDictionary)
        {
            BlockGroup key = blockEvent.Key;
            PosistionAndRotationTrigger value = blockEvent.Value;
            key.SetPositionAndRotaion(value.position, value.rotation);
        }

        buttonObject?.transform.DOLocalMoveY(0.41f, duriation);

        EventBus<RiverSourceActiveEvent>.Raise(new RiverSourceActiveEvent { sourceDictionary = riverSourceDictionary});
        EventBus<CameraShakeEvent>.Raise(new CameraShakeEvent {  strenght = this.strenght, duriation = this.duriation});
    
        canPress = false;
    }
}
