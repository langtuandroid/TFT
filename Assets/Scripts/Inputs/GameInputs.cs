// ************ @autor: Álvaro Repiso Romero *************
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputs
{
    // Gameplay
    public event Action OnJumpButtonStarted;
    public event Action OnJumpButtonCanceled;
    public event Action OnPhysicActionButtonPerformed;
    public event Action OnMediumAttackButtonStarted;
    public event Action OnMediumAttackButtonCanceled;
    public event Action OnWeakAttackButtonStarted;
    public event Action OnWeakAttackButtonCanceled;

    public event Action OnStrongAttackPerformed;

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
        _playerInputActions.PlayerGround.Jump.started += Jump_started;
        _playerInputActions.PlayerGround.Jump.canceled += Jump_canceled;
        _playerInputActions.PlayerGround.PhysicAction.performed += PhysicAction_performed;
        _playerInputActions.PlayerGround.MediumAttack.started += MediumAttack_started;
        _playerInputActions.PlayerGround.MediumAttack.canceled += MediumAttack_canceled;
        _playerInputActions.PlayerGround.WeakAttack.started += WeakAttack_started;
        _playerInputActions.PlayerGround.WeakAttack.canceled += WeakAttack_canceled;
        _playerInputActions.PlayerGround.Pause.performed += Pause_performed;

        _playerInputActions.PlayerGround.StrongAttack.performed += StrongAttack_performed;
    }

    private void StrongAttack_performed(InputAction.CallbackContext ctx)
    {
        OnStrongAttackPerformed?.Invoke();
    }

    private void PhysicAction_performed(InputAction.CallbackContext ctx)
    {
        OnPhysicActionButtonPerformed?.Invoke();
    }

    private void MediumAttack_started(InputAction.CallbackContext ctx)
    {
        OnMediumAttackButtonStarted?.Invoke();
    }

    private void MediumAttack_canceled(InputAction.CallbackContext ctx)
    {
        OnMediumAttackButtonCanceled?.Invoke();
    }

    private void WeakAttack_started(InputAction.CallbackContext ctx)
    {
        OnWeakAttackButtonStarted?.Invoke();
    }

    private void WeakAttack_canceled(InputAction.CallbackContext ctx)
    {
        OnWeakAttackButtonCanceled?.Invoke();
    }

    private void Jump_canceled(InputAction.CallbackContext ctx)
    {
        OnJumpButtonCanceled?.Invoke();
    }

    private void Jump_started(InputAction.CallbackContext ctx)
    {
        OnJumpButtonStarted?.Invoke();
    }

    private void Pause_performed(InputAction.CallbackContext ctx)
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

    private void Cancel_Performed(InputAction.CallbackContext ctx)
    {
        OnCancelPerformed?.Invoke();
    }
}
