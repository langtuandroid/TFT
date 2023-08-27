// ************ @autor: �lvaro Repiso Romero *************
using UnityEngine;

public class Interaction
{
    private Transform _transform;
    private LayerMask _interactableLayer;
    private IInteractable _interactable;

    private float   _checkDistance = 0.6f;
    private Vector2 _rayCastOffset = new( 0.2f , 0.2f );
    private Vector2 _colliderOffset;

    public bool IsInteracting { get; private set; }

    public Interaction( Transform playerTransform , Vector2 colliderOffset , PlayerPhysicalDataSO physicalData )
    {
        _transform         = playerTransform;
        _colliderOffset    = colliderOffset;
        _interactableLayer = physicalData.interactableLayerMask;
    }

    public void Interact( bool interactInput , Vector2 lookDirection )
    {
        StopInteracting();
        if ( CanInteract( lookDirection ) )
        {
            if ( interactInput )
            {
                _interactable?.Interact( lookDirection );
                IsInteracting = true;
            }            
        }
    }

    private bool CanInteract( Vector2 lookDirection )
    {
        float xRayOffset = lookDirection.y != 0 ? _rayCastOffset.x : 0;
        float yRayOffset = lookDirection.x != 0 ? _rayCastOffset.y : 0;


        Vector2 origin = new Vector2( _colliderOffset.x + _transform.position.x + xRayOffset,
                                      _colliderOffset.y + _transform.position.y + yRayOffset );

        RaycastHit2D hit = Physics2D.Raycast( origin , lookDirection , _checkDistance , _interactableLayer );

        _interactable = hit.collider?.GetComponent<IInteractable>();
        if ( _interactable != null )
        {
            _interactable?.ShowCanInteract( true );
            return true;
        }


        origin = new Vector2( _colliderOffset.x + _transform.position.x - xRayOffset ,
                              _colliderOffset.y + _transform.position.y - yRayOffset );

        hit = Physics2D.Raycast( origin , lookDirection , _checkDistance , _interactableLayer );

        _interactable = hit.collider?.GetComponent<IInteractable>();
        if ( _interactable != null )
        {
            _interactable?.ShowCanInteract( true );
            return true;
        }
        return false;
    }    

    private void StopInteracting()
    {
        if ( _interactable == null ) return;

        _interactable?.ShowCanInteract( false );
        _interactable = null;
        IsInteracting = false; // TODO: IsInteracting has to change false when the interactiong has ended through event
    }
}
