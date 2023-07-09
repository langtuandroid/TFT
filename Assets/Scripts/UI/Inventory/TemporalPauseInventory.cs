using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalPauseInventory : MonoBehaviour
{
    private GameInputs gameInputs;

    [SerializeField]
    GameObject _pause;

    private void Start()
    {
        gameInputs = ServiceLocator.GetService<GameInputs>();
        gameInputs.OnPausePerformed += OnPausePerformed;
    }


    private void OnPausePerformed()
    {
        _pause.SetActive(!_pause.activeSelf);
    }
}
