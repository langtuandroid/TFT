using UnityEngine;
using DG.Tweening;
using Utils;

public class Pickable : MonoBehaviour, IPickable
{
    [SerializeField] private GameObject _exclamationIcon;
    private Collider2D _collider;
    private Transform _pickItUpPoint;

    private bool _canPickItUp = true;
    

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _pickItUpPoint = FindGameObject.WithCaseInsensitiveTag(Constants.TAG_PLAYER_PICKUP_POSITION).GetComponent<Transform>();
    }
    
    public void PickItUp(Vector2 lookDirection)
    {
        if (_canPickItUp && lookDirection.y > 0)
        {
            _canPickItUp = false;
            _collider.enabled = false;
            transform.parent = _pickItUpPoint;
            ShowCanPickUpItem(false);
            transform.DOMove(_pickItUpPoint.position, 0.3f).SetEase(Ease.Linear).Play();
        }
    }

    public void ThrowIt(Vector2 lookDirection)
    {
        _canPickItUp = true;
        transform.parent = null;
        _collider.enabled = true;

        Vector3 jumpTarget = lookDirection * 2f;

        transform.DOJump(jumpTarget, 1f, 1, 1f).SetEase(Ease.OutQuad).Play();
    }
    
    public void ShowCanPickUpItem(bool show)
    {
        _exclamationIcon.SetActive(show);
    }
}
