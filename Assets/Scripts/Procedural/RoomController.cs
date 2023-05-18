using Player;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Procedural
{
    public class RoomController : MonoBehaviour
    {
        public event Action OnEnterRoom;

        private static Transform _playerTransform;

        [SerializeField] private GameObject _playerPrefab;

        [Header("Doors")]
        [SerializeField] private DoorController _northDoorController;
        [SerializeField] private DoorController _eastDoorController;
        [SerializeField] private DoorController _southDoorController;
        [SerializeField] private DoorController _westDoorController;

        [Header("Enemies")]
        [SerializeField] private GameObject _bossPrefab;
        [SerializeField] private GameObject _miniBossPrefab;
        [SerializeField] private List<GameObject> _enemyPrefabList;

        [Header("Props")]
        [SerializeField] private GameObject _treasureKeyPrefab;


        private DoorController[] _doorControllerArray;

        private int _enemiesLeftInRoom;
        private bool _isBossRoom;
        private bool _isMiniBossRoom;


        private void OnTriggerEnter2D( Collider2D collision )
        {
            if ( collision.gameObject.tag.Equals( "Player" ) )
            {
                Debug.Log( "enter room " + _playerTransform.position );
                OnEnterRoom?.Invoke();
            }
        }

        private void OnTriggerExit2D( Collider2D collision )
        {
            if ( collision.gameObject.tag.Equals( "Player" ) )
            {
                Debug.Log( "room cleared" );
                RoomHasBeenClear();
            }
        }



        public void SetRoom( RoomCell roomData , Cinemachine.CinemachineVirtualCamera camera )
        {
            if ( roomData.IsStartRoom )
            {
                GameObject playerObj = Instantiate( _playerPrefab , transform.position , Quaternion.identity );
                _playerTransform = playerObj.transform;
                camera.Follow = playerObj.transform;
                playerObj.GetComponentInChildren<AnimatorBrain>().Init( Vector2.down ); 
                Destroy( gameObject );
            }
            else
            {
                OnEnterRoom += CreateRoomEnemies;

                _isBossRoom = roomData.IsBossRoom;
                _isMiniBossRoom = roomData.IsMiniBossRoom;

                SetRoomDoors( roomData.OpenSidesIntMask , roomData.BossDoorMask );
            }
        }


        private void SetRoomDoors( int openSidesMask , int bossDoorMask )
        {
            _doorControllerArray = new[] { _northDoorController ,  // 0001
                                           _eastDoorController ,   // 0010
                                           _southDoorController ,  // 0100
                                           _westDoorController };  // 1000


            for ( int i = 0; i < _doorControllerArray.Length; i++ )
            {
                if ( ( openSidesMask & 1 << i ) > 0 )
                {
                    _doorControllerArray[i].gameObject.SetActive( true );

                    bool isBossDoor = ( bossDoorMask & 1 << i ) > 0;
                    _doorControllerArray[i].SetDoor( isBossDoor , this );
                }
                else
                    DestroyImmediate( _doorControllerArray[i].gameObject );
            }
        }


        private void CreateRoomEnemies()
        {
            GameObject enemyInstantiated;

            if ( _isBossRoom )
            {
                enemyInstantiated = Instantiate( _bossPrefab , transform.position , Quaternion.identity );
                //enemyInstantiated.GetComponent<EnemyController>().OnDeath += EnemiesInRoomCount;
            }
            else 
            if ( _isMiniBossRoom )
            {
                enemyInstantiated = Instantiate( _miniBossPrefab , transform.position , Quaternion.identity );
                //enemyInstantiated.GetComponent<EnemyController>().OnDeath += EnemiesInRoomCount;
            }
            else
            {
                int numOfEnemies = Random.Range( 1 , 2 );
                for ( int i = 0; i < numOfEnemies; i++ )
                {
                    Debug.Log( "create enemy" );
                    int randIndex = Random.Range( 0 , _enemyPrefabList.Count - 1 );

                    float x = Random.Range( -5 , 5 );
                    float y = Random.Range( -2 , 2 );
                    Vector3 position = transform.position + new Vector3( x, y );

                    enemyInstantiated = Instantiate( _enemyPrefabList[randIndex] , position , Quaternion.identity );
                    Debug.Log( "instantiated enemy" );
                    enemyInstantiated.GetComponent<EnemySlime>().SetAsProceduralEnemy( _playerTransform );
                    enemyInstantiated.GetComponent<SlimeHealth>().OnDeath += EnemiesInRoomCount;

                    _enemiesLeftInRoom++;
                }
            }
        }


        private void EnemiesInRoomCount()
        {
            _enemiesLeftInRoom--;

            if ( _enemiesLeftInRoom <= 0 )
                RoomHasBeenClear();
        }



        private void RoomHasBeenClear()
        {
            if ( _isBossRoom )
            {
                Debug.Log( "Level Completed" );
                // Level Complete Event
            }
            else
            if ( _isMiniBossRoom )
            {
                Debug.Log( "Key Obtained" );
                //Instantiate( _treasureKeyPrefab , transform.position , Quaternion.identity );
            }

            OnEnterRoom = null;
            Destroy( gameObject );
        }
    }
}