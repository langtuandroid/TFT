using System;

[Serializable]
public class PlayerStatusSave
{
    // Basic status
    public int currentHealth;
    public int maxHealth;
    public int currentSouls;
    public int maxSouls;

    // Primary skill
    public bool isFireWeakUnlock;
    public bool isFireMediumUnlock;
    public bool isFireStrongUnlock;
    
    public bool isPlantWeakUnlock;
    public bool isPlantMediumUnlock;
    public bool isPlantStrongUnlock;
    
    public bool isWaterWeakUnlock;
    public bool isWaterMediumUnlock;
    public bool isWaterStrongUnlock;

    // Secondary skill
    public bool isLightUnlock;
}
