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

    private void Awake()
    {
        _isPaused = false;
    }

    private void Start()
    {
        _gameInputs = ServiceLocator.GetService<GameInputs>();
        _gameInputs.OnPausePerformed += OnPausePerformed;


        _gameStatus = ServiceLocator.GetService<GameStatus>();
        _gameStatus.AskChangeToMenuUIState();
        _gameStatus.AskChangeToGamePlayState();
        _gameStatus.OnGameStateChanged += GameStatus_OnGameStateChanged;
    }

    private void OnDestroy()
    {
        _gameInputs.OnPausePerformed -= OnPausePerformed;
    }

    private void OnPausePerformed()
    {
        if (!_isPaused)
            _gameStatus.AskChangeToMenuUIState();
        else
            _gameStatus.AskChangeToGamePlayState();
    }

    private void GameStatus_OnGameStateChanged(GameStatus.GameState gameState)
    {
        switch (gameState)
        {
            case GameStatus.GameState.MenuUI:
                _pause.SetActive(true);
                _isPaused = true;
                break;
            case GameStatus.GameState.GamePlay:
                _pause.SetActive(false);
                _isPaused = false;
                break;
            case GameStatus.GameState.Inactive:
                // No hace nada
                break;
        }
    }

}
