using Attack;
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
        private PlayerStatus _playerStatus;
        private PlayerMovement _movement;
        private Interaction _interaction;
        // Script de elevar objetos no pesados del personaje
        private PickUpItem _pickable;
        // Script de salto del personaje
        private Jump _jump;
        //private PlayerMagicAttack _magicAttack;

        // Script de acción secundaria
        private SecondaryAction _secondaryAction;
        private AnimatorBrain _animatorBrain;

        // VARIABLES
        // Masks
        [SerializeField] private LayerMask _interactableLayerMask;
        // Inputs
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

        // Secondary action input
        private bool _isSecondaryInput;

        // Lists
        // MagicAttack (primary skill) list
        private List<MagicAttack> _magicAttacks;
        private int _magicIndex => _playerStatus.PrimarySkillIndex;

        // Secondary action (or skill) list
        private List<SecondaryAction> _secondaryActions;
        private int _secondaryIndex => _playerStatus.SecondarySkillIndex;

        #endregion

        #region Unity methods

        private void Awake()
        {
            // Obtenemos componentes
            _movement = new PlayerMovement(GetComponent<Rigidbody2D>());
            _jump = GetComponent<Jump>();
            _interaction = new Interaction(transform, GetComponent<Collider2D>().offset, _interactableLayerMask);
            _pickable = GetComponent<PickUpItem>();
            //_magicAttack = GetComponent<PlayerMagicAttack>();
            _secondaryAction = GetComponent<LightAttack>();
            _animatorBrain = GetComponentInChildren<AnimatorBrain>();
            _playerStatus = GetComponent<PlayerStatus>();

            // Inicializamos variables
            _magicAttacks = new List<MagicAttack>();
            AddMagicAttacks();

            _secondaryActions = new List<SecondaryAction>();

        }

#if UNITY_EDITOR
        private bool _isInitialized;
        private void Start()
        {
            if (!_isInitialized)
                Init(Vector2.down, 1 << 16);
        }
