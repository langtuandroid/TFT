using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using TMPro;
using System;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    #region SerializeFields

    [Header("Move Settings")]
    [SerializeField] private float _speed; // Velocidad de movimiento del personaje

    [Header("Jump")]
    private Jump _jump;

    [Header("Interactable")]
    [SerializeField] private LayerMask _interactableLayer;

    //[Header("Attack")]
    //private PlayerAttackExtra _attack;

    #endregion

    #region Const Variables
    private const string WALKING = "IsWalking";

    public enum AnimationLayers
    {
        // Animaciones de caminar
        WalkDown,
        WalkHorizontal,
        WalkUp,
        // Animaciones de salto
        JumpDown,
        JumpHorizontal,
        JumpUp,
        // Animaciones de ataque
        // Animación nula
        Null
    }
    #endregion

    #region Public Variables
    public AnimationLayers Layer => _layer; // Da la capa de animación en la que estamos
    //public bool IsJumping => _jump.IsJumping; // Devuelve si está saltando o no
    public bool HorizontalFlip => _spriteRend.flipX; // Devuelve si está volteado el sprite o no
    #endregion

    #region Private Variables
    // SERVICIOS
    private GameInputs _gameInputs;

    // COMPONENTES DEL GAMEOBJECT
    private Rigidbody2D _rb; // RigidBody del personaje
    private Animator _anim; // Animator del personaje
    private SpriteRenderer _spriteRend; // SpriteRenderer del personaje

    // MOVIMIENTO
    private Vector2 _direction; // Direcci�n de movimiento del personaje
    private Vector2 _lookDirection;

    // ANIMATOR
    private AnimationLayers _layer; // Layer en ese momento
    //private AnimationLayers _jumpLayer; // Layer para el salto

    private bool _isJumping = false;
    private bool _isInteracting = false;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        //Hacemos Singleton a la clase
        if ( Instance == null )
        {
            Instance = this;
            DontDestroyOnLoad( gameObject );
        }
        else
            Destroy( gameObject );

        // Inicializamos variables
        _rb = GetComponent<Rigidbody2D>();
        _jump = GetComponent<Jump>();
        _anim = GetComponentInChildren<Animator>();
        _spriteRend = GetComponentInChildren<SpriteRenderer>();
        //_attack = GetComponent<PlayerAttackExtra>();

        // Establecemos como layer inicial el primero (Walkdown)
        _layer = AnimationLayers.WalkDown;
        // Y el layer para el salto a null
        //_jumpLayer = AnimationLayers.Null;
    }

    private void Start()
    {
        _gameInputs = ServiceLocator.GetService<GameInputs>();
        _gameInputs.OnSouthButtonStarted  += GameInputs_OnSouthButtonStarted;
        _gameInputs.OnSouthButtonCanceled += GameInputs_OnSouthButtonCanceled;
        _gameInputs.OnEastButtonPerformed += GameInputs_OnEastButtonPerformed;
    }

    private void OnDestroy()
    {
        _gameInputs.OnSouthButtonStarted  -= GameInputs_OnSouthButtonStarted;
        _gameInputs.OnSouthButtonCanceled -= GameInputs_OnSouthButtonCanceled;
        _gameInputs.OnEastButtonPerformed -= GameInputs_OnEastButtonPerformed;
    }

    private void Update()
    {
        //if (MyGameManager.Instance.Stoped)
        //    return;

        // Obtenemos el vector de direcci�n
        GetDirection();

        // INTERACT
        Debug.DrawRay( transform.position , _lookDirection );
        if ( _isInteracting )
        {
            float comprovationDistance = 1;
            RaycastHit2D hit = Physics2D.Raycast( transform.position ,
                _lookDirection , 
                comprovationDistance , _interactableLayer );

            if ( hit )
            {
                hit.collider.GetComponent<IInteractable>()?.Interact();
            }

            _isInteracting = false;
        }

        // JUMP
        if ( _isJumping )
        {
            _jump.JumpAction();
            if ( !_jump.IsJumping )
                _isJumping = false;
        }
        else
            _jump.Fall();
    }

    private void FixedUpdate()
    {
        //if (MyGameManager.Instance.Stoped)
        //    return;

        // Nos movemos con el RigidBody
        Move();

        if (_anim != null)
            // Cambiamos sprites de movimiento
            ChangeAnimation();
    }

    #endregion


    #region Private Methods

    private void GameInputs_OnEastButtonPerformed()
    {
        _isInteracting = true;
    }

    private void GameInputs_OnSouthButtonCanceled() => _isJumping = false;
    private void GameInputs_OnSouthButtonStarted()
    {
        if ( _jump.CanJump )
            _isJumping = true;
    }


    /// <summary>
    /// Obtiene el vector de direcci�n final (normalizado para que se mueva en la misma direcci�n en todos los ejes)
    /// </summary>
    private void GetDirection()
    {
        // Obtenemos el vector de direcci�n
        _direction = _gameInputs.GetDirectionNormalized();

        if ( _direction.magnitude > 0.05f )
            _lookDirection = _direction;
    }

    /// <summary>
    /// Mueve el GameObject del personaje
    /// </summary>
    private void Move()
    {
        // Y movemos el RigidBody
        _rb.MovePosition(_rb.position + Time.deltaTime * _speed * _direction);
    }

    #region Animation Methods

    private void ChangeAnimation()
    {

        // Cogemos el estado de caminar
        bool isWalking = _direction.magnitude > 0;
        // Y lo cambiamos
        SetWalkingState(isWalking);

        if (isWalking)
        {
            _anim.SetFloat("x", _direction.x);
            _anim.SetFloat("y", _direction.y);
        }


        //// Si está saltando
        //if (_jump.IsJumping)
        //    // Cambiamos la animación de saltar
        //    ChangeJumpAnimation();
        //// En caso de que no esté saltadno
        //// y continúe con la animación de salto
        //else if (!_jump.IsJumping &&
        //    _jumpLayer != AnimationLayers.Null)
        //    ChangeJumpLayer();
        //// Si está caminando
        //else if (isWalking)
        //    // Cambiamos la animación de andar
        //ChangeWalkAnimation();


    }

    ///// <summary>
    ///// Cambia la animación de salto por la de caminar
    ///// y viceversa
    ///// </summary>
    //private void ChangeJumpAnimation()
    //{
    //    // Si nos estamos moviendo horizontalmente
    //    if (_horizontal != 0f)
    //        // Cambiamos el flip del sprite en caso de que movimiento horizontal
    //        // sea negativo
    //        _spriteRend.flipX = _horizontal < 0f;

    //    // Y cambiamos el layer a saltar
    //    ChangeJumpLayer();
    //}


    //private void ChangeWalkAnimation()
    //{
    //    // Cambiamos el flip del sprite en caso de que movimiento horizontal
    //    // sea negativo
    //    _spriteRend.flipX = _horizontal < 0f;



    //}

    /// <summary>
    /// Cambia el estado de la variable de movimiento
    /// </summary>
    /// <param name="isWalking"></param>
    private void SetWalkingState(bool isWalking)
    {
        // Cambiamos el estado de movimiento
        _anim.SetBool(WALKING, isWalking);
    }

    /// <summary>
    /// Cambia el layer actual y modifica los pesos en el animator
    /// </summary>
    /// <param name="layer"></param>
    private void ChangeAnimLayer(AnimationLayers layer)
    {
        // Primero, quitamos el peso en la actual layer
        _anim.SetLayerWeight((int)_layer, 0f);
        // Cambiamos de layer
        _layer = layer;
        // Y le ponemos el peso a esta nueva layer
        _anim.SetLayerWeight((int)layer, 1f);
    }

    ///// <summary>
    ///// Cambia el estado de la layer de salto
    ///// </summary>
    ///// <param name="isJumping"></param>
    //private void ChangeJumpLayer()
    //{
    //    // Si está saltando y todavía no hay animación de salto asignada
    //    if (_jumpLayer == AnimationLayers.Null)
    //    {

    //        // Asignamos el layer correspondiente al salto
    //        switch (_layer)
    //        {
    //            case AnimationLayers.WalkDown:
    //                _jumpLayer = AnimationLayers.JumpDown;
    //                break;
    //            case AnimationLayers.WalkHorizontal:
    //                _jumpLayer = AnimationLayers.JumpHorizontal;
    //                break;
    //            case AnimationLayers.WalkUp:
    //                _jumpLayer = AnimationLayers.JumpUp;
    //                break;
    //        }

    //        // Quitamos el peso a la layer de caminar
    //        _anim.SetLayerWeight((int)_layer, 0f);
    //        // Y se lo damos a la layer de salto
    //        _anim.SetLayerWeight((int)_jumpLayer, 1f);
    //    }
    //    // Si no estamos saltando
    //    else if (!_jump.IsJumping)
    //    {
    //        // Quitamos el peso a la layer de salto que teníamos asignada
    //        _anim.SetLayerWeight((int)_jumpLayer, 0f);
    //        // Y ponemos el peso a nuestra layer de caminar
    //        _anim.SetLayerWeight((int)_layer, 1f);
    //        // Asignamos la capa nula al layer del salto
    //        _jumpLayer = AnimationLayers.Null;
    //    }
    //}

    #endregion

    #endregion

    #region Public Methods

    //Método que cambia la ubicación del personaje
    public void ChangeWorldPosition(Transform _destinyTransform)
    {
        transform.position =
            new Vector3(_destinyTransform.position.x, _destinyTransform.position.y, _destinyTransform.position.z);
    }

    //Devuelve la dirección del personaje
    public Vector2 GetActualDirection()
    {
        return _direction;
    }
    #endregion

}
