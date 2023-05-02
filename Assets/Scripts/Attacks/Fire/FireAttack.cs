using System.Collections;
using UnityEngine;
using Utils;


namespace Attack
{
    public class FireAttack : MonoBehaviour, IAttack
    {
        #region Private Variables

        // Temporizador para ver si usar el lanzallamas
        private float _timer = 0f;
        // Booleano que te indica si se ha pulsado el botón de poder fuerte
        private bool _isStrongAttackActive = false;
        // Booleano que indica si se ha activado ya el lanzallamas
        private bool _isFlamethrowerActive = false;
        // Lista que apunta a la lista actual de llamas
        private GameObject[] _flames;
        // Índice de la llama activa en ese instante
        private int _flameIndex = 0;
        // Corrutina del lanzallamas
        private Coroutine _flameCoroutine;

        #endregion

        #region Interface Methods

        public void Execute(bool pressed)
        {
            // Si hemos pulsado el botón de disparo
            if (pressed)
            {
                // Si hemos pulsado para usar el ataque fuerte
                if (_isStrongAttackActive)
                    // Lo utilizamos
                    StrongAttack();
                // Si no
                else
                    // Usamos el medio
                    MediumAttack();

            }
            // Si no, en caso de soltarlo
            else
            {
                // Si no se ha llegado a pulsar el botón lo suficiente
                // como para activar el ataque medio
                if (_timer < Constants.TIME_TO_FLAMETHROWER)
                    // Usamos el ataque débil
                    WeakAttack();

                // Finalmente, reseteamos valores
                ResetValues();
            }
        }

        /// <summary>
        /// Lanza una bola de fuego
        /// </summary>
        public void WeakAttack()
        {
            // Instanciamos bola de fuego
            //GameObject fireball = MonoBehaviour.Instantiate(
            //    PlayerAttack.Instance.FireBall, // Prefab de la bola
            //    PlayerAttack.Instance.
            //    Transform.position, // Posición del jugador
            //    Quaternion.identity // Quaternion identity
            //    );

            ////// Y modificamos su dirección
            //ChangeFireBallDirection(fireball.GetComponent<Fireball>());
        }

        /// <summary>
        /// Activa el lanzallamas (si corresponde)
        /// </summary>
        public void MediumAttack()
        {
            // Si el temporizador no ha alcanzado el tiempo suficiente como
            // para activar el lanzallamas
            if (_timer < Constants.TIME_TO_FLAMETHROWER)
                // Incrementamos el contador de tiempo
                _timer += Time.deltaTime;
            // En caso contrario,
            // si el lanzallamas no ha sido activado aún
            else if (!_isFlamethrowerActive)
            {
                // Activamos el lanzallamas, teniendo en cuenta la dirección
                // de la animación de movimiento
                //switch (PlayerMovement.Instance.Layer)
                //{
                //    // Si nos movemos hacia abajo
                //    case PlayerMovement.AnimationLayers.WalkDown:
                //        AsignNewList(PlayerAttack.Instance.Flames[2]);
                //        break;
                //    // Si nos movemos en horizontal
                //    case PlayerMovement.AnimationLayers.WalkHorizontal:
                //        // Si se mueve a la izquierda, se activa el de la izquierda
                //        // en otro caso, el de la derecha
                //        GameObject[] list = PlayerMovement.Instance.HorizontalFlip ?
                //            PlayerAttack.Instance.Flames[3] :
                //            PlayerAttack.Instance.Flames[1];
                //        AsignNewList(list);
                //        break;
                //    // Si nos movemos hacia arriba
                //    case PlayerMovement.AnimationLayers.WalkUp:
                //        AsignNewList(PlayerAttack.Instance.Flames[0]);
                //        break;
                //}

                //// Activamos una nueva corrutina
                //_flameCoroutine = PlayerAttack.Instance.
                //    ActivateCoroutine(ActivateFlames());
                //// E indicamos que ha sido activado el lanzallamas
                //_isFlamethrowerActive = true;
            }
        }

        /// <summary>
        /// Activa una ráfaga de bolas de fuego que afecta a toda la pantalla
        /// </summary>
        public void StrongAttack()
        {
            // Activamos la corrutina del poder definitivo de fuego
            StartCoroutine(FinalPower());
            //// Cambiamos el estado de ataque fuerte
            //PlayerAttack.Instance.ChangeStrongAttackState();
            //// Y tras ello, ponemos a 0 la parte de poder máximo en el HUD
            //PlayerAttack.Instance.ChangePowerValue(0f, this);
        }

