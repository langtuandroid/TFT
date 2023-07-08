using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalPauseInventory : MonoBehaviour
{
    private GameInputs gameInputs;

    [SerializeField]
    GameObject _pause;

    // Start is called before the first frame update
    void Start()
    {
        gameInputs = ServiceLocator.GetService<GameInputs>();
        gameInputs.OnPausePerformed += OnPausePerformed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPausePerformed()
    {
        _pause.SetActive(!_pause.activeSelf);
    }
}
