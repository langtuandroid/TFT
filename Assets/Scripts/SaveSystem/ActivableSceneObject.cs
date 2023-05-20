using UnityEngine;

public class ActivableSceneObject : MonoBehaviour
{
    protected bool _hasBeenActivated;

    public virtual void TriggerActivation()
    {
        _hasBeenActivated = true;
        Debug.Log( gameObject.name + " has been activated" );
    }

    public bool HasBeenActivated() => _hasBeenActivated;
}
