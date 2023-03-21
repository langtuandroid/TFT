using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace UI
{
    public class LoadGameMenu : MonoBehaviour
    {
        [Header("Load Game Buttons")]
        [SerializeField] private Button _firstSlotButton;
        [SerializeField] private Button _secondSlotButton;
        [SerializeField] private Button _thirdSlotButton;

        [Header("Save Slots Panel")]
        [SerializeField] private GameObject _firstSaveSlotPanel;
        [SerializeField] private GameObject _secondSaveSlotPanel;
        [SerializeField] private GameObject _thirdSaveSlotPanel;


        private void Awake()
        {
            _firstSlotButton.onClick.AddListener( () => OpenSaveSlotPanel() );
            _secondSlotButton.onClick.AddListener( () => OpenSaveSlotPanel() );
            _thirdSlotButton.onClick.AddListener( () => OpenSaveSlotPanel() );
        }


        private void OpenSaveSlotPanel()
        {
            _firstSaveSlotPanel.SetActive( true );
        }
    }
}