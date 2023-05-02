using UnityEngine;
using Utils;

public class Chest : MonoBehaviour, IInteractable
{
    private bool _canBeOpened = true;
    private bool _isAlreadyOpened = false;
    private Animator _anim;

    public void Interact( Vector2 lookDirection )
    {
        if ( _canBeOpened && !_isAlreadyOpened )
        {
            _canBeOpened = false;
            _isAlreadyOpened = true;
            _anim.SetBool( Constants.ANIM_CHEST_OPENED , true );
        }
    }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

}



