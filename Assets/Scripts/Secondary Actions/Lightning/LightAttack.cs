using UnityEngine;
using UnityEngine.Rendering.Universal;
using Utils;

namespace Attack
{
    public class LightAttack : SecondaryAction
    {
        #region SerializeFields

        [SerializeField]
        [Tooltip("Prefab de la bola de luz")]
        private GameObject _lightPrefab;

        private LightMovement _lightScript;

        #endregion

        #region Inherited Methodss

        /// <summary>
        /// Lanza el hechizo de la bola de luz
        /// </summary>
        public override void Effect()
        {
            _isUsingEffect = true;

            // Instanciamos bola de luz
            GameObject lightBall = Instantiate(
                _lightPrefab, // Prefab de la bola
                transform.position, // Posición del jugador
                Quaternion.identity // Quaternion identity
                );

            _lightScript = lightBall.GetComponent<LightMovement>();
            
            Vector2 directionFixed = new Vector2(direction.x, direction.y).normalized * Constants.PLAYER_OFFSET;
            
            lightBall.transform.position = new Vector2(
                transform.position.x + directionFixed.x,
                transform.position.y + directionFixed.y
            );

            SetBallDirection(directionFixed.normalized);
            _isUsingEffect = false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Cambia la dirección de la bola de luz conforme a la dirección
        /// a la que mira el jugador
        /// </summary>
        /// <param name="lightScript"></param>
        private void SetBallDirection(Vector2 lightDirection)
        {
            _lightScript.HandleMovement(lightDirection);
        }

        #endregion

    }
}
