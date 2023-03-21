using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionMenuUI : MonoBehaviour
{
    public static OptionMenuUI Instance { get; private set; }


    [Header("Option Buttons")]
    [SerializeField] private Button _firstSlotButton;
    [SerializeField] private Button _returnButton;


    private Action _onMainMenuReturn;


    private void Awake()
    {
        Instance = this;
        SetButtonEvents();
        Hide();
    }

    private void SetButtonEvents()
    {
        _returnButton.onClick.AddListener( () => {
            Hide();
            _onMainMenuReturn();
        } );
    }

    public void Show( Action onMainMenuReturn )
    {
        gameObject.SetActive( true );
        _returnButton.Select();
        _onMainMenuReturn = onMainMenuReturn;
    }

    private void Hide()
    {
        gameObject.SetActive( false );
    }
}
