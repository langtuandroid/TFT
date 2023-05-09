using UnityEngine;

public class Mushroom : MonoBehaviour, IJumpable
{
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void JumpIn( Transform transform )
    {
        transform.position += Vector3.up * 2;
    }

    public void CanBeJump()
    {
        _collider.isTrigger = true;
    }
}
