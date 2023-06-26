// ************ @autor: Álvaro Repiso Romero *************
using System;

public class LevelEvents
{
    public event Action<ChangeZoneArgs> OnChangeZone;
    public class ChangeZoneArgs
    {
        public int nextStartPointRefId;
        public UnityEngine.Color fadeColor;
        public float fadeDurationSeconds;
    }
    public void ChangeZone( ChangeZoneArgs bumperUIArgs)
    {
        OnChangeZone?.Invoke( bumperUIArgs );
    }
    
    
    public event Action OnZoneCompleted;
    public void ZoneCompleted()
    {
        OnZoneCompleted?.Invoke();
    }


    public event Action OnKeyObtained;
    public void KeyObtained()
    {
        OnKeyObtained?.Invoke();
    }
}
