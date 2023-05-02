using UnityEngine;

public class Interaction : MonoBehaviour
{
    [Header("Interactable")]
    [SerializeField] private LayerMask _interactableLayer;

    public void Interact( Vector2 origin , Vector2 lookDirection )
    {
        float comprovationDistance = 1;
        origin = new Vector2( origin.x + transform.position.x , origin.y + transform.position.y );
        RaycastHit2D hit = Physics2D.Raycast( origin ,
                lookDirection ,
                comprovationDistance , _interactableLayer );

        if ( hit )
        {
            hit.collider.GetComponent<IInteractable>()?.Interact( lookDirection );
        }
    }
}
