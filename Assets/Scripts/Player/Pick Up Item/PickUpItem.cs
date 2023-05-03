using System.Collections;
using Player;
using UnityEngine;
using Utils;

public class PickUpItem : MonoBehaviour
{
    private GameObject _heldItem;
    private bool isThrowing; 
    
    void Update()
    {
        CheckHeldItem();
    }

    private void CheckHeldItem()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _heldItem != null)
        {
            isThrowing = true;
            GetPlayerDirection();
        }
        
        if (_heldItem != null && !isThrowing)
        {
            _heldItem.transform.position = new Vector3(PlayerMovement.Instance.transform.position.x,
                PlayerMovement.Instance.transform.position.y + 1f);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER_PICKUP_POSITION) && _heldItem == null)
        {
            _heldItem = other.gameObject;
            _heldItem.transform.localPosition = Vector3.zero; 
            _heldItem.GetComponent<Collider2D>().enabled = false;
        }
    }
    
    private IEnumerator ThrowHeldItem(Vector2 direction)
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

            _heldItem.transform.position = Vector3.Lerp(transform.position, pos1, t) * 2f * Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            _heldItem.transform.position = Vector3.Lerp(pos1, pos2, t) * 2f * Time.deltaTime;
            yield return null;
        }
        
        _heldItem.GetComponent<Collider2D>().enabled = true;
        
        _heldItem = null;
        
        isThrowing = false;
    }
    
    public void GetPlayerDirection()
    {
        switch (PlayerMovement.Instance.Layer)
        {
            case PlayerMovement.AnimationLayers.WalkDown:
                StartCoroutine(ThrowHeldItem(Vector3.down));
                break;
            case PlayerMovement.AnimationLayers.WalkHorizontal:
                if (PlayerMovement.Instance.HorizontalFlip)
                    StartCoroutine(ThrowHeldItem(Vector3.left));
                else
                    StartCoroutine(ThrowHeldItem(Vector3.right));
                break;
            case PlayerMovement.AnimationLayers.WalkUp:
                StartCoroutine(ThrowHeldItem(Vector3.up));
                break;
        }
    }
    
}
