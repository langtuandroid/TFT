using UnityEngine;

namespace Player
{
    public class PlayerMovement
    {
        private float _speed; // Velocidad de movimiento del personaje
        private float _accelerationOnAir; // Aceleración presente en el personaje en el aire
        private float _currentSpeedOnAir; // Velocidad de movimiento del personaje en el aire
        private Rigidbody2D _rb; // RigidBody del personaje

        public PlayerMovement( Rigidbody2D rigidbody2d , PlayerPhysicalDataSO physicalData )
        {
            _rb = rigidbody2d;
            _speed = physicalData.moveSpeed;
            _accelerationOnAir = physicalData.accelerationOnAir;
        }

        /// <summary>
        /// Mueve el GameObject del personaje
        /// </summary>
        public void Move(Vector2 direction)
        {
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
            _currentSpeedOnAir = 0;
            _rb.velocity = Vector2.zero;
        }
    }
}
