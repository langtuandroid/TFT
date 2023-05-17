using Attack;
using System.Collections;
using System.Collections.Generic;
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
        // Script de elevar objetos no pesados del personaje
        private PickUpItem _pickable;
        // Script de salto del personaje
        private Jump _jump;
        private PlayerMagicAttack _magicAttack;

        // Script de acción secundaria
        private SecondaryAction _secondaryAction;
        private AnimatorBrain _animatorBrain;

        // VARIABLES
        // Masks
        [SerializeField] private LayerMask _interactableLayerMask;
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

        // Secondary action input
        private bool _isSecondaryInput;

        #endregion

        #region Unity methods

        private void Awake()
        {
            // Obtenemos componentes
            _movement = GetComponent<PlayerMovement>();
            _jump = GetComponent<Jump>();
            _interaction = new Interaction( transform , GetComponent<Collider2D>().offset , _interactableLayerMask );
            _pickable = GetComponent<PickUpItem>();
            _magicAttack = GetComponent<PlayerMagicAttack>();
            _secondaryAction = GetComponent<LightAttack>();
            _animatorBrain = GetComponentInChildren<AnimatorBrain>();
        }

        private void Start()
        {
            _movement.Init();
            _jump.Init( _animatorBrain , GetComponent<Collider2D>().offset , _interactableLayerMask );
            _animatorBrain.Init();

            _gameInputs = ServiceLocator.GetService<GameInputs>();
            _gameInputs.OnJumpButtonStarted += GameInputs_OnJumpButtonStarted;
            _gameInputs.OnJumpButtonCanceled += GameInputs_OnJumpButtonCanceled;
            _gameInputs.OnPhysicActionButtonPerformed += GameInputs_OnPhysicActionButtonPerformed;
            _gameInputs.OnMediumAttackButtonStarted += GameInputs_OnMediumAttackButtonStarted;
            _gameInputs.OnMediumAttackButtonCanceled += GameInputs_OnMediumAttackButtonCanceled;
            _gameInputs.OnWeakAttackButtonStarted += GameInputs_OnWeakAttackButtonStarted;
            _gameInputs.OnWeakAttackButtonCanceled += GameInputs_OnWeakAttackButtonCanceled;
            _gameInputs.OnStrongAttackPerformed += GameInputs_OnStrongAttackButtonPerformed;
            _gameInputs.OnSecondaryPerformed += GameInputs_OnSecondaryButtonPerformed;
        }

        private void OnDestroy()
        {
            _gameInputs.OnJumpButtonStarted -= GameInputs_OnJumpButtonStarted;
            _gameInputs.OnJumpButtonCanceled -= GameInputs_OnJumpButtonCanceled;
            _gameInputs.OnPhysicActionButtonPerformed -= GameInputs_OnPhysicActionButtonPerformed;
            _gameInputs.OnMediumAttackButtonStarted -= GameInputs_OnMediumAttackButtonStarted;
            _gameInputs.OnMediumAttackButtonCanceled -= GameInputs_OnMediumAttackButtonCanceled;
            _gameInputs.OnWeakAttackButtonStarted -= GameInputs_OnWeakAttackButtonStarted;
            _gameInputs.OnWeakAttackButtonCanceled -= GameInputs_OnWeakAttackButtonCanceled;
            _gameInputs.OnStrongAttackPerformed -= GameInputs_OnStrongAttackButtonPerformed;
            _gameInputs.OnSecondaryPerformed -= GameInputs_OnSecondaryButtonPerformed;
        }

        private void Update()
        {
            // Controlamos las acciones
            GetActionsInformation();

            // Realizamos salto
            DoJump();
            // Realizamos elevar objeto no pesado
            DoPickUpItem();
            // Realizamos interacción
            DoInteraction();
            // Atacamos con magia
            DoMagicAttack();

            // Realizamos la acción secundaria
            // (hechizos menores / uso de consumibles)
            DoSecondaryAction();

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


            if (_jump.IsPerformingJump || _mediumMagicUsed)
                return;

            _lookDirection = _animatorBrain.LookDirection(_direction);
        }

        private void DoMove()
        {
            if (_jump.IsOnAir)
                _movement.MoveOnAir(_direction);
            else
            if (_jump.IsCooldown)
                _movement.Stop();
            else
            if (!_jump.IsPerformingJump)
                _movement.Move(_direction);

            else
                _movement.Stop();
        }

        #region Jump
        private void GameInputs_OnJumpButtonCanceled() => _isJumpInput = false;
        private void GameInputs_OnJumpButtonStarted()
        {
            if (CanJump())
                _isJumpInput = true;
        }

        private void DoJump()
        {
            //if (IsAttacking())
            //    return;

            _jump.JumpAction(_isJumpInput, _lookDirection, _direction);
            if (!_jump.IsPerformingJump)
                _isJumpInput = false;
        }

        #endregion

        #region Interact

        private void GameInputs_OnPhysicActionButtonPerformed() => _isPhysicActionInput = true;


        private void DoInteraction()
        {
            if (_jump.IsPerformingJump || IsAttacking() || _pickable.HasItem)
                return;

            _interaction.Interact(_isPhysicActionInput, _lookDirection);

            _isPhysicActionInput = false;
        }

        #endregion

        #region Pick item

        private void DoPickUpItem()
        {
            if (_jump.IsPerformingJump || IsAttacking())
                return;

            if (!_pickable.HasItem)
            {
                if (_pickable.CanPickItUp(_lookDirection))
                {
                    if (_isPhysicActionInput)
                    {
                        _pickable.PickItUp(_lookDirection);
                        _isPhysicActionInput = false;
                    }
                }
            }
            else
            {
                if (_isPhysicActionInput)
                {
                    _isPhysicActionInput = false;
                    _pickable.ThrowIt(_lookDirection);
                }
            }
        }
        #endregion

        #region Secondary Action

        private void GameInputs_OnSecondaryButtonPerformed()
        {
            if (!_jump.IsPerformingJump
                && !IsAttacking())
                _isSecondaryInput = true;
        }

        private void DoSecondaryAction()
        {
            if (!_isSecondaryInput)
                return;

            // Activamos el efecto de la acción secundaria
            _secondaryAction.SetDirection(_lookDirection);
            _secondaryAction.Effect();
            _isSecondaryInput = false;
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
                || _mediumMagicUsed || _isMediumMagicInput
                || _isStrongMagicInput;
        }

        #region Physic Attack


        #endregion

        #region Magic attack

        private void GameInputs_OnWeakAttackButtonStarted()
        {
            if (_magicAttack.CanAttack()
                && !_jump.IsPerformingJump
                && !IsAttacking()
                )
                _isWeakMagicInput = true;
        }
        private void GameInputs_OnWeakAttackButtonCanceled() => _isWeakMagicInput = false;
        private void GameInputs_OnMediumAttackButtonStarted()
        {
            if (_magicAttack.CanAttack()
                && !_jump.IsPerformingJump
                && !IsAttacking()
                )
                _isMediumMagicInput = true;
        }

        private void GameInputs_OnMediumAttackButtonCanceled()
        {
            _isMediumMagicInput = false;

            if (_mediumMagicUsed)
            {
                _mediumMagicUsed = false;
                _magicAttack.StopMediumAttack();
                _magicAttack.ResetTimer();
            }
        }

        private void GameInputs_OnStrongAttackButtonPerformed()
        {
            if (_magicAttack.CanAttack()
                && _magicAttack.CanUseMaxAttack()
                && !_jump.IsPerformingJump
                && !IsAttacking()
                )
                _isStrongMagicInput = true;
        }

        /// <summary>
        /// Gestiona los ataques mágicos
        /// </summary>
        private void DoMagicAttack()
        {
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

            // Reseteamos variables
            _isWeakMagicInput = false;
            _magicAttack.ResetTimer();
            // Y activamos la magia débil
            _magicAttack.WeakAttack(_lookDirection);
        }

        /// <summary>
        /// Gestiona los ataques medios
        /// </summary>
        private void DoMediumMagicAttack()
        {
            if (!_isMediumMagicInput || _mediumMagicUsed)
                return;

            // Activamos las variables de magia media e invocamos el nuevo ataque
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

            // Activamos la magia fuerte
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
            if (_jump.IsPerformingJump)
                return;
            _animatorBrain.IsWalking(_direction.magnitude > 0);
        }
    }
}