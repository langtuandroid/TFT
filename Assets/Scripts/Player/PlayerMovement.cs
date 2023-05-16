using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public static PlayerMovement Instance;

        [Header("Move Settings")]
        [SerializeField] private float _speed = 3; // Velocidad de movimiento del personaje
        [SerializeField] private float _accelerationOnAir = 8; // Aceleración presente en el personaje en el aire

        private float _currentSpeedOnAir; // Velocidad de movimiento del personaje en el aire

        public enum AnimationLayers
        {
            WalkDown,
            WalkHorizontal,
            WalkUp,
            JumpDown,
            JumpHorizontal,
            JumpUp,
            Null
        }

        public AnimationLayers Layer => _layer; // Da la capa de animación en la que estamos
        public bool HorizontalFlip => _spriteRend.flipX; // Devuelve si está volteado el sprite o no

        // COMPONENTES DEL GAMEOBJECT
        private Rigidbody2D _rb; // RigidBody del personaje
        private SpriteRenderer _spriteRend; // SpriteRenderer del personaje

        // ANIMATOR
        private AnimationLayers _layer; // Layer en ese momento

        private void Awake() => Instance = this;
        public void Init()
        {
            // Inicializamos variables
            _rb = GetComponent<Rigidbody2D>();
            _spriteRend = GetComponentInChildren<SpriteRenderer>();
            // Establecemos como layer inicial el primero (Walkdown)
            _layer = AnimationLayers.WalkDown;
        }

        /// <summary>
        /// Mueve el GameObject del personaje
        /// </summary>
        public void Move(Vector2 direction)
        {
            // Y movemos el RigidBody
            _rb.MovePosition(_rb.position + Time.deltaTime * _speed * direction);

            _currentSpeedOnAir = direction.magnitude > 0 ? _speed : 0;
            _rb.velocity = Vector2.zero;
        }

        public void MoveOnAir( Vector2 direction )
        {
            _currentSpeedOnAir += Time.deltaTime * _accelerationOnAir;
            Vector2 airVelocity = _currentSpeedOnAir * direction;
            _rb.velocity = Vector2.ClampMagnitude( airVelocity , _speed );
        }

        public void Stop()
        {
            _rb.velocity = Vector2.zero;
        }

        /// <summary>
        /// Método que cambia la ubicación del personaje
        /// </summary>        
        public void ChangeWorldPosition(Transform _destinyTransform)
        {
            transform.position =
                new Vector3(_destinyTransform.position.x, 
                            _destinyTransform.position.y, 
                            _destinyTransform.position.z);
        }
    }
}
