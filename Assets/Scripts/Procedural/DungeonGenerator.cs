// ************ @autor: Álvaro Repiso Romero *************
using System.Collections.Generic;
using UnityEngine;

namespace Procedural
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] private DungeonDataSO _dungeonDataSO;

        private Dictionary<Vector2Int, RoomCell> _dungeonLayOutDict;
        private List<Vector2Int> _deadEndPosList;

        private DungeonDrawer _dungeonDrawer;


        private readonly Vector2Int[] _dirList = {
                Vector2Int.up,     // 0001
                Vector2Int.right,  // 0010
                Vector2Int.down,   // 0100
                Vector2Int.left    // 1000
            };


        private void Awake() => Init();

        public void Init()
        {
            ClearMap();
            DungeonPathsGenerator();
            SelectBossAndMiniBossRooms();
            DrawGrid();
        }

        private void DrawGrid()
        {
            _dungeonDrawer.CreateDungeon( _dungeonLayOutDict );
        }        



        public void DungeonPathsGenerator()
        {
            Vector2Int startPos = new Vector2Int( _dungeonDataSO.DungeonGridSize / 2 , _dungeonDataSO.DungeonGridSize / 2);

            _dungeonLayOutDict.Add( startPos , new RoomCell() );
            _dungeonLayOutDict[startPos].SetAsStartRoom();


            int pathsFinished = 0;


            while ( pathsFinished < _dungeonDataSO.NumOfPaths )
            {
                Vector2Int currentPos = startPos;
                int roomsCreated = 1;

                // On startPos all directions could be excluded
                // if all sides are open
                // then choose a start direction here
                int randDir = Random.Range( 0 , _dirList.Length );

                while ( roomsCreated < _dungeonDataSO.NumOfRoomsPerPath )
                {
                    // open exit on current room
                    if ( HasPossibleDirectionsToGo( currentPos , out int excludedDirectionsBitMask ) )// && roomsCreated < _numOfRoomsPerPath )
                    {
                        randDir = RandomIntDirection( excludedDirectionsBitMask );
                    }
                    else
                    {
                        randDir = NoWhereToGoDirection( currentPos , randDir );

                        // allow get out of bounds and end path
                        if ( IsOutOfLimits( currentPos + _dirList[randDir] ) )
                            roomsCreated = _dungeonDataSO.NumOfRoomsPerPath;
                    }


                    _dungeonLayOutDict[currentPos].OpenRoomSide( 1 << randDir );
                    // ******

                    // open enter on next room
                    currentPos += _dirList[randDir];


                    if ( !_dungeonLayOutDict.ContainsKey( currentPos ) )
                    {
                        _dungeonLayOutDict.Add( currentPos , new RoomCell() );

                        roomsCreated++;
                    }


                    _dungeonLayOutDict[currentPos].OpenRoomSide( 1 << ComplementaryBit( randDir ) );
                    // ******
                }

                // last position added to _dungeonLayout is always a dead end
                _deadEndPosList.Add( currentPos );

                Debug.Log( "Generator: " + currentPos );

                pathsFinished++;
            }            
        }



        private void SelectBossAndMiniBossRooms()
        {
            int bossRoomIndex = Random.Range( 0 , _deadEndPosList.Count );
            Vector2Int bossRoomPos = _deadEndPosList[bossRoomIndex];
            _dungeonLayOutDict[bossRoomPos].SetAsBossRoom();

            int miniBossRoomIndex = Helpers.RandomIntExcludingValue( bossRoomIndex , _deadEndPosList.Count );
            Vector2Int miniBossRoomPos = _deadEndPosList[miniBossRoomIndex];
            _dungeonLayOutDict[miniBossRoomPos].SetAsMiniBossRoom();

            for ( int i = 0; i < 4; i++ )
            {
                if ( ( _dungeonLayOutDict[bossRoomPos].OpenSidesIntMask & 1 << i ) > 0 )
                {
                    Vector2Int neighbourPos = bossRoomPos + _dirList[i];
                    _dungeonLayOutDict[neighbourPos].BossDoorMask |= 1 << ComplementaryBit( i );

                    _dungeonLayOutDict[bossRoomPos].BossDoorMask |= 1 << i;
                }
            }
        }



        /// <summary> Check if going forward is a dead end and change direction </summary>
        /// <returns> <para>0 if is not a dead end,</para> 1 if is a dead end </returns>
        private int NoWhereToGoDirection( Vector2Int currentPos , int lastDir )
        {
            Vector2Int nextCandidatePos = currentPos + _dirList[lastDir];

            if ( _dungeonLayOutDict.ContainsKey( nextCandidatePos ) &&
                 _dungeonLayOutDict[nextCandidatePos].IsDeadEnd() )
            {
                lastDir = NoWhereToGoDirection( currentPos , LastResortCase( lastDir ) );
            }

            return lastDir;
        }




        /// <returns> A Random bit direction excluding the last comprobated direction </returns>
        private int LastResortCase( int lastDir )
        {
            int randDir = Random.Range( 0 , _dirList.Length - 1 );

            if ( randDir >= lastDir )
                randDir++;
            if ( randDir >= _dirList.Length ) 
                randDir -= _dirList.Length;

            return randDir;
        }




        /// <summary> Get a BitMask output with all posibles direction to go </summary>
        /// <returns>True if there are directions not open yet in the room</returns>
        private bool HasPossibleDirectionsToGo( Vector2Int currentPos , out int excludedDirectionsbitMask )
        {
            excludedDirectionsbitMask = _dungeonLayOutDict[currentPos].OpenSidesIntMask;


            for ( int i = 0; i < _dirList.Length; i++ )
            {
                // if already excluded -> ignore
                if ( ( excludedDirectionsbitMask & 1 << i ) > 0 ) 
                    continue;


                Vector2Int neighbourPos = currentPos + _dirList[i];


                if ( IsOutOfLimits( neighbourPos ) )
                {
                    //Debug.Log( "out of limits: " + neighbourPos );
                    excludedDirectionsbitMask |= 1 << i;
                    continue;
                }


                if ( _dungeonLayOutDict.ContainsKey( neighbourPos ) ) 
                {
                    if ( _dungeonLayOutDict[neighbourPos].IsDeadEnd() )
                    {
                        //Debug.Log( "dead end: " + neighbourPos );
                        excludedDirectionsbitMask |= 1 << i;
                        continue;
                    }

                    // if the side of the neighbour we are trying to enter is open
                    if ( _dungeonLayOutDict[neighbourPos].CheckIfSideOpen( 1 << ComplementaryBit( i ) ) )
                    {
                        excludedDirectionsbitMask |= 1 << i;
                    }
                }
            }

            return excludedDirectionsbitMask < 15; // 1111 all excluded return false
        }





        /// <param name="exceptions">BitMask with the exception flags</param>
        /// <param name="maxValue">Maximun exclusive value</param>
        /// <returns>A random integer between 0 and maxValue excluding the exceptions bitMask</returns>
        private int RandomIntDirection( int exceptions , int maxValue = 4 )
        {
            int result = Random.Range( 0, maxValue - 1 );

            for ( int i = 1; i < maxValue; i++ )
            {
                if ( ( 1 << result & exceptions ) == 0 ) // There are no coincidences then is a valid direction
                    break;
                else
                {
                    result++;
                    if ( result >= maxValue ) result -= maxValue;
                }
            }
            return result;
        }


        /// <summary> Check if the position is outside of the delimited area </summary>
        /// <returns> True if out of limits </returns>
        private bool IsOutOfLimits( Vector2Int checkPos )
        {
            return checkPos.x < 0 || checkPos.x >= _dungeonDataSO.DungeonGridSize ||
                   checkPos.y < 0 || checkPos.y >= _dungeonDataSO.DungeonGridSize;
        }



        /// <returns>The complementary bit position bitwise of the bitFlag</returns>
        private int ComplementaryBit( int bitFlag , int bitsBlock = 4 )
        {
            bitFlag += bitsBlock / 2;
            if ( bitFlag >= bitsBlock ) bitFlag -= bitsBlock;
            return bitFlag;
        }



        public void ClearMap()
        {
            _dungeonDrawer = GetComponent<DungeonDrawer>();
            _dungeonDrawer.ClearTilemaps();
            _dungeonLayOutDict = new Dictionary<Vector2Int , RoomCell>();
            _deadEndPosList = new List<Vector2Int>();
        }
    }
}