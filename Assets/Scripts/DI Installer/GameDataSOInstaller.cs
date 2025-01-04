using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameDataSOInstaller", menuName = "Installers/GameDataSOInstaller")]
public class GameDataSOInstaller : ScriptableObjectInstaller<GameDataSOInstaller>
{
    [SerializeField] private PlayerSetting playerSetting;
    [SerializeField] private RiverSetting riverSetting;
    public override void InstallBindings()
    {
        Container.BindInstance(playerSetting).AsSingle().NonLazy();
        Container.BindInstance(riverSetting).AsSingle().NonLazy();
    }
}


[Serializable]
public class PlayerSetting
{
    [Header("State:")]
    [Range(0f, 1f)] public float StateChangeDuriation = 0.25f;
    [Header("Walk:")]
    [Range(0f, 1f)] public float WalkDuriation = 0.2f;
    [Range(0f, 1f)] public float WalkRotationDuriation = 0.2f;

    [Header("Ladder:")]
    [Range(0f, 1f)] public float InAndOutLadderDuriation = 0.2f;
    [Range(0f, 1f)] public float LadderDuriation = 0.3f;
    [Range(0f, 1f)] public float LadderRotationDuriation = 0.1f;
    [Range(0f, 1f)] public float TimeEndLadder = 0.3f;

    [Header("Bezier:")]
    [Range(0f, 1f)] public float BezierDuriation = 0.2f;
    [Range(0f, 1f)] public float BezierRotationDuriation = 0.2f;

    [Header("Circle:")]
    [Range(0f, 1f)] public float QuaterCircleDuriationByOneUnitRadius = 0.3f;
    [Range(0f, 1f)] public float QuaterCircleRotationDuriationByOneUnitRadius = 0.2f;


    public readonly Dictionary<Direction, Vector3> directionDictioanry = new Dictionary<Direction, Vector3>()
    {
        {Direction.Forward, Vector3.forward },
        {Direction.Back, Vector3.back },
        {Direction.Left, Vector3.left },
        {Direction.Right, Vector3.right },
        {Direction.Up, Vector3.up },
        {Direction.Down, Vector3.down },
    };

    public Vector3 GetDirection(Direction direction)
    {
        if (directionDictioanry.TryGetValue(direction, out Vector3 value)) return value;
        return Vector3.forward;
    }
}

[Serializable]
public class RiverSetting
{
    [Range(0.1f, 50f)] public float riverVelocity = 8f;
}