using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class River : MonoBehaviour
{
    [Header("Effects:")]
    [SerializeField] private ParticleSystem waterRunForwardEffect;
    [SerializeField] private ParticleSystem waterRunCircleEffect;
    
    [Header("River Connects:")]
    [SerializeField] private List<RiverPath> waterPaths = new List<RiverPath>();

    [Header("Settings:")]
    [SerializeField, Range(0.1f, 10f)] private float waterVelocity = 4f;
    [SerializeField, Range(0f, 5f)] private float diffDistanceBeforeFull; 
    
    private const string DissolveProperty = "_Dissove";
    private const string IsRunningProperty = "_IsRunning";
    private const string FoamSacleProperty = "_FoamTextureScale";

    private LineRenderer lineRenderer;
    private Material _material;
    public Material Material => _material;
    private Tween _currentTween;
    private float _length;
    private float _dissolveValue;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        _material = lineRenderer.material;
        CaculateLengthWater();
        SetTexureScaleByLength();

        ActiveWaterForwardEffec(false);
        ActiveWaterCircleEffec(false);

        _material.SetFloat(DissolveProperty, 0);
        _material.SetInt(IsRunningProperty, 0);
    }

    public void ActivePath(River target, bool value)
    {
        RiverPath path = waterPaths.FirstOrDefault(x => x.target == target);
        if (path == null) return;
        path.active = value;
    }

    public void ActiveRiverWater(bool value)
    {
        
        float newValue = value ? 1 : 0;
        //Check current is same state then inoke and active path
        if (_dissolveValue == newValue)
        {
            ActiveConnectRivers();
            return;
        }
        _dissolveValue = newValue;

        //Active Effect Water Forward
        ActiveWaterForwardEffec(value);
        
        //Ative water run material
        _currentTween?.Kill();
        _currentTween = _material.DOFloat(_dissolveValue, DissolveProperty, GetDuration()).SetEase(Ease.Linear)
        //Trigger on Update 
        .OnUpdate(OnRiverUpdate)
        //Triger on Complete
        .OnComplete(OnRiverComplete);
    }

    private void OnRiverUpdate()
    {
        if (_currentTween.Elapsed() < GetTimeTrigger()) return;
        _currentTween.OnUpdate(null);

        //Active Effect Water Circle
        ActiveWaterCircleEffec(_dissolveValue > 0.5f);
        ActiveConnectRivers();
    }

    private void OnRiverComplete()
    {
        _material.SetInt(IsRunningProperty, (int)_dissolveValue);
    }

    private void ActiveConnectRivers()
    { 
        //Active connect paths
        foreach (RiverPath path in waterPaths)
        {
            path.target.ActiveRiverWater(_dissolveValue > 0.5f && path.active);
        }
    }

    private void CaculateLengthWater()
    {
        _length = 0;
        if (lineRenderer.positionCount < 2) return;
        Vector3[] pointsInLine;
        pointsInLine = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(pointsInLine);
        for (int i = 0; i < pointsInLine.Length - 1; i++)
        {
            _length += Vector3.Distance(pointsInLine[i], pointsInLine[i + 1]);
        }
    }

    private void SetTexureScaleByLength()
    {
        _material.SetVector(FoamSacleProperty, new Vector2(_length, 1));
    }

    private float GetDuration()
    {
        return _length / waterVelocity;
    }

    private float GetTimeTrigger()
    {
        return (_length - diffDistanceBeforeFull) / waterVelocity;
    }

    private void ActiveWaterForwardEffec(bool value)
    {
        if (waterRunForwardEffect == null) return;
        if (value) waterRunForwardEffect.Play(); else waterRunForwardEffect.Stop();
    }

    private void ActiveWaterCircleEffec(bool value)
    {
        if (waterRunCircleEffect == null) return;
        if (value) waterRunCircleEffect.Play(); else waterRunCircleEffect.Stop();
    }
}


[Serializable]
public class RiverPath
{
    public River target;
    public bool active = false;
}

  
