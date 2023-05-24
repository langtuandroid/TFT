// ************ @autor: Álvaro Repiso Romero *************
using System;

public class LevelEvents
{
    public event Action<BumperUI.FadeOutArgs> OnChangeZone;
    public void ChangeZone( BumperUI.FadeOutArgs bumperUIArgs)
    {
        OnChangeZone?.Invoke( bumperUIArgs );
    }
    
    
    public event Action OnZoneCompleted;
    public void ZoneCompleted()
    {
        OnZoneCompleted?.Invoke();
    }
}
