using UnityEngine;
using Utils;

public class Chest : ActivableSceneObject, IInteractable
{
    [SerializeField] private GameObject _exclamationIcon;

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        // TODO -> Get if CAN BE OPEN because is not OPEN FROM BEFORE from ServiceLocator SaveData
        // if Cannot be opened then change chest anim to opened
    }

    public override void TriggerActivation()
    {
        base.TriggerActivation();
        _anim.SetBool( Constants.ANIM_CHEST_OPENED , true );
    }

    public void Interact( Vector2 lookDirection )
    {
        if ( !_hasBeenActivated && lookDirection.y > 0 )
        {
            _anim.SetBool( Constants.ANIM_CHEST_OPENED , true );
            ShowCanInteract( false );
            _hasBeenActivated = true;
        }
    }

    public void ShowCanInteract( bool show )
    {
        if ( !_hasBeenActivated )
            _exclamationIcon.SetActive( show );
    }
}