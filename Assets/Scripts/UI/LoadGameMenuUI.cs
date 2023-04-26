// ************ @autor: Álvaro Repiso Romero *************
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class LoadGameMenuUI : MonoBehaviour
    {
        public static LoadGameMenuUI Instance { get; private set; }


        [Header("Load Game Buttons")]
        [SerializeField] private Button _firstSlotButton;
        [SerializeField] private Button _secondSlotButton;
        [SerializeField] private Button _thirdSlotButton;
        [SerializeField] private Button _returnButton;

        [Header("Save Slots Panel")]
        [SerializeField] private GameObject _firstSaveSlotPanel;
        [SerializeField] private GameObject _secondSaveSlotPanel;
        [SerializeField] private GameObject _thirdSaveSlotPanel;


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
            if ( gameObject.activeSelf )
                Hide();
        }

        private void SetButtonEvents()
        {
            _firstSlotButton.onClick.AddListener( () => OpenSaveSlotPanel() );
            _secondSlotButton.onClick.AddListener( () => OpenSaveSlotPanel() );
            _thirdSlotButton.onClick.AddListener( () => OpenSaveSlotPanel() );

            _returnButton.onClick.AddListener( () => Hide() );
        }

        private void OpenSaveSlotPanel()
        {
            _firstSaveSlotPanel.SetActive( true );
        }


        public void Show( Action OnReturnButtonClicked )
        {
            gameObject.SetActive( true );
            _firstSlotButton.Select();
            this.OnReturnButtonClicked = OnReturnButtonClicked;
        }

        public void Hide()
        {
            gameObject.SetActive( false );
            OnReturnButtonClicked?.Invoke();
        }
    }
}