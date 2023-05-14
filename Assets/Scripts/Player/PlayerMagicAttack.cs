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

        [Header("Fire Power")]
        [SerializeField]
        [Tooltip("Prefab de la bola de fuego.")]
        private GameObject _fireBallPrefab;

        [SerializeField]
        [Tooltip("Prefab del lanzallamas hacia arriba")]
        private GameObject _flamesUp;
        [SerializeField]
        [Tooltip("Prefab del lanzallamas hacia la derecha")]
        private GameObject _flamesRight;
        [SerializeField]
        [Tooltip("Prefab del lanzallamas hacia abajo")]
        private GameObject _flamesDown;
        [SerializeField]
        [Tooltip("Prefab del lanzallamas hacia la izda")]
        private GameObject _flamesLeft;

        [SerializeField]
        [Tooltip("Lista de orbes que giran alrededor del personaje al usar el poder máximo de fuego")]
        private List<GameObject> _fireOrbs;

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
            _attack = gameObject.AddComponent<FireAttack>();
            _timer = _cooldownTime;
        }

        private void Start()
        {
            _magicEvents = ServiceLocator.GetService<MagicEvents>();

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
            _attack.SetDirection(direction);

            if (_attack.GetType() == typeof(FireAttack))
                _attack.WeakAttack(_fireBallPrefab);
        }

        /// <summary>
        /// Realiza el ataque medio
        /// </summary>
        public void MediumAttack(Vector2 direction)
        {
            _attack.SetDirection(direction);

            if (_attack.GetType() == typeof(FireAttack))
            {
                GameObject flame = null;
                if (direction.Equals(Vector2.up))
                    flame = _flamesUp;
                else if (direction.Equals(Vector2.down))
                    flame = _flamesDown;
                else if (direction.Equals(Vector2.left))
                    flame = _flamesLeft;
                else if (direction.Equals(Vector2.right))
                    flame = _flamesRight;

                _attack.MediumAttack(flame);
            }
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
            if (_attack.GetType() == typeof(FireAttack))
                _attack.StrongAttack(_fireOrbs);
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
