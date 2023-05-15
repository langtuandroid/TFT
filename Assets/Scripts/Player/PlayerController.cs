using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Private variables
        // SERVICES
        private GameInputs _gameInputs;

        // SCRIPTS DEL JUGADOR
        private PlayerMovement _movement;
        private Interaction _interaction;
        private Jump _jump;
        private PlayerMagicAttack _magicAttack;
        private AnimatorBrain _animatorBrain;

        // VARIABLES
        // Jump input
        private bool _isJumpInput;

        // Movement input
        private Vector2 _direction;
        private Vector2 _lookDirection;

        // Interact input
        private bool _isPhysicActionInput;

        // Attack input
        private bool _isPhysicAttacking;
        private bool _isWeakMagicInput;
        private bool _isMediumMagicInput;
        private bool _isStrongMagicInput;
        private bool _mediumMagicUsed;

        #endregion

        #region Unity methods

        private void Awake()
        {
            // Obtenemos componentes
            _movement = GetComponent<PlayerMovement>();
            _jump = GetComponent<Jump>();
            _interaction = GetComponent<Interaction>();
            _magicAttack = GetComponent<PlayerMagicAttack>();
            _animatorBrain = GetComponentInChildren<AnimatorBrain>();
        }

        private void Start()
        {
            _movement.Init();
            _jump.Init();
            _interaction.Init();
            _animatorBrain.Init();

            _gameInputs = ServiceLocator.GetService<GameInputs>();
            _gameInputs.OnJumpButtonStarted           += GameInputs_OnJumpButtonStarted;
            _gameInputs.OnJumpButtonCanceled          += GameInputs_OnJumpButtonCanceled;
            _gameInputs.OnPhysicActionButtonPerformed += GameInputs_OnPhysicActionButtonPerformed;
            _gameInputs.OnMediumAttackButtonStarted   += GameInputs_OnMediumAttackButtonStarted;
            _gameInputs.OnMediumAttackButtonCanceled  += GameInputs_OnMediumAttackButtonCanceled;
            _gameInputs.OnWeakAttackButtonStarted     += GameInputs_OnWeakAttackButtonStarted;
            _gameInputs.OnWeakAttackButtonCanceled    += GameInputs_OnWeakAttackButtonCanceled;
            _gameInputs.OnStrongAttackPerformed       += GameInputs_OnStrongAttackButtonPerformed;
        }

        private void OnDestroy()
        {
            _gameInputs.OnJumpButtonStarted           -= GameInputs_OnJumpButtonStarted;
            _gameInputs.OnJumpButtonCanceled          -= GameInputs_OnJumpButtonCanceled;
            _gameInputs.OnPhysicActionButtonPerformed -= GameInputs_OnPhysicActionButtonPerformed;
            _gameInputs.OnMediumAttackButtonStarted   -= GameInputs_OnMediumAttackButtonStarted;
            _gameInputs.OnMediumAttackButtonCanceled  -= GameInputs_OnMediumAttackButtonCanceled;
            _gameInputs.OnWeakAttackButtonStarted     -= GameInputs_OnWeakAttackButtonStarted;
            _gameInputs.OnWeakAttackButtonCanceled    -= GameInputs_OnWeakAttackButtonCanceled;
            _gameInputs.OnStrongAttackPerformed       -= GameInputs_OnStrongAttackButtonPerformed;
        }

        private void Update()
        {
            // Controlamos las acciones
            GetActionsInformation();

            // Realizamos salto
            DoJump();
            // Realizamos interacción
            DoInteraction();
            // Atacamos con magia
            //DoMagicAttack();

            // Cambiamos la animación según corresponda
            SetWalkingAnim();
        }

        private void FixedUpdate()
        {
            DoMove();
        }

        #endregion

        private void GetActionsInformation()
        {
            // Obtenemos la dirección
            _direction = _gameInputs.GetDirectionNormalized();


            if ( _jump.IsPerformingJump || _mediumMagicUsed )
                return;

            _lookDirection = _animatorBrain.LookDirection( _direction );
        }

        private void DoMove()
        {
            if ( _jump.IsOnAir )
                _movement.MoveOnAir( _direction );
            else
            if ( _jump.IsCooldown )
                _movement.Stop();
            else
            if ( !_jump.IsPerformingJump )
                _movement.Move( _direction );
            else
                _movement.Stop();
        }

        #region Jump
        private void GameInputs_OnJumpButtonCanceled() => _isJumpInput = false;
        private void GameInputs_OnJumpButtonStarted()
        {
            if ( CanJump() )
                _isJumpInput = true;
        }

        private void DoJump()
        {
            //if (IsAttacking())
            //    return;

            _jump.JumpAction( _isJumpInput , _lookDirection , _direction );
            if ( !_jump.IsPerformingJump )
                _isJumpInput = false;
        }

        #endregion

        #region Interact

        private void GameInputs_OnPhysicActionButtonPerformed() => _isPhysicActionInput = true;


        private void DoInteraction()
        {
            if ( _jump.IsPerformingJump || IsAttacking())
                return;

            if (_interaction.CanInteract(_lookDirection))
            {
                if (_isPhysicActionInput)
                {
                    _interaction.Interact(_lookDirection);
                    _isPhysicActionInput = false;
                }
            }
            else
            {
                _interaction.StopInteracting();
            }
        }

        #endregion

        #region States Control

        private bool CanJump()
        {
            return !_jump.IsPerformingJump && !IsAttacking();
        }

        private bool IsAttacking()
        {
            return _isPhysicAttacking || _isWeakMagicInput
                || _isMediumMagicInput || _isStrongMagicInput;
        }

        #region Physic Attack


        #endregion

        #region Magic attack

        private void GameInputs_OnWeakAttackButtonStarted()
        {
            if ( _jump.IsPerformingJump )
                return;
            _isWeakMagicInput = true;
        }
        private void GameInputs_OnWeakAttackButtonCanceled() => _isWeakMagicInput = false;
        private void GameInputs_OnMediumAttackButtonStarted()
        {
            if ( _jump.IsPerformingJump )
                return;
            _isMediumMagicInput = true;
        }
        private void GameInputs_OnMediumAttackButtonCanceled() => _isMediumMagicInput = false;

        private void GameInputs_OnStrongAttackButtonPerformed()
        {
            if ( _jump.IsPerformingJump )
                return;
            _isStrongMagicInput = true;
        }

        /// <summary>
        /// Gestiona los ataques mágicos
        /// </summary>
        private void DoMagicAttack()
        {
            if ( _jump.IsPerformingJump ||
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
            if (!_isWeakMagicInput)
                return;

            if (_isMediumMagicInput || _isStrongMagicInput)
                _isWeakMagicInput = false;

            else
            {
                // Ponemos a false todas las variables de magia
                _isWeakMagicInput = false;
                _isMediumMagicInput = false;
                _isStrongMagicInput = false;
                _magicAttack.ResetTimer();
                _magicAttack.WeakAttack(_lookDirection);
            }
        }

        /// <summary>
        /// Gestiona los ataques medios
        /// </summary>
        private void DoMediumMagicAttack()
        {
            if (!_isMediumMagicInput)
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
            _magicAttack.MediumAttack(_lookDirection);

        }

        /// <summary>
        /// Gestiona los ataques fuertes
        /// </summary>
        private void DoStrongMagicAttack()
        {
            if (!_isStrongMagicInput)
                return;

            if (_mediumMagicUsed)
            {
                _isStrongMagicInput = false;
                return;
            }

            // Ponemos a false todas las variables de magia
            _isStrongMagicInput = false;
            _magicAttack.ResetTimer();
            _magicAttack.StrongAttack();
        }

        #endregion

        #endregion

        // TODO: Selecci�n de tipo de acci�n
        private void SelectElement()
        {

        }

        private void SetWalkingAnim()
        {
            // Si está saltando
            if ( _jump.IsPerformingJump )
                return;

            _animatorBrain.IsWalking( _direction.magnitude > 0 );
        }
    }
}