        public void ChangeStrongAttackState()
        {
            // Cambiamos el estado del booleano
            _isStrongAttackActive = !_isStrongAttackActive;
        }

        public void ResetValues()
        {
            // Reiniciamos variables
            _timer = 0f;

            // Si está activado el lanzallamas
            if (_isFlamethrowerActive)
            {
                // Si tenemos una corrutina activa
                if (_flameCoroutine != null)
                    // La desactivamos
                    StopCoroutine(_flameCoroutine);

                // Y activamos la corrutina de desactivar las llamas
                _flameCoroutine = StartCoroutine(DeactivateFlames());

                // Finalmente, indicamos que está inactivo
                _isFlamethrowerActive = false;
            }
        }

        #endregion

        #region Private Methods

        #region Weak Attack

        /// <summary>
        /// Cambia la dirección de movimiento de una bola de fuego dada
        /// </summary>
        /// <param name="fireball"></param>
        /// <param name="script"></param>
        //private void ChangeFireBallDirection(Fireball script)
        //{
        //    // Realizamos el cambio de dirección según la dirección de movimiento
        //    switch (PlayerMovement.Instance.Layer)
        //    {
        //        case PlayerMovement.AnimationLayers.WalkDown:
        //            // Establecemos una posición un poco más baja
        //            script.transform.SetY(PlayerAttack.Instance.
        //                Transform.position.y - 1);
        //            // Y establecemos movimiento hacia abajo
        //            script.SetDirection(Vector3.down);
        //            break;
        //        // Si miramos en horizontal
        //        case PlayerMovement.AnimationLayers.WalkHorizontal:
        //            // Si está mirando a la izquierda
        //            if (PlayerMovement.Instance.HorizontalFlip)
        //            {
        //                // Establecemos una posición algo ms a la izquierda
        //                script.transform.SetX(PlayerAttack.Instance.
        //                    Transform.position.x - 1);
        //                // Y lo dirigimos hacia la izquierda
        //                script.SetDirection(Vector3.left);
        //            }
        //            // En otro caso
        //            else
        //            {
        //                // Establecemos una posición algo más a la derecha
        //                script.transform.SetX(PlayerAttack.Instance.
        //                    Transform.position.x + 1);
        //                // Y lo dirigimos hacia la derecha
        //                script.SetDirection(Vector3.right);
        //            }
        //            break;
        //        // Si miramos hacia arriba
        //        case PlayerMovement.AnimationLayers.WalkUp:
        //            script.transform.SetY(PlayerAttack.Instance.
        //                Transform.position.y + 1);
        //            // Y lo dirigimos hacia arriba
        //            script.SetDirection(Vector3.up);
        //            break;
        //    }
        //}

        #endregion

        #region Medium Attack

        /// <summary>
        /// Asigna una nueva lista a la lista de flames.
        /// En caso de ser la misma, para corrutinas existentes
        /// </summary>
        /// <param name="list"></param>
        private void AsignNewList(GameObject[] list)
        {
            if (_flames != list)
                _flames = list;
            else if (_flameCoroutine != null)
                StopCoroutine(_flameCoroutine);
        }

        #endregion

        #region Strong Attack

        private void ChangeOrbsState()
        {
            //// Para cada orbe de la lista
            //foreach (GameObject obj in PlayerAttack.Instance.FireOrbs)
            //    // Se cambia el estado al opuesto que tiene en ese momento
            //    obj.SetActive(!obj.activeSelf);
        }

        #endregion

        #endregion

        #region Coroutines

        #region Medium Attack

        /// <summary>
        /// Corrutina para ir activando las llamitas
        /// </summary>
        /// <returns></returns>
        private IEnumerator ActivateFlames()
        {
            for (int i = _flameIndex; i < _flames.Length; i++)
            {
                _flameIndex = i;
                _flames[_flameIndex].SetActive(true);
                yield return new WaitForSeconds(0.02f);
            }
        }

        /// <summary>
        /// Corrutina para ir desactivando las llamitas
        /// </summary>
        /// <returns></returns>
        private IEnumerator DeactivateFlames()
        {
            for (int i = _flameIndex; i >= 0; i--)
            {
                _flameIndex = i;
                _flames[_flameIndex].SetActive(false);

                yield return new WaitForSeconds(0.02f);
            }
        }

        #endregion

        #region Strong Attack

        /// <summary>
        /// Aplica el poder final
        /// </summary>
        /// <returns></returns>
        private IEnumerator FinalPower()
        {
            // "Paramos" el juego
            //MyGameManager.Instance.ChangeStopedValue(true);

            // Configuramos el panel
            StartCoroutine(ChangePanel());
            // Rotamos los orbes
            yield return RotateOrbs();

            //// Devolvemos el juego a su curso
            //MyGameManager.Instance.ChangeStopedValue(false);
            //// Desactivamos el panel
            //PlayerAttack.Instance.PowerPanel.SetActive(false);
            //// Finalmente, actualizamos en la UI
            //PlayerAttack.Instance.FirePowerActivated();
        }

