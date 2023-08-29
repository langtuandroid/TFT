using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    #region SerializeFields

    [Header("Other menus")]
    [SerializeField]
    [Tooltip("Menú de inventario")]
    private GameObject _inventoryMenu;

    #endregion

    #region Private variables

    // SERVICES
    // GameInputs
    private GameInputs _gameInputs;

    // GameStatus
    private GameStatus _gameStatus;

    #endregion

    #region Unity methods

    private void Start()
    {
        // EVENTS
        // GameInputs
        _gameInputs = ServiceLocator.GetService<GameInputs>();
        _gameInputs.OnCancelPerformed += GameInputs_OnCancel;
        _gameInputs.OnPrevMenuPerformed += GameInputs_OnPrevMenu;

        // GameStatus
        _gameStatus = ServiceLocator.GetService<GameStatus>();
        _gameStatus.OnGameStateChanged += GameStatus_OnGameStateChanged;
    }

    private void OnEnable()
    {
        if (_gameInputs != null)
        {
            _gameInputs.OnCancelPerformed += GameInputs_OnCancel;
            _gameInputs.OnPrevMenuPerformed += GameInputs_OnPrevMenu;
        }

        if (_gameStatus != null)
            _gameStatus.OnGameStateChanged += GameStatus_OnGameStateChanged;
    }

    private void OnDisable()
    {
        QuitEvents();
    }

    private void OnDestroy()
    {
        QuitEvents();
    }

    #endregion

    #region Private methods

    #region GameInputs

    private void GameInputs_OnCancel()
    {
        _gameStatus.AskChangeToGamePlayState();
        gameObject.SetActive(false);
    }

    private void GameInputs_OnPrevMenu()
    {
        _inventoryMenu.SetActive(true);
        // TODO: Meter animación
        gameObject.SetActive(false);
    }

    private void QuitEvents()
    {
        if (_gameInputs != null)
        {
            _gameInputs.OnCancelPerformed -= GameInputs_OnCancel;
            _gameInputs.OnPrevMenuPerformed -= GameInputs_OnPrevMenu;
        }

        if (_gameStatus != null)
            _gameStatus.OnGameStateChanged -= GameStatus_OnGameStateChanged;
    }

    #endregion

    #region GameStatus

    private void GameStatus_OnGameStateChanged(GameStatus.GameState gameState)
    {
        switch (gameState)
        {
            case GameStatus.GameState.MenuUI:
                // No hace nada
                break;
            case GameStatus.GameState.GamePlay:
                // Desactiva elemento
                gameObject.SetActive(false);
                break;
            case GameStatus.GameState.Inactive:
                // No hace nada
                break;
        }
    }

    #endregion

    #endregion

}
