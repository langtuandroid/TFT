using System;
using UnityEngine;

namespace Procedural
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private GameObject _normalDoorVisual;
        [SerializeField] private GameObject _keyDoorVisual;

        private Collider2D _doorCollider;
        private bool _isBossDoor;


        public void SetDoor( bool isBossDoor , RoomController roomController )
        {
            _doorCollider = GetComponent<Collider2D>();

            _isBossDoor = isBossDoor;

            DestroyImmediate( isBossDoor ? _normalDoorVisual : _keyDoorVisual );

            OpenDoor();

            if ( isBossDoor )
            {
                _doorCollider.enabled = true;
                _keyDoorVisual.SetActive( true );

                //roomController.OnKeyObtained += _keyDoorVisual.GetComponent<BossDoor>().HasGetKey;
            }

            roomController.OnEnterRoom += CloseDoor;
        }

        private void OpenDoor()
        {
            _doorCollider.enabled = false;

            if ( _isBossDoor )
                _keyDoorVisual.SetActive( false );
            else
                _normalDoorVisual.SetActive( false );
        }

        private void CloseDoor()
        {
            _doorCollider.enabled = true;

            if ( _isBossDoor )
                _keyDoorVisual.SetActive( true );
            else
                _normalDoorVisual.SetActive( true );
        }
    }
}