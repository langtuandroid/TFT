using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {

        #region Private variables
        // INPUTS
        private GameInputs _gameInputs;

        // SCRIPTS DEL JUGADOR
        // Script de movimiento del personaje
        private PlayerMovement _movement;
        // Script de interacción del personaje
        private Interaction _interaction;
        // Script de salto del personaje
        private Jump _jump;


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

        // Axis (for animator)
        private float _lastX;
        private float _lastY;

        #endregion

        #region Unity methods

        private void Awake()
        {
            // Obtenemos componentes
            _movement = GetComponent<PlayerMovement>();
            _interaction = GetComponent<Interaction>();
            _jump = GetComponent<Jump>();

            _anim = GetComponentInChildren<Animator>();

            // Inicializamos variables
            // Jump state
            _isJumping = false;
            // Interact state
            _isInteracting = false;

            // Axis
            _lastX = 0f;
            _lastY = 0f;

        }

        private void Start()
        {
            _gameInputs = ServiceLocator.GetService<GameInputs>();
            _gameInputs.OnSouthButtonStarted += GameInputs_OnSouthButtonStarted;
            _gameInputs.OnSouthButtonCanceled += GameInputs_OnSouthButtonCanceled;
            _gameInputs.OnEastButtonPerformed += GameInputs_OnEastButtonPerformed;
        }

        private void OnDestroy()
        {
            _gameInputs.OnSouthButtonStarted -= GameInputs_OnSouthButtonStarted;
            _gameInputs.OnSouthButtonCanceled -= GameInputs_OnSouthButtonCanceled;
            _gameInputs.OnEastButtonPerformed -= GameInputs_OnEastButtonPerformed;
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
        private void GameInputs_OnSouthButtonCanceled() => _isJumping = false;
        private void GameInputs_OnSouthButtonStarted()
        {
            if (_jump.CanJump)
                _isJumping = true;
        }

        private void DoJump()
        {
            if (_isJumping)
                _jump.JumpAction();
            else
                _jump.Fall();
        }

        #endregion

        #region Interact

        private void GameInputs_OnEastButtonPerformed() => _isInteracting = true;

        private void GetInteraction()
        {
            if (_direction.magnitude > 0.05f)
                _lookDirection = _direction;
        }

        private void DoInteraction()
        {
            if (_isJumping)
                return;

            if (_isInteracting)
            {
                _interaction.Interact(_lookDirection);
                _isInteracting = false;
            }
        }


        #endregion

        #endregion

        #region Animations

        private void SetAnimations()
        {
            // Controlamos los saltos
            _anim.SetBool(Constants.ANIM_PLAYER_JUMP, _isJumping);
            // Si está saltando
            if (_isJumping)
                // Volvemos
                return;

            // Controlamos el movimiento
            ControlWalking2();
        }

        private void ControlWalking()
        {
            bool isWalking = _direction.magnitude > 0f;
            _anim.SetBool(Constants.ANIM_PLAYER_WALKING, isWalking);

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