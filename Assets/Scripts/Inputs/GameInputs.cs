// ************ @autor: �lvaro Repiso Romero *************
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputs
{
    // Gameplay
    public event Action OnJumpButtonStarted;
    public event Action OnJumpButtonCanceled;
    public event Action OnPhysicActionButtonStarted;
    public event Action OnMediumAttackButtonStarted;
    public event Action OnMediumAttackButtonCanceled;
    public event Action OnWeakAttackButtonStarted;
    public event Action OnWeakAttackButtonCanceled;
    public event Action OnStrongAttackPerformed;
    public event Action OnSecondaryPerformed;

    // UI
    public event Action OnPausePerformed;
    public event Action OnCancelPerformed;
    public event Action OnSubmitPerformed;
    public event Action OnNextMenuPerformed;
    public event Action OnPrevMenuPerformed;

    // TODO: Definir aquí la función OnNavigatePerformed
    // que sea la que coja Inventory en vez de definir ahí el
    // NavigateAction.performed
    public InputAction NavigateAction;


    private PlayerInputActions _playerInputActions;
    private InputAction _moveAction;

    private OptionsSave _options;
    private MonoTimer _rumbleTimer;
    private GameStatus _gameStatus;

    public GameInputs(OptionsSave options, GameStatus gameStatus)
    {
        _playerInputActions = new PlayerInputActions();
        _options = options;
        _gameStatus = gameStatus;
        InitPlayerGroundMode();
        InitMenuUIMode();

        _gameStatus.OnGameStateChanged += GameStatus_OnGameStateChanged;
    }

    private void GameStatus_OnGameStateChanged( GameStatus.GameState gameState )
    {
        switch ( gameState )
        {
            case GameStatus.GameState.MenuUI:
                _playerInputActions.UI.Enable();
                _playerInputActions.PlayerGround.Disable();
                break;
            case GameStatus.GameState.GamePlay:
                _playerInputActions.UI.Disable();
                _playerInputActions.PlayerGround.Enable();
                break;
            case GameStatus.GameState.Inactive:
                _playerInputActions.UI.Disable();
                _playerInputActions.PlayerGround.Disable();
                break;
        }
    }

    public void RumblePad(float lowFrequency, float highFrequency, float durationSeconds)
    {
        if (_options.isVibrationActive)
        {
            if (Gamepad.current == null)
                return;

            if (!_rumbleTimer)
            {
                _rumbleTimer = new GameObject("Rumble", typeof(MonoTimer)).GetComponent<MonoTimer>();
                _rumbleTimer.OnDestroyObject = () => _rumbleTimer = null;
            }

            Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);

            _rumbleTimer.StartTimer(() => Gamepad.current.SetMotorSpeeds(0, 0), durationSeconds);
        }
    }

    // ***************************** PlayerGround *****************************

    /// <returns>A direction vector2 normalized from inputs</returns>
    public Vector2 GetDirectionNormalized()
    {
        return _moveAction.ReadValue<Vector2>().normalized;
    }

    private void InitPlayerGroundMode()
    {
        _playerInputActions.PlayerGround.Enable();
        _moveAction = _playerInputActions.PlayerGround.Move;

        _playerInputActions.PlayerGround.Jump.started += Jump_started;
        _playerInputActions.PlayerGround.Jump.canceled += Jump_canceled;
        _playerInputActions.PlayerGround.PhysicAction.started += PhysicAction_started;
        _playerInputActions.PlayerGround.MediumAttack.started += MediumAttack_started;
        _playerInputActions.PlayerGround.MediumAttack.canceled += MediumAttack_canceled;
        _playerInputActions.PlayerGround.WeakAttack.started += WeakAttack_started;
        _playerInputActions.PlayerGround.WeakAttack.canceled += WeakAttack_canceled;
        _playerInputActions.PlayerGround.Pause.performed += Pause_performed;

        _playerInputActions.PlayerGround.StrongAttack.performed += StrongAttack_performed;
        _playerInputActions.PlayerGround.Secondary.performed += Secondary_performed;
    }

    private void Secondary_performed(InputAction.CallbackContext ctx)
    {
        OnSecondaryPerformed?.Invoke();
    }

    private void StrongAttack_performed(InputAction.CallbackContext ctx)
    {
        OnStrongAttackPerformed?.Invoke();
    }

    private void PhysicAction_started(InputAction.CallbackContext ctx)
    {
        OnPhysicActionButtonStarted?.Invoke();
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
    }

    // ***************************** UI *****************************

    private void InitMenuUIMode()
    {
        _playerInputActions.UI.Enable();
        _playerInputActions.UI.Cancel.performed += Cancel_Performed;
        _playerInputActions.UI.Submit.performed += Submit_Performed;
        _playerInputActions.UI.NextMenu.performed += NextMenu_Performed;
        _playerInputActions.UI.PrevMenu.performed += PrevMenu_Performed;

        NavigateAction = _playerInputActions.UI.Navigate;
    }

    private void Cancel_Performed(InputAction.CallbackContext ctx)
    {
        OnCancelPerformed?.Invoke();
    }

    private void Submit_Performed(InputAction.CallbackContext ctx)
    {
        OnSubmitPerformed?.Invoke();
    }

    private void NextMenu_Performed(InputAction.CallbackContext ctx)
    {
        OnNextMenuPerformed?.Invoke();
    }

    private void PrevMenu_Performed(InputAction.CallbackContext ctx)
    {
        OnPrevMenuPerformed?.Invoke();
    }
}
