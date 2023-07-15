using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagicUI : MonoBehaviour
{

    #region SerializeFields

    [Header("Scriptable Objects")]
    [SerializeField]
    [Tooltip("Scriptable Object con la info del player")]
    private PlayerStatusSaveSO _playerStatusSaveSO;

    [Header("Canvas")]
    [SerializeField]
    [Tooltip("Texto con la información de la magia")]
    private TMP_Text _magicTextInfo;

    #endregion

    #region Private methods

    // Eventos mágicos
    private MagicEvents _magicEvents;

    #endregion


    #region Unity methods

    private void Start()
    {
        _magicEvents = ServiceLocator.GetService<MagicEvents>();
        _magicEvents.OnUseOfMagicValue += OnUseOfMagicValue;
        ChangeText();
    }

    private void Update()
    {
        ChangeText();
    }

    private void OnDestroy()
    {
        _magicEvents.OnUseOfMagicValue -= OnUseOfMagicValue;
    }

    #endregion

    #region Private methods

    private void OnUseOfMagicValue(int value)
    {
        ChangeText();
    }

    private void ChangeText()
    {
        int l = _playerStatusSaveSO.playerStatusSave.maxHealth.ToString().Length;
        string s = "";
        switch (l)
        {
            case 1:
                s = $"{_playerStatusSaveSO.playerStatusSave.currentMagic:D1}";
                break;
            case 2:
                s = $"{_playerStatusSaveSO.playerStatusSave.currentMagic:D2}";
                break;
            case 3:
                s = $"{_playerStatusSaveSO.playerStatusSave.currentMagic:D3}";
                break;
        }

        _magicTextInfo.text = s +
            $" / {_playerStatusSaveSO.playerStatusSave.maxMagic}";
    }


    #endregion




}
