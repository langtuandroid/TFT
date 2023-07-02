using UnityEngine;

public interface IPickable
{
    void PickItUp(Vector2 lookDirection, Transform pickUpPoint);
    void ThrowIt(Vector2 lookDirection);
    void ShowCanPickUpItem(bool show);
}
