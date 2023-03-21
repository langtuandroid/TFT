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


        private Action _onReturn;


        private void Awake()
        {
            Instance = this;
            SetButtonEvents();
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


        public void Show( Action onReturn )
        {
            gameObject.SetActive( true );
            _firstSlotButton.Select();
            _onReturn = onReturn;
        }

        public void Hide()
        {
            gameObject.SetActive( false );
            _onReturn?.Invoke();
        }
    }
}