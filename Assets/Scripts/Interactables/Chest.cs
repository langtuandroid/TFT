using UnityEngine;
using Utils;

public class Chest : ActivableSceneObject, IInteractable
{
    [SerializeField] private GameObject _exclamationIcon;

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
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