using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{

    [SerializeField]
    [Tooltip("Icono que aparece para indicar que puedes abrir el cofre.")]
    private GameObject _icon;

    private const string OPENED = "Opened";

    private bool _canBeOpened = true;
    private bool _isAlreadyOpened = false;
    private Animator _anim;

    public void Interact( Vector2 lookDirection )
    {
        if ( _canBeOpened && !_isAlreadyOpened )
        {
            _canBeOpened = false;
            _isAlreadyOpened = false;
            _icon.SetActive( false );
            _anim.SetBool( OPENED , true );
        }
    }

    private void Awake()
    {
        _icon.SetActive(false);
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.CompareTag( "Player" ) )
        {
            _canBeOpened = true;
            _icon.SetActive( true );
        }
    }

    private void OnTriggerExit2D( Collider2D collision )
    {
        if ( collision.CompareTag( "Player" ) )
        {
            _canBeOpened = false;
            _icon.SetActive( false );
        }
    }
}



