using UnityEngine;
using Utils;

public class Chest : ActivableSceneObject, IInteractable
{
    [SerializeField] private GameObject _exclamationIcon;

    private bool _canBeOpened = true;
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        // TODO -> Get if CAN BE OPEN because is not OPEN FROM BEFORE from ServiceLocator SaveData
        // if Cannot be opened then change chest anim to opened
    }

    public void Interact( Vector2 lookDirection )
    {
        if ( _canBeOpened && lookDirection.y > 0 )
        {
            _anim.SetBool( Constants.ANIM_CHEST_OPENED , true );
            _canBeOpened = false;
        }
    }

    public void ShowCanInteract( bool show )
    {
        _exclamationIcon.SetActive( show );
    }
}