using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.Rendering.DebugUI;

public class CameraShakeModule : MonoBehaviour
{
    private CinemachineVirtualCamera _cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin _basicMultiChannelPerlin;

    private EventBinding<CameraShakeEvent> _cameraShakeEventBinding;

    private void Awake()
    {
        _cameraShakeEventBinding = new EventBinding<CameraShakeEvent>(DoCameraShake);
    }

    private void Start()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (_cinemachineVirtualCamera == null) return;
        _basicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _basicMultiChannelPerlin.m_AmplitudeGain = 0;
    }


    private void OnEnable()
    {
        EventBus<CameraShakeEvent>.Register(_cameraShakeEventBinding);
    }

    private void OnDisable()
    {
        EventBus<CameraShakeEvent>.Deregister(_cameraShakeEventBinding);
    }

    private void DoCameraShake(CameraShakeEvent @event)
    {
        DOVirtual.Float(0f, @event.strenght, @event.duriation, null)
            .OnStart(CameraShakeStart)
            .OnComplete(CameraShakeComplete);


        void CameraShakeStart()
        {
            if (_basicMultiChannelPerlin == null) return;
            _basicMultiChannelPerlin.m_AmplitudeGain = @event.strenght;
        }

        void CameraShakeComplete()
        {
            if (_basicMultiChannelPerlin == null) return;
            _basicMultiChannelPerlin.m_AmplitudeGain = 0;
        }
    }

  
}

public struct CameraShakeEvent : IEvent
{
    public float strenght;
    public float duriation;
}

