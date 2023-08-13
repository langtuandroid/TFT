using System;
using System.Collections;
using UnityEngine;
using Utils;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public float spawnInterval = 5f; 
    public int maxEnemies = 10; 
    public float spitForce = 10f; 
    public float spitDuration = 0.5f; 
    public float spitHeight = 2f;
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
                Vector2 spitDirection = Quaternion.Euler(0, 0, Random.Range(-30f, 30f)) * Vector2.up; 
                rb.velocity = spitDirection * spitForce;
                
                rb.DOMoveY(rb.position.y + spitHeight, spitDuration * 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    rb.DOMoveY(rb.position.y, spitDuration * 0.5f).SetEase(Ease.InQuad);
                });
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

    public void SpawnEnemiesWhenDead(int cant)
    {
        for (int i = 0; i < cant; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, PlayerDetectionRadious);
    }
}
