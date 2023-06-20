using UnityEngine;
using DG.Tweening;

public class SealMovement : MonoBehaviour
{
    [SerializeField] private float _duration = 3f;
    [SerializeField] private int _pixelToMove = 3;

    private void Start()
    {
        float pixel = 1f / 16;
        transform.DOMoveY( pixel * _pixelToMove , _duration )
            .SetLoops( -1 , LoopType.Yoyo )
            .SetRelative()
            .Play();
    }
}
