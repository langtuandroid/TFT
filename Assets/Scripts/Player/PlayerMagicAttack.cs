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

        [SerializeField]
        private GameObject[] _flamesUp; // Lanzallamas hacia arriba
        [SerializeField]
        private GameObject[] _flamesRight; // Lanzallamas hacia la derecha
        [SerializeField]
        private GameObject[] _flamesDown; // Lanzallamas hacia abajo
        [SerializeField]
        private GameObject[] _flamesLeft; // Lanzallamas hacia la izda

        [SerializeField]
        private GameObject[] _fireOrbs; // Bolas del ataque fuerte de fuego

        #endregion

        #region Public Variables

        // Transformada del player
        public Transform Transform => transform;

        // Indica si está atacando
        public bool IsAttacking => _isAttackButtonPressed;

        #region Light Power

        // Prefab de la bola de luz
        public GameObject LightBall => _lightPrefab;

        #endregion

        #region Fire Power

        // Prefab de la bola de fuego
        public GameObject FireBall => _fireBallPrefab;

        // Partes del lanzallamas
        public List<GameObject[]> Flames => new List<GameObject[]>
    {
        // Va en sentido de las agujas del reloj
        _flamesUp,
        _flamesRight,
        _flamesDown,
        _flamesLeft
    };

        // Orbes para el poder final de fuego
        public GameObject[] FireOrbs => _fireOrbs;

        #endregion

        // Panel para el poder final
        //public GameObject PowerPanel => _powerPanel;

        // Imagen del panel (para colorear)
        public Image PanelImage => _panelImage;

        #endregion

        #region Private Variables

        private MagicEvents _magicEvents;

        private IAttack _attack; // Tipo de ataque que se usa
        private bool _isAttackButtonPressed; // Comprobador de si se ha pulsado el botón de ataque
        private Image _panelImage; // Imagen del panel
        private float _timer; // Temporizador para el cooldown

        // CORRUTINAS (para más adelante)
        //private Coroutine _firePowerCoroutine; // Corrutina del poder de fuego

        #endregion

        #region Unity Methods

        private void Awake()
        {
            // Obtenemos componentes
            //_panelImage = _powerPanel.GetComponent<Image>();
            // Inicializamos variables
            // Asignamos el poder de fuego (con botón de poder no pulsado)
            AssignPower(new FireAttack(), Color.red);
            //MyUIManager.Instance.SelectPower(0);

        }

        private void Start()
        {
            _magicEvents = ServiceLocator.GetService<MagicEvents>();
        }

        private void Update()
        {
            // Si el juego está "parado"
            //if (MyGameManager.Instance.Stoped)
            //    // Volvemos
            //    return;

            // Controlamos ataque
            if (CanAttack())
                Attack();
        }

        #endregion

        #region Private Methods

        #region Assignment of powers

        private bool SelectTypeOfAttack()
        {
            bool res = false;

            //// Flecha arriba
            //if (res = Input.GetKeyDown(KeyCode.Keypad8))
            //    MyUIManager.Instance.SelectPower(0);
            //// Flecha derecha
            //else if (res = Input.GetKeyDown(KeyCode.Keypad6))
            //    MyUIManager.Instance.SelectPower(1);
            //// Flecha abajo
            //else if (res = Input.GetKeyDown(KeyCode.Keypad2))
            //    MyUIManager.Instance.SelectPower(2);
            //// Flecha izda
            //else if (res = Input.GetKeyDown(KeyCode.Keypad4))
            //    MyUIManager.Instance.SelectPower(3);

            return res;
        }

        /// <summary>
        /// Método para asignar un tipo de poder de ataque.
        /// Devuelve true en caso de existir cambios.
        /// </summary>
        private bool AssignTypeOfAttack()
        {
            // Si pulsamos la tecla "R" -> Cambio de poder
            bool res = Input.GetKeyDown(KeyCode.L);
            // CAMBIO DE PODERES
            if (res)
            {
                var type = _attack.GetType();
                // CAMBIO A PODER DE LUZ
                // Si tenía el poder de fuego asignado
                if (type == typeof(FireAttack))
                    // Asignamos el poder de luz
                    AssignPower(new LightAttack());
                // Si por el contrario, tiene poder de luz
                else if (type == typeof(LightAttack))
                    // Asignamos poder de fuego (con panel rojo)
                    AssignPower(new FireAttack(), Color.red);

                //MyUIManager.Instance.ChangeIcon(_attack);
            }

            return res;
        }

        /// <summary>
        /// Asigna un tipo de poder dado
        /// </summary>
        /// <param name="power"></param>
        private void AssignPower(IAttack power)
        {
            // Asignamos el poder en cuestión
            _attack = power;
            // Y desactivamos la opción de que el poder esté pulsado
            _isAttackButtonPressed = false;
        }

        /// <summary>
        /// Asigna un tipo de poder dado
        /// y configura el panel del poder con el color dado
        /// </summary>
        /// <param name="power"></param>
        /// <param name="color"></param>
        private void AssignPower(IAttack power, Color color)
        {
            // Asignamos el poder
            AssignPower(power);
            // Y configuramos el panel del poder
            //_panelImage.color = color;
            //_panelImage.SetImageAlpha(0f);
        }

        #endregion

        #region Attacking

        /// <summary>
        /// Mira si puede atacar y en caso contrario, 
        /// incrementa un contador de tiempo
        /// </summary>
        /// <returns></returns>
        private bool CanAttack()
        {
            bool res = true;

            if (_timer < _cooldownTime)
            {
                _timer += Time.deltaTime;
                res = false;
            }

            return res;
        }

        /// <summary>
        /// Gestiona los ataques
        /// </summary>
        public void Attack()
        {
            return;
            //// Si seleccionamos un tipo de ataque
            //if (SelectTypeOfAttack())
            //    // Volvemos
            //    return;

            //// Miramos un tipo de ataque en función de la tecla que pulses
            //if (AssignTypeOfAttack())
            //    // En caso de hacerlo, volvemos
            //    return;


            //// Controlamos el estado del ataque fuerte
            //SetStrongAttackState();

            //// Finalmente, controlamos las pulsaciones
            //// del botón de atacar
            //AttackPressing();
        }

        /// <summary>
        /// Controla las pulsaciones del botón de ataque
        /// </summary>
        private void AttackPressing()
        {
            // En caso de pulsar el botón de ataque
            if (Input.GetMouseButtonDown(0))
                // Activamos su variable
                _isAttackButtonPressed = true;
            // Si no, si se suelta
            else if (Input.GetMouseButtonUp(0)
                && _isAttackButtonPressed)
            {
                // Desactivamos su variable
                _isAttackButtonPressed = false;
                _attack.Execute(false);
                // Reiniciamos el contador
                _timer = 0f;
            }

            // Si se ha pulsado el botón de atacar
            if (_isAttackButtonPressed)
                // Ejecutamos el ataque
                _attack.Execute(true);
        }

        /// <summary>
        /// Cambia el estado del ataque fuerte según
        /// se pulse (o no) el botón de ataque
        /// </summary>
        private void SetStrongAttackState()
        {
            // Si pulsamos el botón de ataque final
            if (Input.GetKeyDown(KeyCode.R))
                // Cambiamos el estado a ataque fuerte
                ChangeStrongAttackState();

        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Cambia el estado a ataque fuerte
        /// </summary>
        public void ChangeStrongAttackState()
        {
            // TODO: Tener en cuenta si se puede
            // (p. ej. si está cargado el ataque y demás)
            if (_attack.GetType() == typeof(FireAttack)
                //&& MyUIManager.Instance.FireValue == 1f
                )
            {
                // Cambiamos el estado de uso de poder fuerte
                //_attack.ChangeStrongAttackState();
                // Desactivamos el botón de ataque
                _isAttackButtonPressed = false;
                // Y reseteamos los valores de ataque
                //_attack.ResetValues();
            }

        }

        public void ResetValues()
        {
            // Reseteamos las variables intrínsecas del ataque
            //_attack.ResetValues();
            // Y pulsamos el botón de ataque
            _isAttackButtonPressed = false;
        }

        #endregion

    }

}
