using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemArmIA : MonoBehaviour
{
    public float speed = 1.0f;
    public float maxDistance = 0.2f;
    private Vector3 originalPosition;
    private bool movingRight = true;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        float distance = Vector3.Distance(transform.position, originalPosition);
        if (distance >= maxDistance)
        {
            movingRight = !movingRight;
        }
    }
}
