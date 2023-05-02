// ************ @autor: Álvaro Repiso Romero *************
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputs
{
    // Gameplay
    public event Action OnSouthButtonStarted;
    public event Action OnSouthButtonCanceled;
    public event Action OnEastButtonPerformed;
    // UI
    public event Action OnCancelPerformed;
    public event Action OnPausePerformed;


    private PlayerInputActions _playerInputActions;
    private InputAction _moveAction;

    public GameInputs() 
    {
        _playerInputActions = new PlayerInputActions();
        PlayerGroundMode();
        MenuModeEnable();
    }


    /// <returns>A direction vector2 normalized from inputs</returns>
    public Vector2 GetDirectionNormalized()
    {
        return _moveAction.ReadValue<Vector2>().normalized;
    }

    private void PlayerGroundMode()
    {
        _playerInputActions.PlayerGround.Enable();
        _moveAction = _playerInputActions.PlayerGround.Move;
        _playerInputActions.PlayerGround.South.started += South_started;
        _playerInputActions.PlayerGround.South.canceled += South_canceled;
        _playerInputActions.PlayerGround.East.performed += East_performed;
        _playerInputActions.PlayerGround.Pause.performed += Pause_performed;
    }

    private void East_performed( InputAction.CallbackContext ctx )
    {
        OnEastButtonPerformed?.Invoke();
    }
    
    private void South_canceled( InputAction.CallbackContext ctx )
    {
        OnSouthButtonCanceled?.Invoke();
    }

    private void South_started( InputAction.CallbackContext ctx )
    {
        OnSouthButtonStarted?.Invoke();
    }

    private void Pause_performed( InputAction.CallbackContext ctx )
    {
        OnPausePerformed?.Invoke();
        // TODO: if game is paused -> MenuMode()
    }

    private void MenuModeEnable()
    {
        _playerInputActions.UI.Enable();
        _playerInputActions.UI.Cancel.performed += Cancel_Performed;
    }

    private void MenuModeDisable()
    {
        _playerInputActions.UI.Cancel.performed -= Cancel_Performed;
        _playerInputActions.UI.Disable();
    }

    private void Cancel_Performed( InputAction.CallbackContext ctx )
    {
        OnCancelPerformed?.Invoke();
    }
}
