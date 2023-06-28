using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TemporalPause : MonoBehaviour
{

    [SerializeField]
    private GameObject _pausePanel;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            _pausePanel.SetActive(!_pausePanel.activeSelf);
        
    }


}