#endif

        public void Init(Vector2 startLookDirection, LayerMask initialGroundLayerMask)
        {
#if UNITY_EDITOR
            _isInitialized = true;
#endif
            _jump.Init(_animatorBrain, GetComponent<Collider2D>().offset, _interactableLayerMask, initialGroundLayerMask);
            _animatorBrain.Init(startLookDirection);
            _magicAttacks[_magicIndex].Select();

            _gameInputs = ServiceLocator.GetService<GameInputs>();
            _gameInputs.OnJumpButtonStarted += GameInputs_OnJumpButtonStarted;
            _gameInputs.OnJumpButtonCanceled += GameInputs_OnJumpButtonCanceled;
            _gameInputs.OnPhysicActionButtonStarted += GameInputs_OnPhysicActionButtonStarted;
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
            _gameInputs.OnPhysicActionButtonStarted -= GameInputs_OnPhysicActionButtonStarted;
            _gameInputs.OnMediumAttackButtonStarted -= GameInputs_OnMediumAttackButtonStarted;
            _gameInputs.OnMediumAttackButtonCanceled -= GameInputs_OnMediumAttackButtonCanceled;
            _gameInputs.OnWeakAttackButtonStarted -= GameInputs_OnWeakAttackButtonStarted;
            _gameInputs.OnWeakAttackButtonCanceled -= GameInputs_OnWeakAttackButtonCanceled;
            _gameInputs.OnStrongAttackPerformed -= GameInputs_OnStrongAttackButtonPerformed;
            _gameInputs.OnSecondaryPerformed -= GameInputs_OnSecondaryButtonPerformed;
        }

        private void Update()
        {
            // TODO: GameOver
            // Si el jugador ha perdido toda su salud,
            // si está aturdido
            // o si está usando el poder máximo, volvemos
            if (_playerStatus.IsDeath ||
                _playerStatus.IsStunned ||
                _magicAttacks[_magicIndex]._isUsingStrongAttack)
            {
                if (_magicAttacks[_magicIndex]._isUsingMediumAttack)
                    GameInputs_OnMediumAttackButtonCanceled();

                return;
            }


            // Controlamos las acciones
            GetActionsInformation();

            // Realizamos salto
            DoJump();

            // Realizamos las interacciones físicas
            DoPhysicalAction();

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
            // TODO: GameOver
            // Si el jugador ha perdido toda su salud,
            // si está aturdido
            // o si está usando el poder máximo, volvemos
            if (_playerStatus.IsDeath ||
                _playerStatus.IsStunned ||
                _magicAttacks[_magicIndex]._isUsingStrongAttack)
                return;

            // Nos movemos
            DoMove();
        }

        #endregion

        private void AddMagicAttacks()
        {
            _magicAttacks.Add(GetComponent<FireAttack>());
        }

        private void GetActionsInformation()
        {
            // Obtenemos la dirección
            _direction = _gameInputs.GetDirectionNormalized();


            if (_jump.IsPerformingJump ||
                _magicAttacks[_magicIndex].IsUsingMediumAttack
                )
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
            if (_playerStatus.IsJumpUnlocked)
                if (CanJump())
                    _isJumpInput = true;
        }

        private void DoJump()
        {
            if (IsAttacking() || _pickable.HasItem || _interaction.IsInteracting)
                return;

            _jump.JumpAction(_isJumpInput, _lookDirection, _direction);
            if (!_jump.IsPerformingJump)
                _isJumpInput = false;
        }

        #endregion

        #region Physical Actions

        private void GameInputs_OnPhysicActionButtonStarted() => _isPhysicActionInput = true;

        private void DoPhysicalAction()
        {
            if (!_jump.IsPerformingJump || !IsAttacking())
            {
                DoInteraction();
                DoPickUpItem();
            }
            _isPhysicActionInput = false;
        }

        private void DoInteraction()
        {
            if (_pickable.HasItem)
                return;

            _interaction.Interact(_isPhysicActionInput, _lookDirection);
        }

        private void DoPickUpItem()
        {
            if (_interaction.IsInteracting)
                return;

            if (!_pickable.HasItem)
            {
                if (_pickable.CanPickItUp(_lookDirection))
                {
                    if (_isPhysicActionInput)
                    {
                        _pickable.PickItUp(_lookDirection);
                    }
                }
            }
            else
            {
                if (_isPhysicActionInput)
                {
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
            return _isPhysicAttacking ||
                _magicAttacks[_magicIndex].IsUsingWeakAttack ||
                _magicAttacks[_magicIndex].IsUsingMediumAttack ||
                _magicAttacks[_magicIndex].IsUsingStrongAttack
                ;


            //return _isPhysicAttacking || _isWeakMagicInput
            //    || _mediumMagicUsed || _isMediumMagicInput
            //    || _isStrongMagicInput;
        }


        #region Magic attack

        private void GameInputs_OnWeakAttackButtonStarted()
        {
            if (_playerStatus.CanUseMagicAttacks()
                && !_jump.IsPerformingJump
                && !IsAttacking()
                )
                _isWeakMagicInput = true;
        }
        private void GameInputs_OnWeakAttackButtonCanceled() => _isWeakMagicInput = false;
        private void GameInputs_OnMediumAttackButtonStarted()
        {
            if (_playerStatus.CanUseMagicAttacks()
                && !_jump.IsPerformingJump
                && !IsAttacking()
                )
                _isMediumMagicInput = true;
        }

        private void GameInputs_OnMediumAttackButtonCanceled()
        {
            _isMediumMagicInput = false;

            // Si está usando el poder medio
            if (_magicAttacks[_magicIndex].IsUsingMediumAttack)
                // Lo detenemos
                _magicAttacks[_magicIndex].StopMediumAttack();

        }

        private void GameInputs_OnStrongAttackButtonPerformed()
        {
            if (_playerStatus.CanUseMagicAttacks()
                && _playerStatus.CanUseMaxPower()
                //&& _magicAttack.CanUseMaxAttack()
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
            if (_playerStatus.IsWeakMagicUnlocked)
                DoWeakMagicAttack();
            if (_playerStatus.IsMediumMagicUnlocked)
                DoMediumMagicAttack();
            if (_playerStatus.IsStrongMagicUnlocked)
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
            // Reseteamos el contador
            _playerStatus.RestartMagicTimer();
            // Y activamos la magia débil
            _magicAttacks[_magicIndex].WeakAttack(_lookDirection);
        }

        /// <summary>
        /// Gestiona los ataques medios
        /// </summary>
        private void DoMediumMagicAttack()
        {
            if (!_isMediumMagicInput ||
                _magicAttacks[_magicIndex].IsUsingMediumAttack)
                return;

            // Activamos las variables de magia media e invocamos el nuevo ataque
            _magicAttacks[_magicIndex].MediumAttack(_lookDirection);

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
            // Reseteamos el contador
            _playerStatus.RestartMagicTimer();
            _magicAttacks[_magicIndex].StrongAttack(_lookDirection);
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