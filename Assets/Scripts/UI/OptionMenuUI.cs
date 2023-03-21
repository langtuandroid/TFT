using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class OptionMenuUI : MonoBehaviour
    {
        public static OptionMenuUI Instance { get; private set; }


        [Header("Option Buttons")]
        [SerializeField] private Button _firstSlotButton;
        [SerializeField] private Button _returnButton;


        private Action _onReturn;


        private void Awake()
        {
            Instance = this;
            SetButtonEvents();
            Hide();
        }

        private void SetButtonEvents()
        {
            _returnButton.onClick.AddListener( () => Hide() );
        }

        public void Show( Action onReturn )
        {
            gameObject.SetActive( true );
            _returnButton.Select();
            _onReturn = onReturn;
        }

        public void Hide()
        {
            gameObject.SetActive( false );
            _onReturn?.Invoke();
        }
    }
}