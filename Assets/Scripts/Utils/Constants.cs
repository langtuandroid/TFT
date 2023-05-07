namespace Utils
{
    public class Constants
    {
        #region Tags
        #region Fire Attacks
        public const string TAG_FIRE_BALL = "FireBall";
        public const string TAG_FLAMES = "Flames";

        #endregion

        #region Enemies
        public const string TAG_WAYPOINT = "Waypoint";
        public const string TAG_PATROL_COLLIDER = "PatrolCollider";

        #endregion

        #region Player
        public const string TAG_PLAYER = "Player";
        public const string TAG_PLAYER_INITIAL_POSITION = "PlayerInitialPosition";
        public const string TAG_PLAYER_PICKUP_POSITION = "PickUp";

        #endregion

        #region References
        public const string TAG_TORCH = "Torch";

        #endregion
        #endregion

        #region Animations variables

        #region Player
        public const string ANIM_PLAYER_JUMP = "Jump";
        public const string ANIM_PLAYER_WALKING = "IsWalking";
        public const string ANIM_PLAYER_DIRX = "x";
        public const string ANIM_PLAYER_DIRY = "y";

        #endregion

        #region References
        public const string ANIM_CHEST_OPENED = "Opened";

        #endregion

        #endregion


        #region Attack variables

        #region Magic maxpower panel names

        public const string PANEL_FIRE = "FirePower";
        public const string PANEL_LEAF = "LeafPower";
        public const string PANEL_WATER = "WaterPower";

        #endregion

        #region Fire attack

        public const float ANGLE_FLAMETHROWER = 45f;
        public const float TIME_TO_FLAMETHROWER = .3f;
        public const float TIME_FIRE_STRONG_ATTACK = 2f;

        #endregion

        #endregion

        #region Health variables

        public const int MAX_QUANTITY_OF_HEARTS = 32;

        #endregion

    }
}



