using UnityEngine;
using Utils;

namespace Attack
{
    public class LightAttack : MonoBehaviour//, IAttack
    {
        private Vector2 _direction;
        private Transform _origin;


        #region Interface Methodss

        /// <summary>
        /// Lanza el hechizo de la bola de luz
        /// </summary>
        public void WeakAttack(GameObject prefab)
        {
            // Instanciamos bola de luz
            GameObject lightBall = Instantiate(
                prefab, // Prefab de la bola
                transform.position, // Posici�n del jugador
                Quaternion.identity // Quaternion identity
                );

            //Y modificamos su direcci�n
            SetBallDirection(lightBall.GetComponent<LightMovement>());

        }

        public void ChangeStrongAttackState()
        {
        }

        public void MediumAttack()
        {
        }

        public void ResetValues()
        {
        }

        public void StrongAttack(System.Object element)
        {
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
            lightScript.HandleMovement(_direction);
        }

        public void SetOriginAndDirection(Transform origin, Vector2 direction)
        {
            _origin = origin;
            _direction = direction;
        }

        #endregion

    }
}
