using System;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [Header("Pickable")]
    [SerializeField] private float _checkDistance = 0.6f;
    [SerializeField] private Vector2 _rayCastOffset = new( 0.2f , 0.2f );
    [SerializeField] private Transform _pickUpPoint;  

    private Transform _transform;
    private Vector2 _colliderOffset;
    private LayerMask _interactableLayer;
    
    private IAudioSpeaker _audioSpeaker;
    private IPickable _pickable;
    public bool HasItem = false;
    
    public void Init(Transform playerTransform , Vector2 colliderOffset , LayerMask interactableLayerMask)
    {
        _transform         = playerTransform;
        _colliderOffset    = colliderOffset;
        _interactableLayer = interactableLayerMask;
        _audioSpeaker = ServiceLocator.GetService<IAudioSpeaker>();
    }

    public bool CanPickItUp( Vector2 lookDirection )
    {
        float xRayOffset = lookDirection.y != 0 ? _rayCastOffset.x : 0;
        float yRayOffset = lookDirection.x != 0 ? _rayCastOffset.y : 0;

        _pickable?.ShowCanPickUpItem( false );

        Vector2 origin = new Vector2( _colliderOffset.x + _transform.position.x + xRayOffset,
            _colliderOffset.y + _transform.position.y + yRayOffset );

        RaycastHit2D hit = Physics2D.Raycast( origin , lookDirection , _checkDistance , _interactableLayer );

        _pickable = hit.collider?.GetComponent<IPickable>();
        if ( _pickable != null )
        {
            _pickable.ShowCanPickUpItem(true);
            return true;
        }


        origin = new Vector2( _colliderOffset.x + _transform.position.x - xRayOffset ,
            _colliderOffset.y + _transform.position.y - yRayOffset );

        hit = Physics2D.Raycast( origin , lookDirection , _checkDistance , _interactableLayer );

        _pickable = hit.collider?.GetComponent<IPickable>();
        if ( _pickable != null ) 
        {
            _pickable.ShowCanPickUpItem(true);
            return true;
        }
        
        return false;
    }
    
    public void PickItUp( Vector2 lookDirection )
    {
        HasItem = true;
        _pickable?.ShowCanPickUpItem( false );
        _pickable?.PickItUp( lookDirection, _pickUpPoint );
    }
    
    public void ThrowIt(Vector2 lookDirection)
    {
        HasItem = false;
        _pickable?.ThrowIt(lookDirection);
        _audioSpeaker.PlaySound( AudioID.G_PLAYER , AudioID.S_THROW );
        StopPickItUp();
    }
    
    public void StopPickItUp()
    {
        if ( _pickable == null ) return;

        _pickable?.ShowCanPickUpItem( false );
        _pickable = null;
    }

    public void ShowCanPickUpItem(bool show)
    {
        _pickable?.ShowCanPickUpItem(show);
    }
}
