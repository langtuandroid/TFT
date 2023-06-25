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

    // Physics Skills
    public bool isPhysicAttackUnlocked; // Si tiene el cetro
    public bool isJumpUnlocked;
    public bool isSwimUnlocked;
    public bool isDashUnlocked;

    // Primary skill
    // Fire power
    public bool isFireWeakUnlocked;
    public bool isFireMediumUnlocked;
    public bool isFireStrongUnlocked;
    
    // Plant power
    public bool isPlantWeakUnlocked;
    public bool isPlantMediumUnlocked;
    public bool isPlantStrongUnlocked;
    
    // Water power
    public bool isWaterWeakUnlocked;
    public bool isWaterMediumUnlocked;
    public bool isWaterStrongUnlocked;

    // Secondary skill
    // Light power
    public bool isLightUnlocked;
    // Air power
    public bool isAirUnlocked;

    // Objects
    public bool lifeBerryUnlocked;
    public int lifeBerryQuantity;
}
