
public class AudioID
{
    // SFX
    public const int G_PLAYER = 0;

    public const int S_PHYSIC_ATK = 0;
    public const int S_PHYSIC_ATK_HIT = 1;
    public const int S_JUMP = 2;
    public const int S_LANDING = 3;
    public const int S_PICK_UP = 4;
    public const int S_THROW = 5;


    public const int G_MAGIC = 1;

    public const int S_CHARGE = 0;
    public const int S_CHARGE_END = 1;


    public const int G_FIRE = 2;

    public const int S_FIRE_BALL = 0;
    public const int SS_FIRE_BALL_HIT = 1;
    public const int S_FLAMETHROWER = 2;
    public const int S_FIRE_DEFINITIVE = 3;
}

public enum MusicName
{
    None,
    Main_Menu,
    Woods_Zone_0,
    Woods_Zone_1,
    Woods_Zone_2,
    Woods_Zone_3,
    Woods_Zone_4,
    Woods_Altar,
    Woods_Dungeon,
    Woods_Boss
}

public enum MusicZoneParameter
{
    None, Zone_0, Zone_1, Zone_2, Zone_3, Zone_4, Altar
}