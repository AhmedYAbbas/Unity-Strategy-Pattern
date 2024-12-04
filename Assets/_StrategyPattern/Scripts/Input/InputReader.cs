using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Scriptable Objects/InputReader")]
public class InputReader : ScriptableObject, GameInput.IPlayerActions, GameInput.IUIActions
{
    public Action<Vector2> MoveEvent;
    public Action<Vector2> LookEvent;
    public Action FireEvent;
    public Action DrawEvent;
    
    private GameInput _gameInput;

    private void OnEnable()
    {
        if (_gameInput == null)
        {
            _gameInput = new GameInput();
            
            _gameInput.Player.SetCallbacks(this);
            _gameInput.UI.SetCallbacks(this);
            SetGameplay();
        }
    }

    private void OnDisable()
    {
        _gameInput.Player.RemoveCallbacks(this);
        _gameInput.UI.RemoveCallbacks(this);
        _gameInput.Player.Disable();
        _gameInput.UI.Disable();
    }
    
    private void SetGameplay()
    {
        _gameInput.Player.Enable();
        _gameInput.UI.Disable();
    }

    private void SetUI()
    {
        _gameInput.Player.Disable();
        _gameInput.UI.Enable();
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            FireEvent?.Invoke();
    }

    public void OnDraw(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            DrawEvent?.Invoke();
    }
}
