using UnityEngine;

public class CircleActivation : MonoBehaviour
{
    [SerializeField] private Color _completedMagicColor;
    private Color _activeMagicColor = Color.yellow;
    
    [Header("Zone Save SO")]
    [SerializeField] private ZoneSaveSO _previousZoneSaveSO;
    [SerializeField] private ZoneSaveSO _thisZoneSaveSO;

    private void Awake()
    {
        bool isColliderEnabled = false;

        if ( _thisZoneSaveSO.zoneSave.IsCompleted )
        {
            foreach ( var item in GetComponentsInChildren<SpriteRenderer>() )
                item.color = _completedMagicColor;
            isColliderEnabled = true;
        }
        else
        if ( _previousZoneSaveSO.zoneSave.IsCompleted )
        {
            GetComponent<SpriteRenderer>().color = _activeMagicColor;
            isColliderEnabled = true;
        }
        GetComponentInChildren<Collider2D>().enabled = isColliderEnabled;
    }
}
