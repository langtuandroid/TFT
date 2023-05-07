using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;


namespace Attack
{
    public class FireAttack : MonoBehaviour, IAttack
    {
        #region Private Variables

        // Dirección de los ataques
        private Vector2 _direction;

        // Objeto del lanzallamas
        private GameObject _flame;
        private List<GameObject> _flamesToDestroy = new List<GameObject>();

        #endregion

        #region Interface Methods

        /// <summary>
        /// Lanza una bola de fuego
        /// </summary>
        public void WeakAttack(GameObject prefab)
        {
            // Instanciamos bola de fuego
            GameObject fireball = Instantiate(
                prefab, // Prefab de la bola
                transform.position, // Posición del player
                Quaternion.identity // Quaternion identity
                );

            fireball.GetComponent<Fireball>().SetDirection(_direction);

        }

        /// <summary>
        /// Activa el lanzallamas (si corresponde)
        /// </summary>
        public void MediumAttack(GameObject prefab)
        {
            _flame = Instantiate(
                prefab, // Prefab de la llama
                transform
                );

            _flame.GetComponent<ParticleSystem>().Play();
        }

        public void StopMediumAttack()
        {
            // Lo quitamos
            _flame.transform.parent = null;
            // Y lo paramos
            _flame.GetComponent<ParticleSystem>().Stop();

            _flamesToDestroy.Add(_flame);
            Invoke(nameof(DisableAndDestroy), 4f);
        }

        /// <summary>
        /// Activa una ráfaga de bolas de fuego que afecta a toda la pantalla
        /// </summary>
        public void StrongAttack(System.Object element)
        {
            // Activamos el poder
            StartCoroutine(FinalPower((List<GameObject>)element));
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        #endregion

        #region Private Methods

        #region Medium Attack

        private void DisableAndDestroy()
        {
            GameObject obj = _flamesToDestroy[0];
            Destroy(obj);
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
