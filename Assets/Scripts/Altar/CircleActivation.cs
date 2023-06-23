using UnityEngine;

public class CircleActivation : MonoBehaviour
{
    [SerializeField] private AltarColorSO _altarColorSO;
    
    [Header("Zone Save SO")]
    [SerializeField] private ZoneSaveSO _previousZoneSaveSO;
    [SerializeField] private ZoneSaveSO _thisZoneSaveSO;

    private void Awake()
    {
        bool isColliderEnabled = false;

        if ( _thisZoneSaveSO.zoneSave.IsCompleted )
        {
            foreach ( var item in GetComponentsInChildren<SpriteRenderer>() )
                item.color = _altarColorSO.completeMagicColor;
            isColliderEnabled = true;
        }
        else
        if ( _previousZoneSaveSO.zoneSave.IsCompleted )
        {
            GetComponent<SpriteRenderer>().color = _altarColorSO.activationMagicColor;
            isColliderEnabled = true;
        }
        GetComponentInChildren<Collider2D>().enabled = isColliderEnabled;
    }
}
