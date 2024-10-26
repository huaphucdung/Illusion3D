using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager 
{
    public static GameInput input { get; private set; }
    public static GameInput.PlayerActions playerActions { get; private set; }
    public static GameInput.UIActions uiActions { get; private set; }

    private const float MAX_MOUSEMOVE_DECTECT = 20f;
    private static bool isPress;

    public static void Initialize()
    {
        if (input == null) input = new GameInput();
        playerActions = input.Player;
        uiActions = input.UI;
        input.Enable();
    }

    public static void ActiveInput(bool value)
    {
        if (value)
        {
            input.Enable();
            AddPlayerAction();
        }
        else
        {
            input.Disable();
            RemovePlayerAction();
        }
    }

    #region Player Input Action
    public static event Action click;

    private static void AddPlayerAction()
    {
        playerActions.Click.started += OnClickStarted;
        playerActions.Click.canceled += OnClickCancled;
        playerActions.ClickMove.performed += OnClickMove;
    }
    private static void RemovePlayerAction()
    {
        playerActions.Click.started -= OnClickStarted;
        playerActions.Click.canceled -= OnClickCancled;
        playerActions.ClickMove.performed -= OnClickMove;
    }



    private static void OnClickStarted(InputAction.CallbackContext context)
    {
        Debug.Log("Click started");
        isPress = true;
    }
    private static void OnClickCancled(InputAction.CallbackContext context)
    {
        if(isPress)
        {
            Debug.Log("Click performed");
            click?.Invoke();
        }
        Debug.Log("Click canceld");
        isPress = false;
    }
    private static void OnClickMove(InputAction.CallbackContext context)
    {
       if(context.ReadValue<Vector2>().sqrMagnitude > MAX_MOUSEMOVE_DECTECT)
       {
            isPress = false;
       }
    }

    #endregion
}
