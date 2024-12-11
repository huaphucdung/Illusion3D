using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameData gameData;
    public static GameData GameData { get; private set; }
    public MapController mapController;


    private void Awake()
    {
        GameData = gameData;
    }

    private void Start()
    {
        Initalize();
    }

    [ContextMenu("Reset")]
    public void Test()
    {
        EventBus<ResetEvent>.Raise(new ResetEvent());
    }

    public void Initalize()
    {
        InputManager.Initialize();
        InputManager.ActiveInput(true);
        mapController.Initiliaze(player);
        AddInputAction();
    }


    public void SetMapControler(MapController mapController)
    {
        this.mapController = mapController;
    }

    private void AddInputAction()
    {
        InputManager.click += OnClick;
    }

    private void RemoveInputAction()
    {
        InputManager.click -= OnClick;
    }

    #region Callback Methods
    private void OnClick()
    {
        if (player.IsWalking) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Walkable block = hit.transform.GetComponent<Walkable>();
            if (block == null) return;
            player.SetClickedPosition(block);
        }
    }
    #endregion
}

[Serializable]
public class GameData
{
    [Header("Walk:")]
    [Range(0f, 2f)] public float WalkDuriation = 0.2f;
    [Range(0f, 2f)] public float WalkRotationDuriation = 0.2f;

    [Header("Ladder:")]
    [Range(0f, 2f)] public float InAndOutLadderDuriation = 0.2f;
    [Range(0f, 2f)] public float LadderDuriation = 0.2f;
    [Range(0f, 2f)] public float LadderRotationDuriation = 0.2f;

    [Header("Bezier:")]
    [Range(0f, 2f)] public float BezierDuriation = 0.2f;
    [Range(0f, 2f)] public float BezierRotationDuriation = 0.2f;

    [Header("Circle:")]
    [Range(0f, 2f)] public float QuaterCircleDuriationByOneUnitRadius = 0.2f;
    [Range(0f, 2f)] public float QuaterCircleRotationDuriationByOneUnitRadius = 0.2f;

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
