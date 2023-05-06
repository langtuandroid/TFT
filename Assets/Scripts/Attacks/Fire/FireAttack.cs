using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;


namespace Attack
{
    public class FireAttack : MonoBehaviour, IAttack
    {
        #region Private Variables

        // Temporizador para ver si usar el lanzallamas
        private float _timer = 0f;
        //// Booleano que te indica si se ha pulsado el botón de poder fuerte
        //private bool _isStrongAttackActive = false;
        //// Booleano que indica si se ha activado ya el lanzallamas
        //private bool _isFlamethrowerActive = false;
        //// Lista que apunta a la lista actual de llamas
        //private GameObject[] _flames;
        //// Índice de la llama activa en ese instante
        //private int _flameIndex = 0;
        //// Corrutina del lanzallamas
        //private Coroutine _flameCoroutine;

        private Vector2 _direction;
        private Vector2 _origin;

        #endregion

        #region Interface Methods

        /// <summary>
        /// Lanza una bola de fuego
        /// </summary>
        public void WeakAttack(GameObject prefab)
        {
            // Instanciamos bola de fuego
            GameObject fireball = MonoBehaviour.Instantiate(
                prefab, // Prefab de la bola
                _origin,
                Quaternion.identity // Quaternion identity
                );

            fireball.GetComponent<Fireball>().SetDirection(_direction);

            //// Y modificamos su dirección
            ChangeFireBallDirection(fireball.GetComponent<Fireball>());
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
            //else if (!_isFlamethrowerActive)
            //{
            //    // Activamos el lanzallamas, teniendo en cuenta la dirección
            //    // de la animación de movimiento
            //    //switch (PlayerMovement.Instance.Layer)
            //    //{
            //    //    // Si nos movemos hacia abajo
            //    //    case PlayerMovement.AnimationLayers.WalkDown:
            //    //        AsignNewList(PlayerAttack.Instance.Flames[2]);
            //    //        break;
            //    //    // Si nos movemos en horizontal
            //    //    case PlayerMovement.AnimationLayers.WalkHorizontal:
            //    //        // Si se mueve a la izquierda, se activa el de la izquierda
            //    //        // en otro caso, el de la derecha
            //    //        GameObject[] list = PlayerMovement.Instance.HorizontalFlip ?
            //    //            PlayerAttack.Instance.Flames[3] :
            //    //            PlayerAttack.Instance.Flames[1];
            //    //        AsignNewList(list);
            //    //        break;
            //    //    // Si nos movemos hacia arriba
            //    //    case PlayerMovement.AnimationLayers.WalkUp:
            //    //        AsignNewList(PlayerAttack.Instance.Flames[0]);
            //    //        break;
            //    //}

            //    //// Activamos una nueva corrutina
            //    //_flameCoroutine = PlayerAttack.Instance.
            //    //    ActivateCoroutine(ActivateFlames());
            //    //// E indicamos que ha sido activado el lanzallamas
            //    //_isFlamethrowerActive = true;
            //}
        }

        /// <summary>
        /// Activa una ráfaga de bolas de fuego que afecta a toda la pantalla
        /// </summary>
        public void StrongAttack(System.Object element)
        {
            // Activamos el poder
            StartCoroutine(FinalPower((List<GameObject>)element));
        }

        public void SetOriginAndDirection(Transform origin, Vector2 direction)
        {
            _origin = origin.position;
            _direction = direction;
        }

        #endregion

        #region Private Methods

        #region Weak Attack

        /// <summary>
        /// Cambia la dirección de movimiento de una bola de fuego dada
        /// </summary>
        /// <param name="fireball"></param>
        /// <param name="script"></param>
        private void ChangeFireBallDirection(Fireball script)
        {
            script.SetDirection(_direction);
        }

        #endregion

        #region Medium Attack

        /// <summary>
        /// Asigna una nueva lista a la lista de flames.
        /// En caso de ser la misma, para corrutinas existentes
        /// </summary>
        /// <param name="list"></param>
        private void AsignNewList(GameObject[] list)
        {
            //if (_flames != list)
            //    _flames = list;
            //else if (_flameCoroutine != null)
            //    StopCoroutine(_flameCoroutine);
        }

        #endregion

        #region Strong Attack

