using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TorchLightMovement : MonoBehaviour
{
    #region SerializeFields

    [SerializeField]
    [Tooltip("Cantidad que se desplaza en el eje X")]
    private float _translation;

    #endregion

    #region Private variables

    // COMPONENTS
    private Animator _anim;

    // VARIABLES
    private Vector3 _initialPos; // Posición inicial
    private float _duration; // Duración de movimiento
    private Tween _tween; // Tween (para hacer kill)

    #endregion

    #region Unity methods

    private void Awake()
    {
        // Components
        _anim = GetComponentInParent<Animator>();

        // Variables
        _initialPos = transform.localPosition;
        AnimationClip[] clips = _anim.runtimeAnimatorController.animationClips;
        _duration = clips[0].length / 2;
    }

    private void OnEnable()
    {
        _tween = MoveLight(-_translation);
    }

    private void OnDisable()
    {
        _tween.Kill();
        transform.localPosition = _initialPos;

    }
    #endregion

    #region Private methods

    /// <summary>
    /// Mueve la luz hacia la posición en X dada
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private Tween MoveLight(float dir)
    {
        Debug.Log(dir);
        _tween = transform.DOMoveX(dir, _duration).SetEase(Ease.Linear).
            OnComplete(() => MoveLight(-dir)).SetRelative().Play();

        return _tween;
    }

    #endregion
}
