using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalPauseInventory : MonoBehaviour
{
    private GameInputs _gameInputs;
    private GameStatus _gameStatus;
    private bool _isPaused;

    [SerializeField]
    GameObject _pause;

    private void Start()
    {
        _gameInputs = ServiceLocator.GetService<GameInputs>();
        _gameInputs.OnPausePerformed += OnPausePerformed;


        _gameStatus = ServiceLocator.GetService<GameStatus>();
        _gameStatus.AskChangeToMenuUIState();
        _gameStatus.AskChangeToGamePlayState();
    }

    private void OnDestroy()
    {
        _gameInputs.OnPausePerformed -= OnPausePerformed;
    }

    private void OnPausePerformed()
    {
        _pause.SetActive(!_pause.activeSelf);

        if (_pause.activeSelf)
            _gameStatus.AskChangeToMenuUIState();
        else
            _gameStatus.AskChangeToGamePlayState();
    }

}
