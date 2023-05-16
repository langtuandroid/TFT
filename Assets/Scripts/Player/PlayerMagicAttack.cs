using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Attack;
using Utils;

namespace Player
{
    public class PlayerMagicAttack : MonoBehaviour
    {

        #region Serialize Fields

        [Header("Attack Settings")]
        [SerializeField]
        [Tooltip("Tiempo entre ataques mágicos")]
        private float _cooldownTime;

        [SerializeField]
        [Tooltip("Tiempo de recarga del poder máximo")]
        private float _maxPowerTime = 10f;


        #endregion

        #region Private Variables

        private MagicAttack _attack; // Tipo de ataque que se usa
        private float _timer; // Temporizador para el cooldown
        private float _maxPowerTimer; // Temporizador para el poder máximo

        private MagicEvents _magicEvents;

        // CORRUTINAS (para más adelante)
        //private Coroutine _firePowerCoroutine; // Corrutina del poder de fuego

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _timer = _cooldownTime;
            _maxPowerTimer = _maxPowerTime;
        }

        private void Start()
        {
            // EVENTOS
            _magicEvents = ServiceLocator.GetService<MagicEvents>();
            // COMPONENTES
            _attack = gameObject.GetComponent<FireAttack>();

            // Invocamos al evento
            _magicEvents.ChangeAttackType(_attack);
        }


        private void Update()
        {

            if (_timer < _cooldownTime)
                _timer += Time.deltaTime;

            if (_maxPowerTimer < _maxPowerTime)
                _maxPowerTimer += Time.deltaTime;
        }

        #endregion

        #region Private Methods


        #endregion

        #region Attacking

        /// <summary>
        /// Mira si puede atacar y en caso contrario, 
        /// incrementa un contador de tiempo
        /// </summary>
        /// <returns></returns>
        public bool CanAttack()
        {
            return _timer >= _cooldownTime;
        }

        public bool CanUseMaxAttack()
        {
            return _maxPowerTimer >= _maxPowerTime;
        }

        /// <summary>
        /// Realiza el ataque débil
        /// </summary>
        public void WeakAttack(Vector2 direction)
        {
            _attack.SetDirection(direction);

            _attack.WeakAttack();
        }

        /// <summary>
        /// Realiza el ataque medio
        /// </summary>
        public void MediumAttack(Vector2 direction)
        {
            // Cambiamos la dirección del ataque
            _attack.SetDirection(direction);
            // Y ejecutamos el ataque
            _attack.MediumAttack();
        }

        public void StopMediumAttack()
        {
            _attack.StopMediumAttack();
        }

        /// <summary>
        /// Realiza el ataque fuerte
        /// </summary>
        public void StrongAttack()
        {
            _maxPowerTimer = 0f;
            _attack.StrongAttack();
        }


        #endregion

        #region Public Methods

        public void ResetTimer()
        {
            // Reseteamos las variables intrínsecas del ataque
            _timer = 0f;
        }

        #endregion

    }

}
