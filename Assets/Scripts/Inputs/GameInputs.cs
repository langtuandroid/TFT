// ************ @autor: Álvaro Repiso Romero *************
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputs
{
    //public static GameInputs Instance { get; private set; }


    public event Action OnCancelPerformed;
    public event Action OnPausePerformed;


    private PlayerInputActions _playerInputActions;

    public GameInputs() 
    {
        _playerInputActions = new PlayerInputActions();
        PlayerGroundMode();
        MenuModeEnable();
    }

    //private void Awake()
    //{
    //    Instance = this;
    //    _playerInputActions = new PlayerInputActions();

    //    PlayerGroundMode();
    //    MenuModeEnable();
    //}

    private void PlayerGroundMode()
    {
        _playerInputActions.PlayerGround.Enable();
        _playerInputActions.PlayerGround.Pause.performed += Pause_performed;
    }

    private void Pause_performed( InputAction.CallbackContext obj )
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

    private void Cancel_Performed( InputAction.CallbackContext obj )
    {
        OnCancelPerformed?.Invoke();
    }
}