        /// <summary>
        /// Aplica rotación circular a los orbes del poder final
        /// </summary>
        /// <returns></returns>
        private IEnumerator RotateOrbs()
        {
            // Cambiamos el estado de los orbes (para activarlos)
            ChangeOrbsState();

            // Tiempo a esperar
            float timeToWait = 0.002f;
            // Grados
            float degrees = 0f;
            // Posición
            Vector2 posInCircle = Vector2.zero;

            // Se aplica durante 5 segs
            for (float i = 0f; i < Constants.TIME_FIRE_STRONG_ATTACK; i += timeToWait)
            {
                // Velocidad de rotación
                float speedRot = 180f + i * 200f;

                // Grados y radianes
                degrees += timeToWait * speedRot;
                float radians = degrees * Mathf.Deg2Rad;

                // Cálculo de la posición en el círculo
                posInCircle.x = Mathf.Cos(radians);
                posInCircle.y = Mathf.Sin(radians);

                //// Para cada objeto de la lista de orbes
                //for (int j = 0; j < PlayerAttack.Instance.FireOrbs.Length; j++)
                //{
                //    GameObject obj = PlayerAttack.Instance.FireOrbs[j];
                //    int n = j + 1;

                //    // Aplicamos la transformación de su posición local
                //    switch (n % 8)
                //    {
                //        case 0:
                //            obj.transform.localPosition = new Vector2
                //                (posInCircle[0], -posInCircle[1]);
                //            break;
                //        case 1:
                //            obj.transform.localPosition = posInCircle;
                //            break;
                //        case 2:
                //            obj.transform.localPosition = new Vector2
                //                (-posInCircle[0], posInCircle[1]);
                //            break;
                //        case 3:
                //            obj.transform.localPosition = -posInCircle;
                //            break;
                //        case 4:
                //            obj.transform.localPosition = new Vector2
                //                (posInCircle[1], posInCircle[0]);
                //            break;
                //        case 5:
                //            obj.transform.localPosition = new Vector2
                //                (-posInCircle[1], posInCircle[0]);
                //            break;
                //        case 6:
                //            obj.transform.localPosition = new Vector2
                //                (-posInCircle[1], -posInCircle[0]);
                //            break;
                //        case 7:
                //            obj.transform.localPosition = new Vector2
                //                (posInCircle[1], -posInCircle[0]);
                //            break;
                //    }

                //    // Y multiplicamos según su distancia
                //    obj.transform.localPosition *= n;
                //}

                // Esperamos el tiempo estipulado
                yield return new WaitForSecondsRealtime(timeToWait);
            }

            // Cambiamos el estado de los orbes (para desactivarlos)
            ChangeOrbsState();
        }

        /// <summary>
        /// Modifica el panel de ataque final
        /// </summary>
        /// <returns></returns>
        private IEnumerator ChangePanel()
        {
            //// Activamos el panel 
            //PlayerAttack.Instance.PowerPanel.SetActive(true);
            //// Cambiamos el alfa de la imagen del panel a 25
            //PlayerAttack.Instance.PanelImage.SetImageAlpha(25f / 255f);
            // Indicamos que debe crecer
            bool grow = true;

            // Cada cierto tiempo hasta llegar al tiempo de duración del ataque final
            for (float t = 0f; t < Constants.TIME_FIRE_STRONG_ATTACK; t += 0.002f)
            {
                // Cogemos el alfa de la imagen
                //float alpha = PlayerAttack.Instance.
                //    PanelImage.color.a;
                // Según deba crecer o no, aumentamos o decrementamos su valor
                //alpha += grow ?
                //    0.1f / 255f : -0.1f / 255f;

                //// Si el alfa llega a un tope por arriba
                //if (alpha >= 100f / 255f)
                //    // Indicamos que decrezca
                //    grow = false;
                //// En caso contrario, si llega a un tope por debajo
                //else if (alpha <= 25f / 255f)
                //    // Indicamos que crezca
                //    grow = true;

                // Cambiamos el alfa de la imagen del panel
                //PlayerAttack.Instance.PanelImage.
                //    SetImageAlpha(alpha);

                // Y esperamos un tiempo
                yield return new WaitForSecondsRealtime(0.002f);
            }
        }

        #endregion

        #endregion

    }
}
