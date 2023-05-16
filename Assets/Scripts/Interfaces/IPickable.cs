using UnityEngine;

public interface IPickable
{
    void PickItUp(Vector2 lookDirection);
    void ThrowIt(Vector2 lookDirection);
    void ShowCanPickUpItem(bool show);
}
