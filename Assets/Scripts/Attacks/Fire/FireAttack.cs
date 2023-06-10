using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Player;
using DG.Tweening;

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

            _audioSpeaker = ServiceLocator.GetService<IAudioSpeaker>();
            _playerStatus = GetComponent<PlayerStatus>();

            _magicEvents.OnMaxPowerUsedValue += RotateOrbs;
        }

        #endregion

        #region Abstract class methods

        public override void DeSelect()
        {
            _magicEvents.OnMaxPowerUsedValue -= RotateOrbs;
        }

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
            _audioSpeaker.PlaySound(AudioID.G_FIRE, AudioID.S_FIRE_BALL);

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
            _magicEvents.MaxPowerUsed(_playerStatus.MaxPowerDuration);
            _audioSpeaker.PlaySound(AudioID.G_FIRE, AudioID.S_FIRE_DEFINITIVE);

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

        private void ChangeOrbsState(bool state)
        {
            // Para cada orbe de la lista
            foreach (GameObject obj in _fireOrbs)
                // Se cambia el estado al opuesto que tiene en ese momento
                obj.SetActive(state);
        }

        #endregion

        #endregion

        #region DoTween

        #region Strong Attack

        /// <summary>
        /// Invoca la rotación de los orbes
        /// </summary>
        /// <param name="time"></param>
        private void RotateOrbs(float time)
        {
            Sequence seq = DOTween.Sequence();
            // Aplicamos rotación
            seq.AppendCallback(() => ApplyRotation(time));
            // Y esperamos un tiempo
            seq.AppendInterval(time);
            // Finalizamos el ataque final
            seq.OnComplete(() => _magicEvents.MaxPowerFinalized());
            // Ejecutamos la secuencia
            seq.Play();
        }

        /// <summary>
        /// Aplica la rotación
        /// </summary>
        /// <param name="time"></param>
        private void ApplyRotation(float time)
        {
            // Cambiamos el estado de los orbes (para activarlos)
            ChangeOrbsState(true);
            float angle = 0f;
            // Iniciamos un tween para dar 4 vueltas
            Tween tween = DOTween.To(() => angle, x => angle = x, 4 * 360f, time)
                .SetEase(Ease.InQuad)
                .OnUpdate(() =>
                {
                    float radians = angle * Mathf.Deg2Rad;
                    Vector2 posInCircle = new Vector2(
                        Mathf.Cos(radians),
                        Mathf.Sin(radians)
                        );

                    // Cogemos cada orbe
                    for (int i = 0; i < _fireOrbs.Count; i++)
                    {
                        GameObject obj = _fireOrbs[i];
                        int n = i + 1;
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

                }).
                // Cuando finalizamos, cambiamos el estado de los orbes
                OnComplete(() => ChangeOrbsState(false)).
                Play();

        }

        #endregion

        #endregion

    }
}
