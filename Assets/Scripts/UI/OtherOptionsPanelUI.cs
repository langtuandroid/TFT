using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OtherOptionsPanelUI : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _lenguageDropdown;
    [SerializeField] private Toggle       _vibrationToggle;
    [SerializeField] private TMP_Dropdown _framerateDropdown;

    private OptionsSave _optionsSave;

    private void Start()
    {
        _optionsSave = ServiceLocator.GetService<OptionsSave>();
        LoadOptionsSaveData();
        SetOptionsEvents();
    }

    private void LoadOptionsSaveData()
    {
        _lenguageDropdown.value  = _optionsSave.lenguageDropdownValue;
        _vibrationToggle.isOn    = _optionsSave.isVibrationActive;
        _framerateDropdown.value = _optionsSave.framerateDropdownValue;
    }

    private void SetOptionsEvents()
    {
        _lenguageDropdown.onValueChanged.AddListener( ( int lenguageIndex ) => {
            _optionsSave.lenguageDropdownValue = lenguageIndex;
        } );

        _vibrationToggle.onValueChanged.AddListener( ( bool isOn ) => {
            _optionsSave.isVibrationActive = isOn;
        } );
        
        _framerateDropdown.onValueChanged.AddListener( ( int framerateIndex ) => {
            _optionsSave.framerateDropdownValue = framerateIndex;
        } );
    }
}
