using UnityEngine;

//[CreateAssetMenu(fileName = "PlayerStatusSaveSO", menuName = "SaveData/PlayerStatusSaveSO")]
public class PlayerStatusSaveSO : ScriptableObject
{
    public PlayerStatusSave playerStatusSave;

    public void NewGameReset(int saveSlot)
    {
        playerStatusSave = new PlayerStatusSave();

        playerStatusSave.saveSlot = saveSlot;
        playerStatusSave.maxHealth = 12;
        playerStatusSave.currentHealth = playerStatusSave.maxHealth;
        playerStatusSave.maxMagic = 200;
        playerStatusSave.currentMagic = playerStatusSave.maxMagic;
        playerStatusSave.maxSouls = 1000;
        playerStatusSave.currentSouls = 0;

        playerStatusSave.isFireWeakUnlocked = true;
    }
}
