// ************ @autor: Álvaro Repiso Romero *************
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace UI
{
    public class OptionMenuUI : MonoBehaviour
    {
        public static OptionMenuUI Instance { get; private set; }


        [Header("Option Buttons")]
        [SerializeField] private GameObject _firstSelectedObj;
        [SerializeField] private Button _returnButton;

        private event Action OnReturnButtonClicked;

        private void Awake()
        {
            Instance = this;
            _returnButton.onClick.AddListener( () => Hide() );
            Hide();
        }        

        public void Show( Action onReturnButtonClicked )
        {
            gameObject.SetActive( true );
            ServiceLocator.GetService<GameInputs>().OnCancelPerformed += GameInputs_OnCancelPerformed;
            OnReturnButtonClicked = onReturnButtonClicked;
            EventSystem.current.SetSelectedGameObject( _firstSelectedObj );
        }

        public void Hide()
        {
            gameObject.SetActive( false );
            ServiceLocator.GetService<GameInputs>().OnCancelPerformed -= GameInputs_OnCancelPerformed;
            OnReturnButtonClicked?.Invoke();
        }

        private void GameInputs_OnCancelPerformed()
        {
            if ( gameObject.activeSelf )
                Hide();
        }
    }
}