using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPhysicalDataSO", menuName = "Player/PlayerPhysicalDataSO" )]
public class PlayerPhysicalDataSO : ScriptableObject
{
    [field: Header("Shared Data:")]
    [field: SerializeField] public LayerMask interactableLayerMask;
    [field: SerializeField] public string visualObjName;

    [field: Header("Movement:")]
    [field: SerializeField][Min(1)] public float moveSpeed;
    [field: SerializeField][Min(1)] public float accelerationOnAir;

    [field: Header("Jump:")]
    [field: SerializeField] public LayerMask boundsLayerMask;

    [field: Header("Pick Up Item:")]
    [field: SerializeField] public string pickUpPointObjName;
}
