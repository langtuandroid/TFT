using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus
{
    private bool isPaused = false;
    public event Action<bool> OnPauseStateChanged;
    public Action OnPlayerStopInteraction; 

    public void PauseGame()
    {
        isPaused = true;
        //Time.timeScale = 0f;
        OnPauseStateChanged?.Invoke(isPaused);
    }

    public void ResumeGame()
    {
        isPaused = false;
        //Time.timeScale = 1f;
        OnPauseStateChanged?.Invoke(isPaused);
    }
    
    public void PlayerStopInteraction()
    {
        OnPlayerStopInteraction?.Invoke();
    }
}
