using UnityEngine;

public class ActivableSceneObject : MonoBehaviour
{
    protected bool _hasBeenActivated;

    public virtual void TriggerActivation()
    {
        _hasBeenActivated = true;
    }

    public bool HasBeenActivated() => _hasBeenActivated;
}
