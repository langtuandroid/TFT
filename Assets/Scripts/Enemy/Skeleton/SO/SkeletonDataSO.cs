using UnityEngine;

[CreateAssetMenu(fileName = "SkeletonDataSO", menuName = "Enemies/Skeleton Data SO")]
public class SkeletonDataSO : ScriptableObject
{
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float DetectionRadius { get; private set; }
    [field: SerializeField] public float AttackIntervalSeconds { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public LayerMask PlayerMask { get; private set; }
    [field: SerializeField] public LayerMask ObstaclesMask { get; private set; }
    [field: SerializeField] public GameObject BonePrefab { get; private set; }
}
