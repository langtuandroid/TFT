using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Events;
using Utils;

public class Switch : MonoBehaviour
{
    #region Variables
    
    #region Inspector
    [SerializeField] private float _sightAware;
    [SerializeField] private float _sightAwareInterior;
    [Header("Events on Activate Objects")]
    public UnityEvent OnSwitchActivation;

    #endregion
    
    #region Private Variable
    private bool _buttonCollision = false;
    private bool _isActivated = false;
    #endregion
    
    #region References
    private Animator _animator;
    private PlayerController _playerController;
    private Collider2D _colliderExterior;
    private Collider2D _colliderInterior;
    #endregion

    public bool CanReset = false;
    public bool ResetPuzle = false;
    
    #endregion

    #region Unity Methods
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _colliderExterior = GetComponent<Collider2D>();
    }

    private void Update()
    {
        ActivateButton();

        if (ResetPuzle)
        {
            CheckPlayerExit();
        }
    }
    #endregion
    
    private void ActivateButton()
    {
        if (CheckPlayer())
        {
            if (_playerController != null)
            {
                if (_playerController.IsJumpInput)
                {
                    _colliderExterior.enabled = false;
                }
                else
                {
                    CheckOnStayPlayer();
                }
            }
        }
        else
        {
            //Activar para volver a pulsar el boton
            //ResetButton();
        }
    }

    public void ResetButton()
    {
        ResetPuzle = false;
        _colliderExterior.enabled = true; //Activo el colider exterior
        _buttonCollision = false; //Reseteo la variable de activar boton para que solo entre 1 vez en el metodo     
        _isActivated = false; // Desactivo el boton
        _animator.Play("Button_Off");
    }

    private void ToggleSwitch()
    {
        if (!_buttonCollision)
        {
            _buttonCollision = true;
            
            _isActivated = !_isActivated;
            
            if(_isActivated)
                _animator.Play("Button_On");
            else
                _animator.Play("Button_Off");

            OnSwitchActivation?.Invoke();
        }
    }
    
    private bool CheckPlayer()
    {
        Collider2D objectDetected = Physics2D.OverlapCircle(transform.position, _sightAware, 
            LayerMask.GetMask(Constants.LAYER_INTERACTABLE, Constants.LAYER_PLAYER));

        if (objectDetected != null)
        {
            if (objectDetected.CompareTag(Constants.TAG_PLAYER))
            { 
                if(_playerController == null) 
                    _playerController = objectDetected.GetComponent<PlayerController>();
                return true;
            }
        }

        return false;
    }
    
    private void CheckOnStayPlayer()
    {
        Collider2D objectDetected = Physics2D.OverlapCircle(transform.position, _sightAwareInterior, 
            LayerMask.GetMask(Constants.LAYER_INTERACTABLE, Constants.LAYER_PLAYER));

        if (objectDetected != null)
        {
            if (objectDetected.CompareTag(Constants.TAG_PLAYER))
            {
                ToggleSwitch();
                return;
            }
        }
      
        _colliderExterior.enabled = true;
    }

    private void CheckPlayerExit()
    {
        Collider2D objectDetected = Physics2D.OverlapCircle(transform.position, _sightAwareInterior, 
            LayerMask.GetMask(Constants.LAYER_INTERACTABLE, Constants.LAYER_PLAYER));
        
        Debug.Log("Objetos detectados: " + objectDetected.gameObject.name);

        if (objectDetected != null)
        {
            if (!objectDetected.CompareTag(Constants.TAG_PLAYER))
            {
                ResetButton();
            }
        }
  
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _sightAware);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _sightAwareInterior);
    }
}