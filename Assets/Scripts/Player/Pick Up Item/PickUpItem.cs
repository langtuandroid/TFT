using System;
using System.Collections;
using UnityEngine;
using Utils;

public class PickUpItem : MonoBehaviour
{
    public static GameObject HeldItem;
    public Func<bool> CanPickUpItem = () => HeldItem != null;
    private bool _itemMoving;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER_PICKUP_POSITION) && HeldItem == null)
        {
            HeldItem = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(!_itemMoving)
            HeldItem = null;
    }
    
    public void PickUp(Transform _playerTransform)
    {
        _itemMoving = true;
        HeldItem.transform.position = new Vector3(_playerTransform.position.x,
            _playerTransform.transform.position.y + 1f);
    }

    public IEnumerator ThrowHeldItem(Vector2 direction)
    {
        Vector2 pos1 = new Vector2();
        Vector2 pos2 = new Vector2();

        if (direction == Vector2.right)
        {
            pos1 = new Vector3(transform.position.x + 0.5f, transform.position.y + 0.5f);
            pos2 = new Vector3(transform.position.x + 0.5f * direction.x, transform.position.y - 1f);
        } else if (direction == Vector2.left)
        {
            pos1 = new Vector3(transform.position.x - 0.5f, transform.position.y + 0.5f);
            pos2 = new Vector3(transform.position.x - 0.5f * direction.x, transform.position.y - 1f); 
        } else if (direction == Vector2.up)
        {
            pos1 = new Vector3(transform.position.x, transform.position.y + 0.5f);
            pos2 = new Vector3(transform.position.x * direction.x, transform.position.y + 0.5f);
        } else if (direction == Vector2.down)
        {
            pos1 = new Vector3(transform.position.x, transform.position.y - 0.5f);
            pos2 = new Vector3(transform.position.x * direction.x, transform.position.y - 0.5f);
        }

        float duration = 0.3f;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            HeldItem.transform.position = Vector3.Lerp(HeldItem.transform.position, pos1, t) * 2f * Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            HeldItem.transform.position = Vector3.Lerp(pos1, pos2, t) * 2f * Time.deltaTime;
            yield return null;
        }
        
        HeldItem.GetComponent<Collider2D>().enabled = true;
        
        Reset();
    }

    private void Reset()
    {
        HeldItem = null;
        _itemMoving = false;
    }
}
