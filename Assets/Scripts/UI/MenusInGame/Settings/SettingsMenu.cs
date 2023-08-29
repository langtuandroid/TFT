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
    }

    private void OnEnable()
    {
        if (_gameInputs != null)
        {
            _gameInputs.OnCancelPerformed += GameInputs_OnCancel;
            _gameInputs.OnPrevMenuPerformed += GameInputs_OnPrevMenu;
        }
    }

    private void OnDisable()
    {
        QuitInputsEvents();
    }

    private void OnDestroy()
    {
        QuitInputsEvents();
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

    private void QuitInputsEvents()
    {
        if (_gameInputs != null)
        {
            _gameInputs.OnCancelPerformed -= GameInputs_OnCancel;
            _gameInputs.OnPrevMenuPerformed -= GameInputs_OnPrevMenu;
        }
    }

    #endregion

    #endregion

}
