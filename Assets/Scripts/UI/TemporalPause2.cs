using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TemporalPause2 : MonoBehaviour
{
    [SerializeField]
    private List<Button> _buttons;
    // 0 -> Continuar
    // 1 -> Salir


    private Button _currentSelected;

    private void Awake()
    {
        _buttons[0].onClick.AddListener(() => gameObject.SetActive(false)); ;
        _buttons[1].onClick.AddListener(() => {
            ServiceLocator.GetService<IAudioSpeaker>().ChangeMusic( MusicName.Main_Menu );
            ServiceLocator.GetService<SceneLoader>().Load( SceneName.S00_MainMenuScene.ToString()); 
        } );
    }


    private void Update()
    {

        if (EventSystem.current.currentSelectedGameObject == null)
            _currentSelected.Select();
    }

    private void OnEnable()
    {
        _currentSelected = _buttons[0];
        _currentSelected.Select();
    }
}
