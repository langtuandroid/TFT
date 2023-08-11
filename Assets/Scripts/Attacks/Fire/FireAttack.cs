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

        #region Unity Methods

        public FireAttack()
        {
            // Inicializamos las variables de estado
            base.Initialize();
            _flameTimer = 0f;
            _fireOrbs = new List<GameObject>();
        }

        public override void Init(MagicAttackSettingsSO magicSettings, PlayerStatus playerStatus, MagicEvents magicEvents, GameStatus gameStatus, IAudioSpeaker audioSpeaker, Transform transform)
        {
            base.Init(magicSettings, playerStatus, magicEvents, gameStatus, audioSpeaker, transform);

            _magicEvents.OnMaxPowerUsedValue += RotateOrbs;
            _magicEvents.OnMaxPowerFinalizedValue += MaxPowerFinalized;

            GameObject strongPrefab = MonoBehaviour.Instantiate(
                _fireSettingsSO.StrongPrefab,
                _transform
                );

            foreach (Transform t in strongPrefab.transform)
                _fireOrbs.Add(t.gameObject);

            strongPrefab.transform.localPosition = new Vector3(0f, .8125f, 0f);

        }

        public override void Destroy()
        {
            _magicEvents.OnMaxPowerUsedValue -= RotateOrbs;
            _magicEvents.OnMaxPowerFinalizedValue -= MaxPowerFinalized;
        }



        public override void Run()
        {

            if (_isUsingMediumAttack)
            {
                _flameTimer += Time.deltaTime;
                if (_flameTimer >= _fireSettingsSO.TimeBetweenConsuming)
                {
                    _magicEvents.UseOfMagicValue(_magicSettingsSO.Costs[1]);
                    _flameTimer = 0f;
                }
            }
        }

        #endregion

        #region Abstract class methods

        /// <summary>
        /// Lanza una bola de fuego
        /// </summary>
        public override void WeakAttack(Vector2 direction)
        {
            Debug.Log("Entro en ataque débil");
            // Activamos el uso de la magia débil
            _isUsingWeakAttack = true;

            // Instanciamos bola de fuego
            GameObject fireball = MonoBehaviour.Instantiate(
                _fireSettingsSO.WeakPrefab, // Prefab de la bola
                _transform.position, // Posición del player
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
            Debug.Log("Entro en ataque medio");

            // Activamos el uso de la magia media
            _isUsingMediumAttack = true;

            //// Creamos el prefab
            //GameObject prefab = null;

            //if (direction.Equals(Vector2.up))
            //    prefab = _flamesUp;
            //else if (direction.Equals(Vector2.down))
            //    prefab = _flamesDown;
            //else if (direction.Equals(Vector2.left))
            //    prefab = _flamesLeft;
            //else if (direction.Equals(Vector2.right))
            //    prefab = _flamesRight;

            //_flame = Instantiate(
            //    prefab, // Prefab de la llama
            //    transform
            //    );

            //_flame.transform.position = new Vector2(
            //    _flame.transform.position.x,
            //    _flame.transform.position.y + Constants.PLAYER_OFFSET
            //    );

            ////_audioSpeaker.PlaySound( AudioID.G_FIRE , AudioID.S_FLAMETHROWER );
            //_flame.GetComponent<ParticleSystem>().Play();
        }

        /// <summary>
        /// Detiene el uso de lanzallamas
        /// </summary>
        public override void StopMediumAttack()
        {
            //// Lo quitamos
            //_flame.transform.parent = null;
            //// Y lo paramos
            //_flame.GetComponent<ParticleSystem>().Stop();

            //_flamesToDestroy.Add(_flame);
            //Invoke(nameof(DisableAndDestroy), 4f);

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

        #region Medium Attack

        ///// <summary>
        ///// Desactiva y destruye el objeto de lanzallamas
        ///// </summary>
        //private void DisableAndDestroy()
        //{
        //    GameObject obj = _flamesToDestroy[0];
        //    _flamesToDestroy.Remove(obj);
        //    Destroy(obj);
        //}

        #endregion

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
