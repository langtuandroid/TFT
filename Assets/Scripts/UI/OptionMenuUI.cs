// ************ @autor: Álvaro Repiso Romero *************
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


        private event Action OnReturnButtonClicked;


        private void Awake()
        {
            Instance = this;
            SetButtonEvents();
            Hide();
        }

        private void Start()
        {
            ServiceLocator.GetService<GameInputs>().OnCancelPerformed += GameInputs_OnCancelPerformed;
        }

        private void OnDestroy()
        {
            ServiceLocator.GetService<GameInputs>().OnCancelPerformed -= GameInputs_OnCancelPerformed;
        }

        private void GameInputs_OnCancelPerformed()
        {
            if (gameObject.activeSelf)
                Hide();
        }

        private void SetButtonEvents()
        {
            _returnButton.onClick.AddListener( () => Hide() );
        }

        public void Show( Action OnReturnButtonClicked )
        {
            gameObject.SetActive( true );
            _returnButton.Select();
            this.OnReturnButtonClicked = OnReturnButtonClicked;
        }

        public void Hide()
        {
            gameObject.SetActive( false );
            OnReturnButtonClicked?.Invoke();
        }
    }
}