        private void ChangeOrbsState(List<GameObject> fireOrbs)
        {
            // Para cada orbe de la lista
            foreach (GameObject obj in fireOrbs)
                // Se cambia el estado al opuesto que tiene en ese momento
                obj.SetActive(!obj.activeSelf);
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
            //for (int i = _flameIndex; i < _flames.Length; i++)
            //{
            //    _flameIndex = i;
            //    _flames[_flameIndex].SetActive(true);
            yield return new WaitForSeconds(0.02f);
            //}
        }

        /// <summary>
        /// Corrutina para ir desactivando las llamitas
        /// </summary>
        /// <returns></returns>
        private IEnumerator DeactivateFlames()
        {
            //for (int i = _flameIndex; i >= 0; i--)
            //{
            //    _flameIndex = i;
            //    _flames[_flameIndex].SetActive(false);

            yield return new WaitForSeconds(0.02f);
            //}
        }

        #endregion

        #region Strong Attack

        /// <summary>
        /// Aplica el poder final
        /// </summary>
        /// <returns></returns>
        private IEnumerator FinalPower(List<GameObject> fireOrbs)
        {
            // TODO: Detener el juego

            // Configuramos el panel
            StartCoroutine(ChangePanel());
            // Rotamos los orbes
            yield return RotateOrbs(fireOrbs);

            // TODO: Dejar de detener el juego
        }

        /// <summary>
        /// Cambia el alfa del poder máximo
        /// </summary>
        /// <returns></returns>
        private IEnumerator ChangePanel()
        {
            PowerPanelsManager.Instance.ChangePanelColor(this);
            float alpha = 25 / 255f;
            // Cambiamos el alfa de la imagen del panel a 25
            PowerPanelsManager.Instance.SetAlpha(alpha);
            // Indicamos que debe crecer
            bool grow = true;

            // Cada cierto tiempo hasta llegar al tiempo de duración del ataque final
            for (float t = 0f; t < Constants.TIME_FIRE_STRONG_ATTACK; t += 0.002f)
            {
                // Según deba crecer o no, aumentamos o decrementamos su valor
                alpha += grow ?
                    0.1f / 255f : -0.1f / 255f;

                // Si el alfa llega a un tope por arriba
                if (alpha >= 100f / 255f)
                    // Indicamos que decrezca
                    grow = false;
                // En caso contrario, si llega a un tope por debajo
                else if (alpha <= 25f / 255f)
                    // Indicamos que crezca
                    grow = true;

                // Cambiamos el alfa de la imagen del panel
                PowerPanelsManager.Instance.SetAlpha(alpha);

                // Y esperamos un tiempo
                yield return new WaitForSecondsRealtime(0.002f);
            }

            PowerPanelsManager.Instance.SetAlpha(0f);
        }

        /// <summary>
        /// Aplica rotación circular a los orbes del poder final
        /// </summary>
        /// <returns></returns>
        private IEnumerator RotateOrbs(List<GameObject> fireOrbs)
        {
            // Cambiamos el estado de los orbes (para activarlos)
            ChangeOrbsState(fireOrbs);

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

                // Para cada objeto de la lista de orbes
                for (int j = 0; j < fireOrbs.Count; j++)
                {
                    GameObject obj = fireOrbs[j];
                    int n = j + 1;

                    // Aplicamos la transformación de su posición local
                    switch (n % 8)
                    {
                        case 0:
                            obj.transform.localPosition = new Vector2
                                (posInCircle[0], -posInCircle[1]);
                            break;
                        case 1:
                            obj.transform.localPosition = posInCircle;
                            break;
                        case 2:
                            obj.transform.localPosition = new Vector2
                                (-posInCircle[0], posInCircle[1]);
                            break;
                        case 3:
                            obj.transform.localPosition = -posInCircle;
                            break;
                        case 4:
                            obj.transform.localPosition = new Vector2
                                (posInCircle[1], posInCircle[0]);
                            break;
                        case 5:
                            obj.transform.localPosition = new Vector2
                                (-posInCircle[1], posInCircle[0]);
                            break;
                        case 6:
                            obj.transform.localPosition = new Vector2
                                (-posInCircle[1], -posInCircle[0]);
                            break;
                        case 7:
                            obj.transform.localPosition = new Vector2
                                (posInCircle[1], -posInCircle[0]);
                            break;
                    }

                    // Y multiplicamos según su distancia
                    obj.transform.localPosition *= n;
                }

                // Esperamos el tiempo estipulado
                yield return new WaitForSecondsRealtime(timeToWait);
            }

            // Cambiamos el estado de los orbes (para desactivarlos)
            ChangeOrbsState(fireOrbs);
        }

        #endregion

        #endregion

    }
}
