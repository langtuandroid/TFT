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
        //private const string WALKING = "IsWalking";

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

        public Vector2 Direction => _direction;
        public AnimationLayers Layer => _layer; // Da la capa de animación en la que estamos
                                                //public bool IsJumping => _jump.IsJumping; // Devuelve si está saltando o no
        public bool HorizontalFlip => _spriteRend.flipX; // Devuelve si está volteado el sprite o no
        #endregion

        #region Private Variables
        // SERVICIOS
        private GameInputs _gameInputs;

        // COMPONENTES DEL GAMEOBJECT
        private Rigidbody2D _rb; // RigidBody del personaje
        private Animator _anim; // Animator del personaje
        private SpriteRenderer _spriteRend; // SpriteRenderer del personaje
        private Collider2D _collider;
        private Interaction _interaction;

        // MOVIMIENTO
        private Vector2 _direction; // Direcci�n de movimiento del personaje
        private Vector2 _lookDirection;

        // ANIMATOR
        private AnimationLayers _layer; // Layer en ese momento
                                        //private AnimationLayers _jumpLayer; // Layer para el salto

        private bool _isInteracting = false;

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
            _anim = GetComponentInChildren<Animator>();
            _spriteRend = GetComponentInChildren<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            _interaction = GetComponent<Interaction>();
            //_attack = GetComponent<PlayerAttackExtra>();

            // Establecemos como layer inicial el primero (Walkdown)
            _layer = AnimationLayers.WalkDown;
            // Y el layer para el salto a null
            //_jumpLayer = AnimationLayers.Null;
        }

        private void Start()
        {
            _gameInputs = ServiceLocator.GetService<GameInputs>();
            _gameInputs.OnEastButtonPerformed += GameInputs_OnEastButtonPerformed;
        }

        private void OnDestroy()
        {
            _gameInputs.OnEastButtonPerformed -= GameInputs_OnEastButtonPerformed;
        }

        private void Update()
        {
            // Obtenemos el vector de direcci�n
            GetDirection();

            // INTERACT
            if (_isInteracting)
            {
                _interaction.Interact(_collider.offset, _lookDirection);
                _isInteracting = false;
            }

        }

        private void FixedUpdate()
        {
            // Nos movemos con el RigidBody
            Move();
        }

        #endregion


        #region Private Methods

        private void GameInputs_OnEastButtonPerformed()
        {
            _isInteracting = true;
        }


        /// <summary>
        /// Obtiene el vector de direcci�n final (normalizado para que se mueva en la misma direcci�n en todos los ejes)
        /// </summary>
        private void GetDirection()
        {
            // Obtenemos el vector de direcci�n
            _direction = _gameInputs.GetDirectionNormalized();

            if (_direction.magnitude > 0.05f)
                _lookDirection = _direction;
        }

        /// <summary>
        /// Mueve el GameObject del personaje
        /// </summary>
        private void Move()
        {
            // Y movemos el RigidBody
            _rb.MovePosition(_rb.position + Time.deltaTime * _speed * _direction);
        }

        #endregion

        #region Public Methods

        //Método que cambia la ubicación del personaje
        public void ChangeWorldPosition(Transform _destinyTransform)
        {
            transform.position =
                new Vector3(_destinyTransform.position.x, _destinyTransform.position.y, _destinyTransform.position.z);
        }

        #endregion

    }
}
