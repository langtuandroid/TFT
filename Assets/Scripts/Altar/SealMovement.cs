using UnityEngine;
using DG.Tweening;

public class SealMovement : MonoBehaviour
{
    [SerializeField] private float _duration = 3f;

    private void Awake()
    {
        float pixel = 1f / 16;
        transform.DOMoveY( pixel * _duration , 1 ).SetLoops( -1, LoopType.Yoyo ).Play();
    }
}
