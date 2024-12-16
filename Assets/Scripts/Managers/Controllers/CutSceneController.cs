using UnityEngine.Playables;

public class CutSceneController
{
    private PlayableDirector _director;

    private EventBinding<CutSceneStartEvent> cutSceneStartEventBinding;
    private EventBinding<CutSceneEndEvent> cutSceneEndEventBinding;

    public void PlayCutScene(PlayableDirector director)
    {
        if (director == null) return;
        if (_director) _director.stopped -= OnCutSceneEnd;
       
        OnCutSceneStart();
        _director = director;
    
        _director.stopped += OnCutSceneEnd;

        _director?.Play();
        OnCutSceneStart();
    }


    private void OnCutSceneStart()
    {
        EventBus<CutSceneStartEvent>.Raise(new CutSceneStartEvent());
    }
    private void OnCutSceneEnd(PlayableDirector pd)
    {
        EventBus<CutSceneEndEvent>.Raise(new CutSceneEndEvent());
    }
}


public struct CutSceneStartEvent : IEvent
{ }

public struct CutSceneEndEvent : IEvent
{ }