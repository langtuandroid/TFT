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
    private bool _colliderExteriorOut = false;
    private bool _isActivated = false;
    #endregion
    
    #region References
    private Animator _animator;
    private PlayerController _playerController;
    private Collider2D _colliderExterior;
    private Collider2D _colliderInterior;
    #endregion
    
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
                    _colliderExteriorOut = true;
                }
                else
                {
                    if(CheckOnStayPlayer()) ToggleSwitch();
                    else StartCoroutine(nameof(ResetColliderOut));
                }
            }
        }
        else
        {
            //Activar para vovler a pulsar el boton
            //ResetButton();
        }
    }

    private IEnumerator ResetColliderOut()
    {
        yield return new WaitForSeconds(2f);
        
        _colliderExterior.enabled = true;
    }

    private void ResetButton()
    {
        _colliderExterior.enabled = true; //Activo el colider exterior
        _buttonCollision = false; //Reseteo la variable de activar boton para que solo entre 1 vez en el metodo     
        _colliderExteriorOut = false; //Reseteo variable que controla que ya se haya quitado el colider exterior
        _isActivated = false; // Desactivo el boton
    }

    private void ToggleSwitch()
    {
        if (!_colliderExteriorOut) return; //si no he quitado el collider exterior no puedo activar el boton
        
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
    
    private bool CheckOnStayPlayer()
    {
        Collider2D objectDetected = Physics2D.OverlapCircle(transform.position, _sightAwareInterior, 
            LayerMask.GetMask(Constants.LAYER_INTERACTABLE, Constants.LAYER_PLAYER));

        if (objectDetected != null)
        {
            if (objectDetected.CompareTag(Constants.TAG_PLAYER))
            {
                return true;
            }
        }

        return false;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _sightAware);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, _sightAwareInterior);
    }
}