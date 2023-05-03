using UnityEngine;

public class Interaction : MonoBehaviour
{
    [Header("Interactable")]
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private float _checkDistance = 0.6f;
    [SerializeField] private Vector2 _rayCastOffset = new( 0.2f , 0.2f );

    private Vector2 _collider_offset;

    private void Awake() => _collider_offset = GetComponent<Collider2D>().offset;

    public void Interact( Vector2 lookDirection )
    {
        float xRayOffset = lookDirection.y != 0 ? _rayCastOffset.x : 0;
        float yRayOffset = lookDirection.x != 0 ? _rayCastOffset.y : 0;


        Vector2 origin = new Vector2( _collider_offset.x + transform.position.x + xRayOffset,
                                      _collider_offset.y + transform.position.y + yRayOffset );

        RaycastHit2D hit = Physics2D.Raycast( origin , lookDirection , _checkDistance , _interactableLayer );

        if ( hit )
        {
            hit.collider.GetComponent<IInteractable>()?.Interact( lookDirection );
            return;
        }


        origin = new Vector2( _collider_offset.x + transform.position.x - xRayOffset,
                              _collider_offset.y + transform.position.y - yRayOffset );

        hit = Physics2D.Raycast( origin , lookDirection , _checkDistance , _interactableLayer );

        if ( hit )
            hit.collider.GetComponent<IInteractable>()?.Interact( lookDirection );
    }
}
