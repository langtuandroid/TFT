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
        public event Action OnRoomFinished;

        private static Transform _playerTransform;

        [SerializeField] private GameObject _playerPrefab;

        [Header("Doors")]
        [SerializeField] private DoorController _northDoorController;
        [SerializeField] private DoorController _eastDoorController;
        [SerializeField] private DoorController _southDoorController;
        [SerializeField] private DoorController _westDoorController;

        [Header("Enemies")]
        [SerializeField] private GameObject _bossRoomTransporterPrefab;
        [SerializeField] private GameObject _miniBossPrefab;
        [SerializeField] private List<GameObject> _enemyPrefabList;

        [Header("Props")]
        [SerializeField] private GameObject _treasureKeyPrefab;


        private DoorController[] _doorControllerArray;

        private int  _enemiesLeftInRoom;
        private bool _isMiniBossRoom;
        private static bool _playerHasKey;

        private void OnTriggerEnter2D( Collider2D collision )
        {
            OnEnterRoom?.Invoke();
        }


        public void SetRoom( RoomCell roomData )
        {
            if ( _playerHasKey ) _playerHasKey = false;

            if ( roomData.IsStartRoom )
            {
                Vector3 startPos = new( transform.position.x , transform.position.y - 0.5f , transform.position.z );
                GameObject playerObj = Instantiate( _playerPrefab , startPos , Quaternion.identity );
                playerObj.GetComponent<PlayerController>().Init( Vector2.down , 0 );

                _playerTransform = playerObj.transform;
                Camera.main.transform.position = new Vector3( transform.position.x , transform.position.y , -10 );
                SetRoomDoors( 0 , 0 );

                OnRoomFinished = null;
            }
            else 
            if ( roomData.IsBossRoom )
            {
                Instantiate( _bossRoomTransporterPrefab , transform.position , Quaternion.identity );
                SetRoomDoors( 0 , 0 );

                OnRoomFinished = null;
            }
            else
            {
                OnEnterRoom += CreateRoomEnemies;

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
                    Destroy( _doorControllerArray[i].gameObject );
            }
        }


        private void CreateRoomEnemies()
        {
            GameObject enemyInstantiated;

            if ( _isMiniBossRoom )
            {
                enemyInstantiated = Instantiate( _miniBossPrefab , transform.position , Quaternion.identity );
                enemyInstantiated.GetComponent<EnemySlime>().SetAsProceduralEnemy( _playerTransform );
                enemyInstantiated.GetComponent<SlimeHealth>().OnDeath += EnemiesInRoomCount;
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
            if ( _isMiniBossRoom )
            {
                Instantiate( _treasureKeyPrefab , transform.position , Quaternion.identity );
            }

            OnRoomFinished?.Invoke();

            OnEnterRoom = null;
            OnRoomFinished = null;
        }
    }
}