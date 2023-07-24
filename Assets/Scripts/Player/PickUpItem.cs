using System;
using Player;
using UnityEngine;

public class PickUpItem
{
    [Header("Pickable")]
    private float _checkDistance = 0.6f;
    private Vector2 _rayCastOffset = new( 0.2f , 0.2f );
    private Transform _pickUpPoint;  

    private Transform _transform;
    private Vector2 _colliderOffset;
    private LayerMask _interactableLayer;
    private AnimatorBrain _animatorBrain;
    
    private IAudioSpeaker _audioSpeaker;
    private IPickable _pickable;
    public bool HasItem = false;

    public void Init(Transform playerTransform , Transform pickUpPoint, Vector2 colliderOffset , LayerMask interactableLayerMask, AnimatorBrain animatorBrain)
    {
        _transform = playerTransform;
        _pickUpPoint = pickUpPoint;
        _colliderOffset = colliderOffset;
        _interactableLayer = interactableLayerMask;
        _animatorBrain = animatorBrain;
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
            _pickable?.ShowCanPickUpItem( true );
            return true;
        }


        origin = new Vector2( _colliderOffset.x + _transform.position.x - xRayOffset ,
            _colliderOffset.y + _transform.position.y - yRayOffset );

        hit = Physics2D.Raycast( origin , lookDirection , _checkDistance , _interactableLayer );

        _pickable = hit.collider?.GetComponent<IPickable>();
        if ( _pickable != null ) 
        {
            _pickable?.ShowCanPickUpItem( true );
            return true;
        }
        
        return false;
    }
    
    public void PickItUp( Vector2 lookDirection )
    {
        HasItem = true;
        _animatorBrain.HasItem(true);
        _pickable?.ShowCanPickUpItem( false );
        _pickable?.PickItUp( lookDirection, _pickUpPoint );
        _animatorBrain.PickUpItem();
    }
    
    public void ThrowIt(Vector2 lookDirection)
    {
        _animatorBrain.HasItem(false);
        _pickable?.ThrowIt(lookDirection);
        _audioSpeaker.PlaySound( AudioID.G_PLAYER , AudioID.S_THROW );
        _pickable?.ShowCanPickUpItem( false );
        HasItem = false;
        _animatorBrain.SetThrow(); 
        _pickable = null;
    }
    
    public void ShowCanPickUpItem(bool show)
    {
        _pickable?.ShowCanPickUpItem(show);
    }

    public void EnemyRockThrow()
    {
        _animatorBrain.HasItem(false);
        _pickable?.ShowCanPickUpItem( false );
        HasItem = false;
        _pickable = null;
    }
}
