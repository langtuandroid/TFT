using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Public variables

        public Vector2 LookDirection => _lookDirection;

        #endregion


        #region Private variables
        // SERVICES
        private GameInputs _gameInputs;
        private MagicEvents _magicEvents;

        // SCRIPTS DEL JUGADOR
        // Script de movimiento del personaje
        private PlayerMovement _movement;
        // Script de interacción del personaje
        private Interaction _interaction;
        // Script de salto del personaje
        private Jump _jump;
        // Script de ataque del personaje
        private PlayerMagicAttack _magicAttack;

        // COMPONENTES
        // Animator del player
        private Animator _anim;

        // VARIABLES
        // Jump state
        private bool _isJumping;

        // Movement state
        private Vector2 _direction;

        // Interact state
        private bool _isInteracting;
        private Vector2 _lookDirection;

        // Attack states
        private bool _isPhysicAttacking;
        private bool _isUsingWeakMagic;
        private bool _isUsingMediumMagic;
        private bool _isUsingStrongMagic;
        private bool _mediumMagicUsed;


        // Axis (for animator)
        private float _lastX;
        private float _lastY;

        #endregion

        #region Unity methods

        private void Awake()
        {

        }

        private void Start()
        {
            // Obtenemos componentes
            _movement = GetComponent<PlayerMovement>();
            _jump = GetComponent<Jump>();
            _interaction = GetComponent<Interaction>();
            _magicAttack = GetComponent<PlayerMagicAttack>();

            _anim = GetComponentInChildren<Animator>();

            _jump.Init();
            _interaction.Init();

            // Inicializamos variables
            // Jump state
            _isJumping = false;
            // Interact state
            _isInteracting = false;
            // Attack states
            _isPhysicAttacking = false;
            _isUsingWeakMagic = false;
            _isUsingMediumMagic = false;
            _isUsingStrongMagic = false;
            _mediumMagicUsed = false;

            // Axis
            _lastX = 0f;
            _lastY = -1f; // Al principio mira hacia abajo

            _gameInputs = ServiceLocator.GetService<GameInputs>();
            _gameInputs.OnJumpButtonStarted += GameInputs_OnJumpButtonStarted;
            _gameInputs.OnJumpButtonCanceled += GameInputs_OnJumpButtonCanceled;
            _gameInputs.OnPhysicActionButtonPerformed += GameInputs_OnPhysicActionButtonPerformed;
            _gameInputs.OnMediumAttackButtonStarted += GameInputs_OnMediumAttackButtonStarted;
            _gameInputs.OnMediumAttackButtonCanceled += GameInputs_OnMediumAttackButtonCanceled;
            _gameInputs.OnWeakAttackButtonStarted += GameInputs_OnWeakAttackButtonStarted;
            _gameInputs.OnWeakAttackButtonCanceled += GameInputs_OnWeakAttackButtonCanceled;
            _gameInputs.OnStrongAttackPerformed += GameInputs_OnStrongAttackButtonPerformed;
        }

        private void OnDestroy()
        {
            _gameInputs.OnJumpButtonStarted -= GameInputs_OnJumpButtonStarted;
            _gameInputs.OnJumpButtonCanceled -= GameInputs_OnJumpButtonCanceled;
            _gameInputs.OnPhysicActionButtonPerformed -= GameInputs_OnPhysicActionButtonPerformed;
            _gameInputs.OnMediumAttackButtonStarted -= GameInputs_OnMediumAttackButtonStarted;
            _gameInputs.OnMediumAttackButtonCanceled -= GameInputs_OnMediumAttackButtonCanceled;
            _gameInputs.OnWeakAttackButtonStarted += GameInputs_OnWeakAttackButtonStarted;
            _gameInputs.OnWeakAttackButtonCanceled += GameInputs_OnWeakAttackButtonCanceled;
            _gameInputs.OnStrongAttackPerformed += GameInputs_OnStrongAttackButtonPerformed;
        }

        private void Update()
        {
            // Controlamos las acciones
            GetActionsInformation();

            // Realizamos acciones
            DoUpdateActions();

            // Cambiamos la animación según corresponda
            SetAnimations();
        }

        private void FixedUpdate()
        {
            DoFixedUpdateActions();
        }

        #endregion

        #region Private methods

        #region Actions

        private void GetActionsInformation()
        {
            // Obtenemos la dirección
            GetDirection();

            // Vemos si interactuamos
            GetInteraction();
        }

        private void DoUpdateActions()
        {
            // Realizamos salto
            DoJump();
            // Realizamos interacción
            DoInteraction();
            // Atacamos con magia
            //DoMagicAttack();
        }

        private void DoFixedUpdateActions()
        {
            // Movemos a nuestro player
            DoMove();
        }

        #region Movement

        private void GetDirection()
        {
            // Obtenemos el vector de dirección
            _direction = _gameInputs.GetDirectionNormalized();
        }

        private void DoMove()
        {
            _movement.Move(_direction);
        }

        #endregion

        #region Jump
        private void GameInputs_OnJumpButtonCanceled() => _isJumping = false;
        private void GameInputs_OnJumpButtonStarted()
        {
            if (_jump.CanJump)
                _isJumping = true;
        }

        private void DoJump()
        {
            if (IsAttacking())
                return;

            if (_isJumping)
                _isJumping = _jump.JumpAction();
            else
                _jump.Fall();
        }

        #endregion

        #region Interact

        private void GameInputs_OnPhysicActionButtonPerformed() => _isInteracting = true;

        private void GetInteraction()
        {
            if (_direction.magnitude > 0.05f)
                _lookDirection = _direction;
        }

        private void DoInteraction()
        {
            if ( _jump.IsPerformingJump || IsAttacking())
                return;

            if (_interaction.CanInteract(_lookDirection))
            {
                if (_isInteracting)
                {
                    _interaction.Interact(_lookDirection);
                    _isInteracting = false;
                }
            }
            else
            {
                _interaction.StopInteracting();
            }
        }

        #endregion

        #region Attack

        private bool IsAttacking()
        {
            return _isPhysicAttacking || _isUsingWeakMagic
                || _isUsingMediumMagic || _isUsingStrongMagic;
        }

        #region Physic Attack


        #endregion

        #region Magic attack

        private void GameInputs_OnWeakAttackButtonStarted() => _isUsingWeakMagic = true;
        private void GameInputs_OnWeakAttackButtonCanceled() => _isUsingWeakMagic = false;
        private void GameInputs_OnMediumAttackButtonStarted() => _isUsingMediumMagic = true;
        private void GameInputs_OnMediumAttackButtonCanceled() => _isUsingMediumMagic = false;

        private void GameInputs_OnStrongAttackButtonPerformed() => _isUsingStrongMagic = true;

        /// <summary>
        /// Gestiona los ataques mágicos
        /// </summary>
        private void DoMagicAttack()
        {
            if (_isJumping ||
                !_magicAttack.CanAttack())
                return;

            DoWeakMagicAttack();
            DoMediumMagicAttack();
            DoStrongMagicAttack();
        }

        /// <summary>
        /// Gestiona los ataques débiles
        /// </summary>
        private void DoWeakMagicAttack()
        {
            if (!_isUsingWeakMagic)
                return;

            if (_isUsingMediumMagic || _isUsingStrongMagic)
                _isUsingWeakMagic = false;

            else
            {
                // Ponemos a false todas las variables de magia
                _isUsingWeakMagic = false;
                _isUsingMediumMagic = false;
                _isUsingStrongMagic = false;
                _magicAttack.ResetTimer();
                _magicAttack.WeakAttack(new Vector2(_lastX, _lastY));
            }
        }

        /// <summary>
        /// Gestiona los ataques medios
        /// </summary>
        private void DoMediumMagicAttack()
        {
            if (!_isUsingMediumMagic)
            {
                if (_mediumMagicUsed)
                {
                    _mediumMagicUsed = false;
                    _magicAttack.StopMediumAttack();
                    _magicAttack.ResetTimer();
                }
                return;
            }
            if (_mediumMagicUsed)
                return;

            // Ponemos a false todas las variables de magia
            _mediumMagicUsed = true;
            _magicAttack.MediumAttack(new Vector2(_lastX, _lastY));

        }

        /// <summary>
        /// Gestiona los ataques fuertes
        /// </summary>
        private void DoStrongMagicAttack()
        {
            if (!_isUsingStrongMagic)
                return;

            if (_mediumMagicUsed)
            {
                _isUsingStrongMagic = false;
                return;
            }

            // Ponemos a false todas las variables de magia
            _isUsingStrongMagic = false;
            _magicAttack.ResetTimer();
            _magicAttack.StrongAttack();
        }

        #endregion

        #endregion

        #region Selections

        // TODO: Selecci�n de tipo de acci�n
        private void SelectElement()
        {

        }

        #endregion


        #endregion

        #region Animations

        private void SetAnimations()
        {
            // Controlamos los saltos
            _anim.SetBool(Constants.ANIM_PLAYER_JUMP, _jump.IsPerformingJump );
            // Si está saltando
            if ( _jump.IsPerformingJump )
                // Volvemos
                return;

            // Controlamos el movimiento
            ControlWalking2();
        }

        private void ControlWalking()
        {
            bool isWalking = _direction.magnitude > 0f;
            _anim.SetBool(Constants.ANIM_PLAYER_WALKING, isWalking);

            if (_mediumMagicUsed)
                return;

            if (isWalking)
            {
                float x = _direction.x;
                float y = _direction.y;


                _lastX = (Mathf.Abs(_lastY) > 0f &&
                    Mathf.Abs(y) > 0f &&
                    _lastX == 0f && Mathf.Abs(x) > 0f) ?
                    _lastX : x;

                _lastY = (Mathf.Abs(_lastX) > 0f &&
                    Mathf.Abs(x) > 0f &&
                    _lastY == 0f && Mathf.Abs(y) > 0f) ?
                    _lastY : y;

                _anim.SetFloat(Constants.ANIM_PLAYER_DIRX, _lastX);
                _anim.SetFloat(Constants.ANIM_PLAYER_DIRY, _lastY);
            }
        }

        private void ControlWalking2()
        {
            bool isWalking = _direction.magnitude > 0f;
            _anim.SetBool(Constants.ANIM_PLAYER_WALKING, isWalking);

            if (_mediumMagicUsed)
                return;

            if (isWalking)
            {
                float x = _direction.x;
                float y = _direction.y;

                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    _lastX = x > 0f ? 1f : -1f;
                    _lastY = 0f;
                }
                else if (Mathf.Abs(y) > Mathf.Abs(x))
                {
                    _lastX = 0f;
                    _lastY = y > 0f ? 1f : -1f;
                }
                else
                {
                    if (x == 0f && y == 0f)
                    {
                        _lastX = x;
                        _lastY = y;
                    }
                    else if (Mathf.Abs(_lastX) > Mathf.Abs(_lastY))
                    {
                        _lastX = x > 0f ? 1f : -1f;
                        _lastY = 0f;
                    }
                    else
                    {
                        _lastX = 0f;
                        _lastY = y > 0f ? 1f : -1f;
                    }
                }

                _anim.SetFloat(Constants.ANIM_PLAYER_DIRX, _lastX);
                _anim.SetFloat(Constants.ANIM_PLAYER_DIRY, _lastY);
            }
        }

        #endregion

        #endregion
    }
}