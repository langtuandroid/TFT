using UnityEngine;

//[CreateAssetMenu(fileName = "PlayerStatusSaveSO", menuName = "SaveData/PlayerStatusSaveSO")]
public class PlayerStatusSaveSO : ScriptableObject
{
    public PlayerStatusSave playerStatusSave;

    public void NewGameReset()
    {
        playerStatusSave.maxHealth = 3;
        playerStatusSave.currentHealth = playerStatusSave.maxHealth;
        playerStatusSave.maxSouls = 1000;
        playerStatusSave.currentSouls = playerStatusSave.maxSouls;

        playerStatusSave.isFireWeakUnlock = false;
    }
}
