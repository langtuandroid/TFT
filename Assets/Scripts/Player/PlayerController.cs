using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {

        #region Private variables

        // SCRIPTS DEL JUGADOR
        // Script de movimiento del personaje
        private PlayerMovement _movement;
        // Script de interacción del personaje
        private Interaction _interact;
        // Script de salto del personaje
        private Jump _jump;


        // COMPONENTES
        // Animator del player
        private Animator _anim;

        // VARIABLES
        private float _lastX;
        private float _lastY;

        #endregion

        #region Unity methods

        private void Awake()
        {
            // Obtenemos componentes
            _movement = GetComponent<PlayerMovement>();
            _interact = GetComponent<Interaction>();
            _jump = GetComponent<Jump>();

            _anim = GetComponentInChildren<Animator>();

            // Inicializamos variables
            _lastX = 0f;
            _lastY = 0f;

        }


        private void Update()
        {
            // Cambiamos la animación según corresponda
            SetAnimations();
        }

        #endregion

        #region Private methods

        private void SetAnimations()
        {
            // Controlamos los saltos
            _anim.SetBool(Constants.ANIM_PLAYER_JUMP, _jump.IsJumping);
            // Si está saltando
            if (_jump.IsJumping)
                // Volvemos
                return;

            // Controlamos el movimiento
            ControlWalking2();
        }

        private void ControlWalking()
        {
            bool isWalking = _movement.Direction.magnitude > 0f;
            _anim.SetBool(Constants.ANIM_PLAYER_WALKING, isWalking);

            if (isWalking)
            {
                float x = _movement.Direction.x;
                float y = _movement.Direction.y;


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
            bool isWalking = _movement.Direction.magnitude > 0f;
            _anim.SetBool(Constants.ANIM_PLAYER_WALKING, isWalking);

            if (isWalking)
            {
                float x = _movement.Direction.x;
                float y = _movement.Direction.y;

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
    }
}