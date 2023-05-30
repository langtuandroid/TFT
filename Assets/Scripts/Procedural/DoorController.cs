using UnityEngine;

namespace Procedural
{
    public class DoorController : MonoBehaviour
    {
        [SerializeField] private GameObject _normalDoorVisual;
        [SerializeField] private GameObject _keyDoorVisual;

        private Collider2D _doorCollider;
        private bool _isBossDoor;
        private bool _hasKey;

        private void OnCollisionEnter2D( Collision2D collision )
        {
            if ( _isBossDoor && _hasKey )
            {
                _doorCollider.enabled = false;
                _keyDoorVisual.SetActive( false );
            }
        }


        public void SetDoor( bool isBossDoor , RoomController roomController )
        {
            _doorCollider = GetComponent<Collider2D>();

            _isBossDoor = isBossDoor;

            Destroy( isBossDoor ? _normalDoorVisual : _keyDoorVisual );

            OpenDoor();

            if ( isBossDoor )
            {
                _doorCollider.enabled = true;
                _keyDoorVisual.SetActive( true );
                ServiceLocator.GetService<LevelEvents>().OnKeyObtained += LevelEvents_OnKeyObtained;
            }
            else
                roomController.OnRoomFinished += OpenDoor;

            roomController.OnEnterRoom += CloseDoor;
        }

        private void LevelEvents_OnKeyObtained()
        {
            _hasKey = true;
        }

        private void OpenDoor()
        {
            _doorCollider.enabled = false;
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

        private void OnDestroy()
        {
            if ( _isBossDoor )
                ServiceLocator.GetService<LevelEvents>().OnKeyObtained -= LevelEvents_OnKeyObtained;
        }
    }
}