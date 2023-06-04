using System;

[Serializable]
public class PlayerStatusSave
{
    public int saveSlot;
    // Basic status
    public int currentHealth;
    public int maxHealth;
    public int currentMagic;
    public int maxMagic;
    public int currentSouls;
    public int maxSouls;

    // Currently equiped skills
    public int primarySkillIndex;
    public int secondarySkillIndex;

    // Physics Skills
    public bool isJumpUnlocked;
    public bool isSwimUnlocked;
    public bool isDashUnlocked;

    // Primary skill
    public bool isFireWeakUnlocked;
    public bool isFireMediumUnlocked;
    public bool isFireStrongUnlocked;
    
    public bool isPlantWeakUnlocked;
    public bool isPlantMediumUnlocked;
    public bool isPlantStrongUnlocked;
    
    public bool isWaterWeakUnlocked;
    public bool isWaterMediumUnlocked;
    public bool isWaterStrongUnlocked;

    // Secondary skill
    public bool isLightUnlocked;
}
