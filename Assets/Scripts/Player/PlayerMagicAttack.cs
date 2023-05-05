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
        private float _cooldownTime; // Tiempo entre ataques
        //[SerializeField]
        //private GameObject _powerPanel; // Panel para el poder final

        [Header("Light Power")]
        [SerializeField]
        private GameObject _lightPrefab; // Prefab de la bola de luz

        [Header("Fire Power")]
        [SerializeField]
        private GameObject _fireBallPrefab; // Prefab de la bola de fuego

        //[SerializeField]
        //private GameObject[] _flamesUp; // Lanzallamas hacia arriba
        //[SerializeField]
        //private GameObject[] _flamesRight; // Lanzallamas hacia la derecha
        //[SerializeField]
        //private GameObject[] _flamesDown; // Lanzallamas hacia abajo
        //[SerializeField]
        //private GameObject[] _flamesLeft; // Lanzallamas hacia la izda

        //[SerializeField]
        //private GameObject[] _fireOrbs; // Bolas del ataque fuerte de fuego

        #endregion

        #region Private Variables

        private IAttack _attack; // Tipo de ataque que se usa
        private float _timer; // Temporizador para el cooldown

        private MagicEvents _magicEvents;

        // CORRUTINAS (para más adelante)
        //private Coroutine _firePowerCoroutine; // Corrutina del poder de fuego

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _magicEvents = ServiceLocator.GetService<MagicEvents>();
            _attack = new FireAttack();
            _timer = _cooldownTime;
        }

        private void Update()
        {
            if (_timer < _cooldownTime)
                _timer += Time.deltaTime;
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

        /// <summary>
        /// Realiza el ataque débil
        /// </summary>
        public void WeakAttack(Vector2 direction)
        {
            _attack = new FireAttack();

            _attack.SetOriginAndDirection(transform, direction);

            if (_attack.GetType() == typeof(FireAttack))
                _attack.WeakAttack(_fireBallPrefab);
        }

        /// <summary>
        /// Realiza el ataque medio
        /// </summary>
        public void MediumAttack()
        {

        }


        /// <summary>
        /// Realiza el ataque fuerte
        /// </summary>
        public void StrongAttack()
        {
            _magicEvents.SetPanelColor(_attack);
        }


        #endregion

        #region Public Methods

        public void ResetValues()
        {
            // Reseteamos las variables intrínsecas del ataque
            _attack.ResetValues();
            _timer = 0f;
        }

        #endregion

    }

}
