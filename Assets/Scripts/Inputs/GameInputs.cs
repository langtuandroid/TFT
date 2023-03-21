using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputs : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        MenuMode();
    }

    private void MenuMode()
    {
        // ToDo: Disable all other action maps
        _playerInputActions.UI.Enable();
        _playerInputActions.UI.Cancel.performed += Cancel_Performed;
    }

    private void Cancel_Performed( InputAction.CallbackContext obj )
    {
        if ( UI.LoadGameMenuUI.Instance.gameObject.activeSelf )
        {
            UI.LoadGameMenuUI.Instance.Hide();
        }
        if ( UI.OptionMenuUI.Instance.gameObject.activeSelf )
        {
            UI.OptionMenuUI.Instance.Hide();
        }
    }
}
