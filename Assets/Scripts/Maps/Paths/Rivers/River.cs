using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

public class River : MonoBehaviour
{
    [Header("Effects:")]
    [SerializeField] private ParticleSystem waterStartEffect;
    [SerializeField] private ParticleSystem waterEndEffect;
    
    [Header("River Main Line Connects:")]
    [SerializeField] private List<RiverPath> waterPaths = new List<RiverPath>();

    [Header("River Fall Connects:")]
    [SerializeField] private RiverPath fallPath;

    [Inject] private RiverSetting settings;

    private const string DissolveProperty = "_Dissove";
    private const string IsRunningProperty = "_IsRunning";
    private const string FoamSacleProperty = "_FoamTextureScale";

    private RiverModule _module;

    private LineRenderer lineRenderer;
    private Material _material;
    public Material Material => _material;
    private Tween _currentTween;
    private float _length;
    private float _dissolveValue;

    private void Start()
    {
        _module = GetComponent<RiverModule>();

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
        //Active Main Path only One Active
        RiverPath path = waterPaths.FirstOrDefault(x => x.target == target);
        if (path != null) path.active = value;

        //Active Fall
        fallPath.active = !waterPaths.Any(x => x.active == true);
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
        _currentTween?.Kill();

        //Ative water run material
        _currentTween = _material.DOFloat(_dissolveValue, DissolveProperty, GetDuration()).SetEase(Ease.Linear)
        //Triger on Complete
        .OnComplete(OnRiverComplete);
    }

   
    private void OnRiverComplete()
    {
        ActiveWaterCircleEffec(_dissolveValue > 0.5f);
        ActiveConnectRivers();
        _material.SetInt(IsRunningProperty, (int)_dissolveValue);

        if (_dissolveValue == 1) _module.Active();
    }

    private void ActiveConnectRivers()
    { 
        //Active Run connect paths
        foreach (RiverPath path in waterPaths)
        {
            path.target.ActiveRiverWater(_dissolveValue > 0.5f && path.active);
        }
        
        //Actice Run fall path
        fallPath.target?.ActiveRiverWater(_dissolveValue > 0.5f && fallPath.active);
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
        _material.SetVector(FoamSacleProperty, new Vector2(_length, lineRenderer.startWidth));
    }

    private float GetDuration()
    {
        return _length / settings.riverVelocity;
    }

    private void ActiveWaterForwardEffec(bool value)
    {
        if (waterStartEffect == null) return;
        if (value) waterStartEffect.Play(); else waterStartEffect.Stop();
    }

    private void ActiveWaterCircleEffec(bool value)
    {
        if (waterEndEffect == null) return;
        if (value) waterEndEffect.Play(); else waterEndEffect.Stop();
    }
}


[Serializable]
public class RiverPath
{
    public River target;
    public bool active = false;
}

  
