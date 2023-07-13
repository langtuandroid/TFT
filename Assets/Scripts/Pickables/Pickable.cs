using UnityEngine;

public class Pickable : MonoBehaviour, IPickable
{
    [SerializeField] private GameObject _exclamationIcon;
    [SerializeField] private float throwSpeedX;
    [SerializeField] private float throwSpeedY;
    [SerializeField] private float throwAcceleration;

    private Vector2 throwVelocity;
    private Collider2D _collider;
    private bool _canPickItUp = true;
    private bool _canThrowIt = false;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!_canThrowIt) return;
            //ThrowInertia();
    }

    public void PickItUp(Vector2 lookDirection, Transform pickUpPoint)
    {
        if (_canPickItUp)
        {
            ResetValues();
            transform.parent = pickUpPoint;
            transform.localPosition = Vector3.zero;
        }
    }

    public void ThrowIt(Vector2 lookDirection)
    {
        ShowCanPickUpItem(false);

        _canPickItUp = true;

        transform.parent = null;

        throwVelocity = lookDirection.normalized;
        throwVelocity.x *= throwSpeedX;
        throwVelocity.y *= throwSpeedY;

        _canThrowIt = true;

        _collider.enabled = true;
    }

    public void ShowCanPickUpItem(bool show)
    {
        _exclamationIcon.SetActive(show);
    }

    private void ThrowInertia()
    {
        //if(throwVelocity.y == 0f)
            throwVelocity.y += throwSpeedY * Time.deltaTime;
        //else if (throwVelocity.x == 0f)
            throwVelocity.x += throwSpeedX * Time.deltaTime;
        
        throwVelocity += throwVelocity.normalized * throwAcceleration * Time.deltaTime;
        
        transform.Translate(throwVelocity * Time.deltaTime);
    }

    private void ResetValues()
    {
        _canPickItUp = false;
        _canThrowIt = false;
        _collider.enabled = false;
    }
}
