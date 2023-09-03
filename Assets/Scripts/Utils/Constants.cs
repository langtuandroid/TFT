namespace Utils
{
    public class Constants
    {
        #region Tags
        #region Fire Attacks
        public const string TAG_MAGIC_POWER = "MagicPower";

        #endregion

        #region Enemies
        public const string TAG_ENEMY = "Enemy";
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

        #region Layers

        public const string LAYER_PLAYER = "Player";
        public const string LAYER_INTERACTABLE = "Interactable";

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

        #region Offset

        public const float PLAYER_OFFSET = 0.8125f;

        #endregion

        #region Attack variables


        #region Magic maxpower panel names

        public const string PANEL_FIRE = "FirePower";
        public const string PANEL_LEAF = "LeafPower";
        public const string PANEL_WATER = "WaterPower";

        #endregion

        #endregion

        #region Health variables

        public const int MAX_QUANTITY_OF_HEARTS = 32;

        #endregion

    }
}



