using UnityEngine;

public class ActivableSceneObject : MonoBehaviour
{
    protected bool _hasBeenActivated;

    public virtual void TriggerActivation( bool hasBeenActivated )
    {
        _hasBeenActivated = hasBeenActivated;
    }

    public bool HasBeenActivated => _hasBeenActivated;
}
