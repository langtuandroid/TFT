using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PruebaCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Level1Manager.instance.GoToLevelTest();
    }
}
