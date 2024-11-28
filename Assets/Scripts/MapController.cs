using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class MapController : MonoBehaviour
{
    [Header("Walk Points:")]
    [SerializeField] private Walkable startPoint;
    [SerializeField] private Walkable endPoint;

    [Header("CutScenes:")]
    [SerializeField] private List<PlayableDirector> cutScenes;

    private int currentCuteScene;
    private EventBinding<TriggertEvent> triggerEvenBinding;
    

    private void Awake()
    {
        triggerEvenBinding = new EventBinding<TriggertEvent>(TriggerEvent);
    }

    public void Initiliaze(Player player)
    {
        player.SetPosition(startPoint);
        currentCuteScene = 0;
    }

    public void Reset()
    {
       
    }

    private void OnEnable()
    {
        EventBus<TriggertEvent>.Register(triggerEvenBinding);
    }

    private void OnDisable()
    {
        EventBus<TriggertEvent>.Deregister(triggerEvenBinding);
    }

    private void TriggerEvent(TriggertEvent @event)
    {
        switch (@event.type)
        {
            case TriggerType.CutScene:
                if (currentCuteScene >= cutScenes.Count) return;
                cutScenes[currentCuteScene].Play();
                currentCuteScene++; 
                break;
            default:
                break;
        }
    }

}
