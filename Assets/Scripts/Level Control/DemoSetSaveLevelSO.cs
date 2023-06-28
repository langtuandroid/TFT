using UnityEngine;

public class DemoSetSaveLevelSO : MonoBehaviour
{
    public ZoneSaveSO[] zoneSaveSoArray;

    public void SetComplete()
    {
        foreach ( var zoneSaveSO in zoneSaveSoArray )
            zoneSaveSO.zoneSave.IsCompleted = true;
    }
}
