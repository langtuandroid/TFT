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

    private List<GameObject> _collisions;

    private ParticleSystem _particles;

    private float _lifeTime;
    private float _stopTime;

    private void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
        _lifeTime = 0;
        _stopTime = 0;
        _collisions = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _collisions.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _collisions.Remove(collision.gameObject);
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
        foreach (GameObject hit in _collisions)
        {
            float distance = Vector2.Distance(hit.transform.position, transform.position);

            if (distance <= Mathf.Min(_lifeTime, _maxDistance) &&
                distance >= _stopTime &&
                hit.CompareTag(Constants.TAG_TORCH)
                && AngleWith(hit) <= Constants.ANGLE_FLAMETHROWER)
            {
                Torch t = hit.GetComponent<Torch>();
                t.ActivateTorch();
            }
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


    public float AngleWith(GameObject obj)
    {
        // Vector 3
        Vector2 playerDir = obj.transform.position - transform.position;
        return Vector3.Angle(GetDirection(), playerDir);
    }
}
