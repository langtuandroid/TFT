using System;
using System.Collections;
using UnityEngine;
using Utils;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public float spawnInterval = 5f; 
    public int maxEnemies = 10; 
    public float spitForce = 10f; 
    public float PlayerDetectionRadious = 3f;

    private int currentEnemies = 0;
    private bool spawningEnabled = true;
    private bool isSpawning = false;

    private void Update()
    {
        if (CheckPlayer() && !isSpawning)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    private bool CheckPlayer()
    {
        Collider2D results = Physics2D.OverlapCircle(transform.position, PlayerDetectionRadious, LayerMask.GetMask(Constants.TAG_PLAYER));

        if (results != null)
        {
            if (results.CompareTag(Constants.TAG_PLAYER))
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator SpawnEnemies()
    {
        isSpawning = true;

        while (currentEnemies < maxEnemies)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = newEnemy.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(Vector2.up * spitForce, ForceMode2D.Impulse);
            }
            currentEnemies++;

            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }

    public void StopSpawning()
    {
        spawningEnabled = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, PlayerDetectionRadious);
    }
}