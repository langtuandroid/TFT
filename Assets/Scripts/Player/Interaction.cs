using UnityEngine;

public class Interaction : MonoBehaviour
{
    [Header("Interactable")]
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private float _checkDistance = 1.0f;

    public void Interact( Vector2 colliderOffset , Vector2 lookDirection )
    {
        Vector2 origin = new Vector2( colliderOffset.x + transform.position.x , 
                                      colliderOffset.y + transform.position.y );

        RaycastHit2D hit = Physics2D.Raycast( origin ,
                lookDirection ,
                _checkDistance , _interactableLayer );

        if ( hit )
        {
            hit.collider.GetComponent<IInteractable>()?.Interact( lookDirection );
        }
    }
}
