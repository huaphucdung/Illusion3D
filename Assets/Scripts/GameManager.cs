using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;

    public GameInput input {  get; private set; }

    public MapController mapController;
    private void Start()
    {
        input = new GameInput();
        input.Enable();
        input.Player.Click.canceled += OnClick;
        Initalize();
    }

    public void Initalize()
    {
        mapController.Initiliaze(player);
    }


    public void SetMapControler(MapController mapController)
    {
        //Remove old map
        this.mapController = mapController;
        //Spawn new map
    }


    #region Callback Methods
    private void OnClick(InputAction.CallbackContext context)
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
