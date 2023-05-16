using UnityEngine;
using Utils;

namespace Attack
{
    public class LightAttack : SecondaryAction
    {
        #region SerializeFields

        [SerializeField]
        [Tooltip("Prefab de la bola de luz")]
        private GameObject _lightPrefab;

        #endregion

        #region Inherited Methodss

        /// <summary>
        /// Lanza el hechizo de la bola de luz
        /// </summary>
        public override void Effect()
        {
            // Instanciamos bola de luz
            GameObject lightBall = Instantiate(
                _lightPrefab, // Prefab de la bola
                transform.position, // Posici�n del jugador
                Quaternion.identity // Quaternion identity
                );

            lightBall.transform.position = new Vector2(
                _lightPrefab.transform.position.x,
                _lightPrefab.transform.position.y + Constants.PLAYER_OFFSET
                );

            //Y modificamos su direcci�n
            SetBallDirection(lightBall.GetComponent<LightMovement>());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Cambia la direcci�n de la bola de luz conforme a la direcci�n
        /// a la que mira el jugador
        /// </summary>
        /// <param name="lightScript"></param>
        private void SetBallDirection(LightMovement lightScript)
        {
            lightScript.HandleMovement(direction);
        }

        #endregion

    }
}
