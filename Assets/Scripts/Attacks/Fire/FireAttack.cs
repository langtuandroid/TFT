using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Player;


namespace Attack
{
    public class FireAttack : MagicAttack
    {
        #region SerializeFields

        [Header("Weak Attack")]
        [SerializeField]
        [Tooltip("Prefab de la bola de fuego")]
        private GameObject _fireballPrefab;

        [Header("Medium Attack")]
        [SerializeField]
        [Tooltip("Prefab del lanzallamas hacia arriba")]
        private GameObject _flamesUp;
        [SerializeField]
        [Tooltip("Prefab del lanzallamas hacia abajo")]
        private GameObject _flamesDown;
        [SerializeField]
        [Tooltip("Prefab del lanzallamas hacia la izda")]
        private GameObject _flamesLeft;
        [SerializeField]
        [Tooltip("Prefab del lanzallamas hacia la derecha")]
        private GameObject _flamesRight;

        [Header("Strong Attack")]
        [SerializeField]
        [Tooltip("Lista de orbes que giran alrededor del personaje al usar el poder máximo de fuego")]
        private List<GameObject> _fireOrbs;

        #endregion

        #region Private Variables

        private IAudioSpeaker _audioSpeaker;

        // Status del jugador
        private PlayerStatus _playerStatus;

        // Objeto del lanzallamas
        private GameObject _flame;
        // Lista de lanzallamas para destruirr
        private List<GameObject> _flamesToDestroy;

        // Evento
        private MagicEvents _magicEvents;

        #endregion

        #region Unity Methods

        private void OnDestroy()
        {
            foreach (GameObject f in _flamesToDestroy)
                Destroy(f);
        }

        private void Start()
        {
            // Inicializamos componentes
            _flamesToDestroy = new List<GameObject>();

            _magicEvents = ServiceLocator.GetService<MagicEvents>();
            _audioSpeaker = ServiceLocator.GetService<IAudioSpeaker>();
            _playerStatus = GetComponent<PlayerStatus>();
        }

        #endregion

        #region Interface Methods

        /// <summary>
        /// Lanza una bola de fuego
        /// </summary>
        public override void WeakAttack(Vector2 direction)
        {
            // Instanciamos bola de fuego
            GameObject fireball = Instantiate(
                _fireballPrefab, // Prefab de la bola
                transform.position, // Posición del player
                Quaternion.identity // Quaternion identity
                );

            fireball.transform.position = new Vector2(
                transform.position.x,
                transform.position.y + Constants.PLAYER_OFFSET
                );

            fireball.GetComponent<Fireball>().SetDirection(direction);
            _audioSpeaker.PlaySound( AudioID.G_FIRE , AudioID.S_FIRE_BALL );

            // Reseteamos el temporizador de uso de poder
            _playerStatus.RestartMagicTimer();
        }

        /// <summary>
        /// Activa el lanzallamas (si corresponde)
        /// </summary>
        public override void MediumAttack(Vector2 direction)
        {
            GameObject prefab = null;

            if (direction.Equals(Vector2.up))
                prefab = _flamesUp;
            else if (direction.Equals(Vector2.down))
                prefab = _flamesDown;
            else if (direction.Equals(Vector2.left))
                prefab = _flamesLeft;
            else if (direction.Equals(Vector2.right))
                prefab = _flamesRight;

            _flame = Instantiate(
                prefab, // Prefab de la llama
                transform
                );

            _flame.transform.position = new Vector2(
                _flame.transform.position.x,
                _flame.transform.position.y + Constants.PLAYER_OFFSET
                );

            //_audioSpeaker.PlaySound( AudioID.G_FIRE , AudioID.S_FLAMETHROWER );
            _flame.GetComponent<ParticleSystem>().Play();
        }

        public override void StopMediumAttack()
        {
            // Lo quitamos
            _flame.transform.parent = null;
            // Y lo paramos
            _flame.GetComponent<ParticleSystem>().Stop();

            _flamesToDestroy.Add(_flame);
            Invoke(nameof(DisableAndDestroy), 4f);

            // Reseteamos el temporizador de uso de poder
            _playerStatus.RestartMagicTimer();
        }

        /// <summary>
        /// Activa una ráfaga de bolas de fuego que afecta a toda la pantalla
        /// </summary>
        public override void StrongAttack(Vector2 direction)
        {
            // Activamos el poder
            StartCoroutine(FinalPower());
            _audioSpeaker.PlaySound( AudioID.G_FIRE , AudioID.S_FIRE_DEFINITIVE );

            // Reseteamos el temporizador de uso de poder
            _playerStatus.RestartMagicTimer();
        }

        #endregion

        #region Private Methods

        #region Medium Attack

        private void DisableAndDestroy()
        {
            GameObject obj = _flamesToDestroy[0];
            _flamesToDestroy.Remove(obj);
            Destroy(obj);
        }

        #endregion

        #region Strong Attack

        private void ChangeOrbsState()
        {
            // Para cada orbe de la lista
            foreach (GameObject obj in _fireOrbs)
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
        private IEnumerator FinalPower()
        {
            // TODO: Detener el juego

            // Quitamos el fillAmount
            _magicEvents.ChangeFillAmount(0f);
            // Configuramos el panel
            StartCoroutine(ChangePanel());
            // Rotamos los orbes
            yield return RotateOrbs();

            StartCoroutine(IncrementFillAmount());
            // TODO: Dejar de detener el juego
        }

        /// <summary>
        /// Cambia el alfa del poder máximo
        /// </summary>
        /// <returns></returns>
        private IEnumerator ChangePanel()
        {
            float alpha = 25 / 255f;
            // Cambiamos el alfa de la imagen del panel a 25
            _magicEvents.ChangePanelAlphaAmount(alpha);
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

                _magicEvents.ChangePanelAlphaAmount(alpha);

                // Y esperamos un tiempo
                yield return new WaitForSecondsRealtime(0.002f);
            }

            _magicEvents.ChangePanelAlphaAmount(0f);
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

                // Para cada objeto de la lista de orbes
                for (int j = 0; j < _fireOrbs.Count; j++)
                {
                    GameObject obj = _fireOrbs[j];
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
            ChangeOrbsState();
        }

        /// <summary>
        /// Bucle que va incrementando el fillAmount de la carga de poder máximo
        /// de un evento
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private IEnumerator IncrementFillAmount()
        {
            for (float i = 0f; i < 1f; i += 0.001f)
            {
                _magicEvents.ChangeFillAmount(i);
                yield return new WaitForSeconds(0.005f);
            }

            // Finalmente, se pone a 1 exacto
            _magicEvents.ChangeFillAmount(1f);
        }

        #endregion

        #endregion

    }
}
