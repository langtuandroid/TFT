using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class FlameDetection : MonoBehaviour
{

    [SerializeField]
    private float _maxDistance = 4f;

    [SerializeField]
    FlameDirection _direction;

    private enum FlameDirection
    {
        Up, Down, Left, Right
    }

    private List<Collider2D> _collisions;

    private ParticleSystem _particles;

    private float _lifeTime;
    private float _stopTime;

    private void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
        _lifeTime = 0;
        _stopTime = 0;
        _collisions = new List<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(Constants.TAG_MAGIC_POWER))
            _collisions.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _collisions.Remove(collision);
    }

    private void Update()
    {
        if (_stopTime >= _maxDistance)
            return;

        if (_lifeTime < _maxDistance)
            _lifeTime += Time.deltaTime;

        if (_particles.isStopped)
            _stopTime += Time.deltaTime;

        CheckCollisions();
    }

    private void CheckCollisions()
    {
        foreach (Collider2D hit in _collisions)
        {
            float distance = Vector2.Distance(
                hit.ClosestPoint(transform.position),
                transform.position
                );

            string text = $"Distancia: {distance}\n" +
                $"Ángulo: {AngleWith(hit)}";

            Debug.Log(text);

            // Si cumple las condiciones de distancia y ángulo,
            // y se trata de un elemento quemable
            if (distance <= Mathf.Min(_lifeTime, _maxDistance) &&
                distance >= _stopTime)
                //AngleWith(hit) <= Constants.ANGLE_FLAMETHROWER &&
                // Lo activamos
                hit.gameObject.GetComponent<IBurnable>()?.Burn();


        }

    }

    private Vector2 GetDirection()
    {
        Vector2 dir = Vector2.zero;
        switch (_direction)
        {
            case FlameDirection.Up:
                dir = transform.up;
                break;
            case FlameDirection.Down:
                dir = -transform.up;
                break;
            case FlameDirection.Left:
                dir = -transform.right;
                break;
            case FlameDirection.Right:
                dir = transform.right;
                break;
        }

        return dir;
    }


    private float AngleWith(Collider2D obj)
    {
        // Vector 3
        Vector2 playerDir = obj.ClosestPoint(transform.position) - (Vector2)transform.position;

        return Vector3.Angle(GetDirection(), playerDir);
    }
}
