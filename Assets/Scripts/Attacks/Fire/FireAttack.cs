using System.Collections.Generic;
using UnityEngine;
using Utils;
using Player;
using DG.Tweening;

namespace Attack
{
    public class FireAttack : MagicAttack
    {
        #region Private Variables

        // DATA
        private FireAttackSettingsSO _fireSettingsSO => (FireAttackSettingsSO)_magicSettingsSO;

        // VARIABLES
        // Medium Attack
        private float _flameTimer;

        // Strong Attack
        private List<GameObject> _fireOrbs;
        #endregion

        #region Constructor

        public FireAttack()
        {
            // Inicializamos las variables de estado
            base.Initialize();
            _flameTimer = 0f;
            _fireOrbs = new List<GameObject>();
        }

        #endregion

        #region Abstract class methods

        public override void Init(MagicAttackSettingsSO magicSettings, PlayerStatus playerStatus, MagicEvents magicEvents, GameStatus gameStatus, IAudioSpeaker audioSpeaker, Transform transform)
        {
            base.Init(magicSettings, playerStatus, magicEvents, gameStatus, audioSpeaker, transform);

            _magicEvents.OnMaxPowerUsedValue += RotateOrbs;
            _magicEvents.OnMaxPowerFinalizedValue += MaxPowerFinalized;

            GameObject strongPrefab = MonoBehaviour.Instantiate(
                _fireSettingsSO.StrongPrefab,
                _transform
                );

            // Inicializamos el prefab de ataque fuerte
            foreach (Transform t in strongPrefab.transform)
                _fireOrbs.Add(t.gameObject);

            strongPrefab.transform.localPosition = new Vector3(0f, .8125f, 0f);

        }

        public override void Destroy()
        {
            // Desuscribimos al destruir
            _magicEvents.OnMaxPowerUsedValue -= RotateOrbs;
            _magicEvents.OnMaxPowerFinalizedValue -= MaxPowerFinalized;
        }

        public override void Run(Vector2 direction)
        {
            // Chequeamos el uso de ataque medio
            if (_isUsingMediumAttack)
            {
                _flameTimer += Time.deltaTime;
                if (_flameTimer >= _fireSettingsSO.TimeBetweenConsuming)
                {
                    MediumAttack(direction);
                }
            }
        }


        /// <summary>
        /// Lanza una bola de fuego
        /// </summary>
        public override void WeakAttack(Vector2 direction)
        {
            Debug.Log("Entro en ataque débil");
            // Activamos el uso de la magia débil
            _isUsingWeakAttack = true;

            Vector2 position = new Vector2(_transform.position.x, _transform.position.y + .8125f);

            // Instanciamos bola de fuego
            GameObject fireball = MonoBehaviour.Instantiate(
                _fireSettingsSO.WeakPrefab, // Prefab de la bola
                position, // Posición del player (desplazada un poco arriba)
                Quaternion.identity // Quaternion identity
                );

            fireball.transform.position = new Vector2(
                _transform.position.x,
                _transform.position.y + Constants.PLAYER_OFFSET
                );

            fireball.GetComponent<Fireball>().SetDirection(direction);
            _audioSpeaker.PlaySound(AudioID.G_FIRE, AudioID.S_FIRE_BALL);

            // Consumimos magia
            _magicEvents.UseOfMagicValue(_magicSettingsSO.Costs[0]);
            // Desactivamos el uso de la magia débil
            _isUsingWeakAttack = false;
            // Reseteamos el temporizador de uso de poder
            _playerStatus.RestartMagicTimer();
        }

        /// <summary>
        /// Activa el lanzallamas (si corresponde)
        /// </summary>
        public override void MediumAttack(Vector2 direction)
        {
            // Activamos el uso de la magia media
            _isUsingMediumAttack = true;
            Vector2 position = new Vector2(_transform.position.x, _transform.position.y + .8125f);

            GameObject flame = MonoBehaviour.Instantiate(
                _fireSettingsSO._mediumPrefab, // Prefab
                position + direction, // Posición del player (un poco desplazada hacia arriba)
                Quaternion.identity // Quaternion identity
                );

            flame.GetComponent<Flame>().Init(direction);
            // Consumimos la magia y reiniciamos el contador de tiempo
            _magicEvents.UseOfMagicValue(_magicSettingsSO.Costs[1]);
            _flameTimer = 0f;
        }

        /// <summary>
        /// Detiene el uso de lanzallamas
        /// </summary>
        public override void StopMediumAttack()
        {
            // Desactivamos el uso de magia media
            _isUsingMediumAttack = false;

            // Reseteamos el temporizador de uso de poder
            _playerStatus.RestartMagicTimer();
        }

        /// <summary>
        /// Activa una ráfaga de bolas de fuego que afecta a toda la pantalla
        /// </summary>
        public override void StrongAttack(Vector2 direction)
        {
            // Activamos el uso de la magia fuerte
            _isUsingStrongAttack = true;
            // Activamos el poder
            _magicEvents.MaxPowerUsed(_playerStatus.MaxPowerDuration);
            _gameStatus.AskChangeToInactiveState();
            _audioSpeaker.PlaySound(AudioID.G_FIRE, AudioID.S_FIRE_DEFINITIVE);
        }

        #endregion

        #region Private Methods

        #region Strong Attack

        private void ChangeOrbsState(bool state)
        {
            // Para cada orbe de la lista
            foreach (GameObject obj in _fireOrbs)
                // Se cambia el estado al opuesto que tiene en ese momento
                obj.SetActive(state);
        }

        private void MaxPowerFinalized()
        {
            // Comprobamos las colisiones (para dañar enemigos)
            CheckMaxPowerCollisions();

            // Desactivamos el uso de magia fuerte
            _isUsingStrongAttack = false;
            // Consumimos magia
            _magicEvents.UseOfMagicValue(_magicSettingsSO.Costs[2]);
            // Y reseteamos el contador de tiempo
            _playerStatus.RestartMagicTimer();
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
            seq.OnComplete(() =>
            {
                _magicEvents.MaxPowerFinalized();
                _gameStatus.AskChangeToGamePlayState();
            });
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

        /// <summary>
        /// Comprueba en un rango circular las colisiones
        /// </summary>
        private void CheckMaxPowerCollisions()
        {
            // Lista de colisiones
            Collider2D[] collisions = Physics2D.OverlapCircleAll(
                _transform.position, // Origen
                _fireOrbs.Count, // Radio
                LayerMask.GetMask(Constants.LAYER_INTERACTABLE) // Capa a la que afecta
                );

            // Para cada colisión, activamos el quemado
            foreach (Collider2D collision in collisions)
                collision.GetComponent<IBurnable>()?.Burn(_fireSettingsSO.StrongAttackDamage);

        }


        #endregion

        #endregion

    }
}
