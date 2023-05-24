// ************ @autor: Álvaro Repiso Romero *************
using System;

public class LevelEvents
{
    public event Action<TriggerChangeZone.FadeOutArgs> OnChangeZone;
    public void ChangeZone( TriggerChangeZone.FadeOutArgs bumperUIArgs)
    {
        OnChangeZone?.Invoke( bumperUIArgs );
    }
    
    
    public event Action OnZoneCompleted;
    public void ZoneCompleted()
    {
        OnZoneCompleted?.Invoke();
    }
}
