// ************ @autor: Álvaro Repiso Romero *************
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "StartRefInfoSO", menuName = "Level Control/StartRefInfoSO")]
public class StartRefInfoSO : ScriptableObject
{
    public StartRefInfo[] startRefInfoArray;

    [Serializable]
    public struct StartRefInfo
    {
        public string exitZoneDescription;

        [Tooltip("Only positive values")]
        public int startPointRefID;
        public Vector2 startPosition;

        private enum LookDirection { Up, Right, Down, Left };
        [SerializeField] private LookDirection _playerStartLookDirection;

        public LayerMask initialLayerMask;
        [Range(0, 10)] public int confinerColliderIndex; 

        public Vector2 PlayerStartLookDirection()
        {
            switch ( _playerStartLookDirection )
            {
                case LookDirection.Right:
                    return Vector2.right;
                case LookDirection.Down:
                    return Vector2.down;
                case LookDirection.Left:
                    return Vector2.left;
            }
            return Vector2.up;
        }
    }
}
