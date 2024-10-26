using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;

    public MapController mapController;
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
