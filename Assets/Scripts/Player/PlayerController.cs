using System.Collections;
using Attack;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Private variables
        // SERVICES
        private GameInputs _gameInputs;
        private GameStatus _gameStatus;
        private IAudioSpeaker _audioSpeaker;

        // EVENTS
        private InventoryEvents _inventoryEvents;
        private MagicEvents _magicEvents;
        private LifeEvents _lifeEvents;
        private SoulEvents _soulEvents;

        // PLAYER'S SCRIPTS
        private PlayerStatus _playerStatus;
        private PlayerMovement _movement;
        private Interaction _interaction;
        private FallController _fallController;

        private PhisicalAttack _phisicalAttack;
        // Script de elevar objetos no pesados del personaje
        private PickUpItem _pickable;

        public PickUpItem Pickable
        {
            get => _pickable;
        }
        // Script de salto del personaje
        private Jump _jump;

        // Script de acción secundaria
        private SecondaryAction _secondaryAction;
        private AnimatorBrain _animatorBrain;

        // DATA
        [SerializeField] private PlayerPhysicalDataSO _physicalDataSO;
        [SerializeField]
        private PlayerStatusSaveSO _statusSaveSO;
        [SerializeField]
        private PlayerStatusSettingDataSO _statusSettingDataSO;

        // Inputs
        // Jump input
        private bool _isJumpInput;

        public bool IsJumpInput { get => _isJumpInput; }

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
            // COMPONENTS
            _animatorBrain = GetComponentInChildren<AnimatorBrain>();
            _secondaryAction = GetComponent<LightAttack>();
            _phisicalAttack = GetComponentInChildren<PhisicalAttack>();

            Collider2D collider = GetComponent<Collider2D>();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Transform characterVisualTrans = transform.Find(_physicalDataSO.visualObjName);
            Transform pickUpPointTrans = characterVisualTrans.transform.Find(_physicalDataSO.pickUpPointObjName);

            // SCRIPTS
            _playerStatus = new PlayerStatus(_statusSaveSO, _statusSettingDataSO);
            _movement = new PlayerMovement(rb, _physicalDataSO);
            _jump = new Jump(collider.offset, transform, characterVisualTrans, _physicalDataSO);
            _interaction = new Interaction(transform, collider.offset, _physicalDataSO);
            _pickable = new PickUpItem();
            _fallController = new FallController(rb, collider, _animatorBrain, _physicalDataSO);

            // Inicializamos variables
            // Magic Attacks
            //AddMagicAttacks();
            // SecondaryActions
            AddSecondaryACtions();

            // Pickable
            _pickable.Init(transform, pickUpPointTrans, collider.offset,
                _physicalDataSO.interactableLayerMask, _animatorBrain);
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
            // SERVICE -> GAMESTATUS
            _gameStatus = ServiceLocator.GetService<GameStatus>();

            // SERVICE -> AUDIOSPEAKER
            _audioSpeaker = ServiceLocator.GetService<IAudioSpeaker>();
            _jump.Init(_animatorBrain, _audioSpeaker, initialGroundLayerMask);
            _animatorBrain.Init(startLookDirection, _jump);
            _fallController.Init(transform.position, _audioSpeaker, _gameStatus);

            // SERVICE -> GAMEINPUTS
            _gameInputs = ServiceLocator.GetService<GameInputs>();

            // GAMEINPUTS -> JUMP
            _gameInputs.OnJumpButtonStarted += GameInputs_OnJumpButtonStarted;
            _gameInputs.OnJumpButtonCanceled += GameInputs_OnJumpButtonCanceled;

            // GAMEINPUTS -> PHYSIC ATTACK
            _gameInputs.OnPhysicActionButtonStarted += GameInputs_OnPhysicActionButtonStarted;

            // GAMEINPUTS -> MAGIC ATTACKS (PRIMARY ACTION)
            _gameInputs.OnWeakAttackButtonStarted += GameInputs_OnWeakAttackButtonStarted;
            _gameInputs.OnWeakAttackButtonCanceled += GameInputs_OnWeakAttackButtonCanceled;
            _gameInputs.OnMediumAttackButtonStarted += GameInputs_OnMediumAttackButtonStarted;
            _gameInputs.OnMediumAttackButtonCanceled += GameInputs_OnMediumAttackButtonCanceled;
            _gameInputs.OnStrongAttackPerformed += GameInputs_OnStrongAttackButtonPerformed;

            // GAMEINPUTS -> SECONDARY ACTION
            _gameInputs.OnSecondaryStarted += GameInputs_OnSecondaryButtonStarted;
            _gameInputs.OnSecondaryCanceled += GameInputs_OnSecondaryButtonCanceled;

            // EVENT -> INVENTORY
            _inventoryEvents = ServiceLocator.GetService<InventoryEvents>();
            _inventoryEvents.OnPrimarySkillChange += OnChangePrimarySkill;
            _inventoryEvents.OnSecondarySkillChange += OnChangeSecondarySkill;

            // EVENT -> LIFE
            _lifeEvents = ServiceLocator.GetService<LifeEvents>();
            _lifeEvents.OnFallDown += _fallController.StartRecovering;

            // EVENT -> MAGIC
            _magicEvents = ServiceLocator.GetService<MagicEvents>();

            // EVENT -> SOULS
            _soulEvents = ServiceLocator.GetService<SoulEvents>();

            _playerStatus.Init(
                lifeEvents: _lifeEvents,
                magicEvents: _magicEvents,
                soulEvents: _soulEvents,
                gameStatus: _gameStatus
                );

            AddMagicAttacks();
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
            _gameInputs.OnSecondaryStarted -= GameInputs_OnSecondaryButtonStarted;

            _inventoryEvents.OnPrimarySkillChange -= OnChangePrimarySkill;
            _inventoryEvents.OnSecondarySkillChange -= OnChangeSecondarySkill;

            _lifeEvents.OnFallDown -= _fallController.StartRecovering;

            _playerStatus.DestroyElements();
        }

        private void Update()
        {
            // TODO: GameOver

            // Actualizamos la información del player
            _playerStatus.UpdateInfo();
            // Si el jugador ha perdido toda su salud,
            // si está aturdido
            // o si está usando el poder máximo, volvemos
            if (_playerStatus.IsDeath)
            {
                if (_magicAttacks[_magicIndex]._isUsingMediumAttack)
                    GameInputs_OnMediumAttackButtonCanceled();

                //return;
            }

            if (_playerStatus.IsStunned &&
                _magicAttacks[_magicIndex].IsUsingMediumAttack)
                GameInputs_OnMediumAttackButtonCanceled();


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
            // si está usando el poder máximo, volvemos
            if (_magicAttacks[_magicIndex]._isUsingStrongAttack)
                return;

            // Nos movemos
            DoMove();
        }

        #endregion

        private void AddMagicAttacks()
        {
            _magicAttacks = new List<MagicAttack>();
            _magicAttacks.Add(GetComponent<FireAttack>());

            PlantAttack plant = GetComponent<PlantAttack>();
            if (plant != null)
                _magicAttacks.Add(plant);

            WaterAttack water = GetComponent<WaterAttack>();
            if (water != null)
                _magicAttacks.Add(GetComponent<WaterAttack>());

            Debug.Log(_magicAttacks.Count);

            foreach (MagicAttack magicAttack in _magicAttacks)
            {
                magicAttack.Init(
                    playerStatus: _playerStatus,
                    magicEvents: _magicEvents,
                    gameStatus: _gameStatus,
                    audioSpeaker: _audioSpeaker
                    );

                magicAttack.enabled = true;
            }
            _magicAttacks[_magicIndex].Select();
        }

        private void AddSecondaryACtions()
        {
            _secondaryActions = new List<SecondaryAction>();
        }

        private void GetActionsInformation()
        {
            // Obtenemos la dirección
            _direction = _gameInputs.GetDirectionNormalized();


            if (_jump.IsPerformingJump ||
                _magicAttacks[_magicIndex].IsUsingMediumAttack ||
                _fallController.HasFalled
                )
                return;

            _lookDirection = _animatorBrain.LookDirection(_direction);
        }

        private void DoMove()
        {
            if (_fallController.IsNotOnScreen)
            {
                _fallController.Move();
                return;
            }
            else
            if (_fallController.HasFalled)
                return;


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
            if (IsAttacking() || _pickable.HasItem || _interaction.IsInteracting
                || _fallController.IsNotOnScreen)
            {
                return;
            }

            _jump.JumpAction(_isJumpInput, _lookDirection, _direction);
            if (!_jump.IsPerformingJump)
                _isJumpInput = false;
        }

        #endregion

        #region Physical Actions

        private void GameInputs_OnPhysicActionButtonStarted() => _isPhysicActionInput = true;

        private void DoPhysicalAction()
        {
            if (!_animatorBrain.HasThrowAnimationEnded()) return;// si no ha terminado la animacion de lanzar no puedo hacer otra accion

            if (!_jump.IsPerformingJump || !IsAttacking())
            {
                DoInteraction();
                DoPickUpItem();
            }

            if (!_interaction.IsInteracting &&
                !_pickable.HasItem &&
                !_jump.IsPerformingJump &&
                !_magicAttacks[_magicIndex].IsUsingWeakAttack &&
                !_magicAttacks[_magicIndex].IsUsingMediumAttack &&
                !_magicAttacks[_magicIndex].IsUsingStrongAttack &&
                _animatorBrain.HasThrowAnimationEnded())
            {
                DoPhysicalAttack();
            }

            if (_isPhysicAttacking && _animatorBrain.HasCurrentAnimationEnded()) _isPhysicAttacking = false; // Reseteo animacion ataque fisico

            _isPhysicActionInput = false;
        }

        private void DoPhysicalAttack()
        {
            if (_isPhysicActionInput)
            {
                _isPhysicAttacking = true;
                _phisicalAttack.Attack(_animatorBrain, _isPhysicActionInput);
            }
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
                    if (_pickable != null)
                        _pickable.ThrowIt(_lookDirection);
                }
            }
        }

        #endregion

        #region Secondary Action

        private void GameInputs_OnSecondaryButtonStarted()
        {
            if (!_jump.IsPerformingJump
                && !IsAttacking())
                _isSecondaryInput = true;
        }

        private void GameInputs_OnSecondaryButtonCanceled()
        {
            _isSecondaryInput = false;
            if (_secondaryAction.IsUsingEffect)
                _secondaryAction.StopEffect();
        }

        private void DoSecondaryAction()
        {
            if (!_isSecondaryInput || !_playerStatus.CanUseSecondarySkill)
                return;

            // Activamos el efecto de la acción secundaria
            //Debug.Log(_lookDirection);
            _isSecondaryInput = false;
            _secondaryAction.SetDirection(_lookDirection);
            _secondaryAction.Effect();
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
        private void GameInputs_OnWeakAttackButtonCanceled()
        {
            _isWeakMagicInput = false;

            // Si está usando el poder débil
            if (_magicAttacks[_magicIndex].IsUsingWeakAttack)
                // Lo detenemos
                _magicAttacks[_magicIndex].StopWeakAttack();
        }
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
            _isMediumMagicInput = false;
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
            _magicAttacks[_magicIndex].StrongAttack(_lookDirection);
        }

        #endregion

        #endregion

        // TODO: Selecci�n de tipo de acci�n
        private void OnChangePrimarySkill(int idx)
        {
            _playerStatus.PrimarySkillIndex = idx;
            _magicAttacks[idx].Select();
        }

        private void OnChangeSecondarySkill(int idx)
        {
            _playerStatus.SecondarySkillIndex = idx;
            // TODO: Que haga bien el select (ahora mismo está en blanco)
            //_secondaryActions[idx].Select();

        }

        private void SetWalkingAnim()
        {
            // Si está saltando
            if (_jump.IsPerformingJump ||
                _magicAttacks[_magicIndex].IsUsingStrongAttack)
            {
                return;
            }

            _animatorBrain.IsWalking(_direction.magnitude > 0);
        }

        public bool IsGrounded => !_jump.IsOnAir;
        public void Fall()
        {
            _jump.FallInHole();
            _movement.Stop();
            _playerStatus.TakeDamage(1);
            _fallController.SetFalling();
        }

        #region Tests

        [ContextMenu("IncrementMaxHealthValue")]
        private void IncrementMaxHealthValue()
        {
            _lifeEvents.AddHeart();
        }

        [ContextMenu("Prueba de take damage")]
        private void TakeDamage()
        {
            //int value = Random.Range(1, 5);
            int value = 1;
            Debug.Log($"Voy a hacer {value} de da�o");
            _playerStatus.TakeDamage(value);
        }

        [ContextMenu("Prueba de heal life")]
        private void HealLife()
        {
            //int value = Random.Range(1, 5);
            int value = 10;
            Debug.Log($"Me curo {value} de salud");
            _playerStatus.HealLife(value);
        }


        #endregion

    }
}