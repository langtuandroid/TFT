using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public static PlayerMovement Instance;

        #region SerializeFields

        [Header("Move Settings")]
        [SerializeField] private float _speed; // Velocidad de movimiento del personaje

        //[Header("Attack")]
        //private PlayerAttackExtra _attack;

        #endregion

        #region Const Variables

        public enum AnimationLayers
        {
            // Animaciones de caminar
            WalkDown,
            WalkHorizontal,
            WalkUp,
            // Animaciones de salto
            JumpDown,
            JumpHorizontal,
            JumpUp,
            // Animaciones de ataque
            // Animación nula
            Null
        }
        #endregion

        #region Public Variables

        public AnimationLayers Layer => _layer; // Da la capa de animación en la que estamos
        public bool HorizontalFlip => _spriteRend.flipX; // Devuelve si está volteado el sprite o no
        #endregion

        #region Private Variables

        // COMPONENTES DEL GAMEOBJECT
        private Rigidbody2D _rb; // RigidBody del personaje
        private SpriteRenderer _spriteRend; // SpriteRenderer del personaje

        // ANIMATOR
        private AnimationLayers _layer; // Layer en ese momento


        #endregion

        #region Unity Methods

        private void Awake()
        {
            //Hacemos Singleton a la clase
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);

            // Inicializamos variables
            _rb = GetComponent<Rigidbody2D>();
            _spriteRend = GetComponentInChildren<SpriteRenderer>();
            //_attack = GetComponent<PlayerAttackExtra>();

            // Establecemos como layer inicial el primero (Walkdown)
            _layer = AnimationLayers.WalkDown;
            // Y el layer para el salto a null
            //_jumpLayer = AnimationLayers.Null;
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Mueve el GameObject del personaje
        /// </summary>
        public void Move(Vector2 direction)
        {
            // Y movemos el RigidBody
            _rb.MovePosition(_rb.position + Time.deltaTime * _speed * direction);
        }

        /// <summary>
        /// Método que cambia la ubicación del personaje
        /// </summary>        
        public void ChangeWorldPosition(Transform _destinyTransform)
        {
            transform.position =
                new Vector3(_destinyTransform.position.x, _destinyTransform.position.y, _destinyTransform.position.z);
        }

        #endregion

    }
}
