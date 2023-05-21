// ************ @autor: Álvaro Repiso Romero *************
using System;

public class LevelEvents
{
    public Action OnChangeZone;
    public void ChangeZone()
    {
        OnChangeZone?.Invoke();
    }
    
    
    public Action OnZoneCompleted;
    public void ZoneCompleted()
    {
        OnZoneCompleted?.Invoke();
    }
}
