using UnityEngine;

public class CircleActivation : ActivableSceneObject
{
    [SerializeField] private Color _completedMagicColor;
    private Color _activeMagicColor = Color.yellow;
    
    [Header("Zone Save SO")]
    [SerializeField] private ZoneSaveSO _previousZoneSaveSO;

    private void Awake()
    {
        GetComponentInChildren<Collider2D>().enabled = false;

        if ( _previousZoneSaveSO.zoneSave.IsCompleted )
            GetComponent<SpriteRenderer>().color = _activeMagicColor;
    }

    public override void TriggerActivation()
    {
        base.TriggerActivation();

        GetComponentInChildren<Collider2D>().enabled = true;

        foreach ( var sprite in GetComponentsInChildren<SpriteRenderer>() )
            sprite.color = _completedMagicColor;
    }
}
