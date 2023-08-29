using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenu : MonoBehaviour
{
    #region SerializeFields

    [Header("Other menus")]
    [SerializeField]
    [Tooltip("Menú de inventario")]
    private GameObject _inventoryMenu;

    #endregion

    #region Private variables

    // SERVICES
    private GameInputs _gameInputs;
    private GameStatus _gameStatus;

    #endregion

    #region Unity methods

    private void Start()
    {
        // EVENTS
        // GameInputs
        _gameInputs = ServiceLocator.GetService<GameInputs>();
        _gameInputs.OnCancelPerformed += GameInputs_OnCancel;
        _gameInputs.OnNextMenuPerformed += GameInputs_OnNextMenu;

        // GameStatus
        _gameStatus = ServiceLocator.GetService<GameStatus>();

    }

    private void OnEnable()
    {
        if (_gameInputs != null)
        {
            _gameInputs.OnCancelPerformed += GameInputs_OnCancel;
            _gameInputs.OnNextMenuPerformed += GameInputs_OnNextMenu;
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

    private void GameInputs_OnNextMenu()
    {
        // TODO: Meter animación
        gameObject.SetActive(false);
        _inventoryMenu.SetActive(true);
    }

    private void QuitInputsEvents()
    {
        if (_gameInputs != null)
        {
            _gameInputs.OnCancelPerformed -= GameInputs_OnCancel;
            _gameInputs.OnNextMenuPerformed -= GameInputs_OnNextMenu;
        }
    }


    #endregion


    #endregion


}
