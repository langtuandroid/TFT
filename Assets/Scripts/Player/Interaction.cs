using UnityEngine;

public class Interaction : MonoBehaviour
{
    [Header("Interactable")]
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private float _checkDistance = 0.6f;
    [SerializeField] private Vector2 _rayCastOffset = new( 0.2f , 0.2f );

    private Vector2 _colliderOffset;
    private IInteractable _interactable;

    private void Awake() => _colliderOffset = GetComponent<Collider2D>().offset;

    public bool CanInteract( Vector2 lookDirection )
    {
        float xRayOffset = lookDirection.y != 0 ? _rayCastOffset.x : 0;
        float yRayOffset = lookDirection.x != 0 ? _rayCastOffset.y : 0;


        Vector2 origin = new Vector2( _colliderOffset.x + transform.position.x + xRayOffset,
                                      _colliderOffset.y + transform.position.y + yRayOffset );

        RaycastHit2D hit = Physics2D.Raycast( origin , lookDirection , _checkDistance , _interactableLayer );

        if ( hit )
        {
            _interactable = hit.collider.GetComponent<IInteractable>();
            _interactable?.ShowCanInteract( true );
            return true;
        }


        origin = new Vector2( _colliderOffset.x + transform.position.x - xRayOffset ,
                              _colliderOffset.y + transform.position.y - yRayOffset );

        hit = Physics2D.Raycast( origin , lookDirection , _checkDistance , _interactableLayer );

        if ( hit )
        {
            _interactable = hit.collider.GetComponent<IInteractable>();
            _interactable?.ShowCanInteract( true );
        }

        return hit;
    }

    public void Interact( Vector2 lookDirection )
    {
        _interactable?.Interact( lookDirection );
    }

    public void StopInteracting()
    {
        if ( _interactable == null ) return;

        _interactable?.ShowCanInteract( false );
        _interactable = null;
    }

    //public void Interact( Vector2 lookDirection )
    //{
    //    float xRayOffset = lookDirection.y != 0 ? _rayCastOffset.x : 0;
    //    float yRayOffset = lookDirection.x != 0 ? _rayCastOffset.y : 0;


    //    Vector2 origin = new Vector2( _collider_offset.x + transform.position.x + xRayOffset,
    //                                  _collider_offset.y + transform.position.y + yRayOffset );

    //    RaycastHit2D hit = Physics2D.Raycast( origin , lookDirection , _checkDistance , _interactableLayer );

    //    if ( hit )
    //    {
    //        hit.collider.GetComponent<IInteractable>()?.Interact( lookDirection );
    //        return;
    //    }


    //    origin = new Vector2( _collider_offset.x + transform.position.x - xRayOffset,
    //                          _collider_offset.y + transform.position.y - yRayOffset );

    //    hit = Physics2D.Raycast( origin , lookDirection , _checkDistance , _interactableLayer );

    //    if ( hit )
    //        hit.collider.GetComponent<IInteractable>()?.Interact( lookDirection );
    //}
}
