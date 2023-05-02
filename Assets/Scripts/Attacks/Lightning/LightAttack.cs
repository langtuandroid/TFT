using UnityEngine;
using Utils;

namespace Attack
{
    public class LightAttack : MonoBehaviour, IAttack
    {
        #region Interface Methods

        public void Execute(bool pressed)
        {
            // Para el caso de la bola de luz,
            // solo tenemos el poder débil
            if (!pressed)
                WeakAttack();
        }

        /// <summary>
        /// Lanza el hechizo de la bola de luz
        /// </summary>
        public void WeakAttack()
        {
            // Instanciamos bola de luz
            GameObject lightBall = new GameObject();
            //GameObject lightBall = Instantiate(
            //    PlayerAttack.Instance.LightBall, // Prefab de la bola
            //    PlayerAttack.Instance.
            //    Transform.position, // Posición del jugador
            //    Quaternion.identity // Quaternion identity
            //    );

            // Y modificamos su dirección
            //SetBallDirection(lightBall.GetComponent<LightMovement>());

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

        public void StrongAttack()
        {
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Cambia la dirección de la bola de luz conforme a la dirección
        /// a la que mira el jugador
        /// </summary>
        /// <param name="_lightScript"></param>
        private void SetBallDirection(LightMovement _lightScript)
        {
            //switch (PlayerMovement.Instance.Layer)
            //{
            //    // Si miramos hacia abajo
            //    case PlayerMovement.AnimationLayers.WalkDown:
            //        _lightScript.transform.SetY(PlayerAttack.Instance.
            //            Transform.position.y - 2);
            //        _lightScript.HandleMovement(Vector3.down);
            //        break;
            //    // Si miramos en el eje horizontal
            //    case PlayerMovement.AnimationLayers.WalkHorizontal:
            //        // Si mira hacia la izquierda
            //        if (PlayerMovement.Instance.HorizontalFlip)
            //        {
            //            _lightScript.transform.SetX(PlayerAttack.Instance.
            //                Transform.position.x - 2);
            //            _lightScript.HandleMovement(Vector3.left);
            //        }
            //        // En caso contrario (derecha)
            //        else
            //        {
            //            _lightScript.transform.SetX(PlayerAttack.Instance.
            //                Transform.position.x + 2);
            //            _lightScript.HandleMovement(Vector3.right);
            //        }
            //        break;
            //    // Si miramos hacia arriba
            //    case PlayerMovement.AnimationLayers.WalkUp:
            //        _lightScript.transform.SetY(PlayerAttack.Instance.
            //            Transform.position.y + 2);
            //        _lightScript.HandleMovement(Vector3.up);
            //        break;
            //}
        }

        #endregion

    }
}
