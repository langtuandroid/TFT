// ************ @autor: Álvaro Repiso Romero *************
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Procedural
{
    public class DungeonDrawer : MonoBehaviour
    {
        [Header("Tilemap Related:")]
        [SerializeField] private Tilemap _wallsTileMap;
        [SerializeField] private Tilemap _groundTileMap;
        [SerializeField] private TileBase _groundTile;
        [SerializeField] private TileBase _wallTile;

        [Header("Rooms Related:")]
        [SerializeField] private Texture2D _roomTex;
        [SerializeField] private GameObject _roomControllerPrefab;
        private Vector3 _roomOffset = new Vector3( 10 , 5.5f );

        private int _roomWidth;
        private int _roomHeight;


        private readonly Color[] _doorSideColor = {
            Color.white,   // 0001 - North 
            Color.red,     // 0010 - East
            Color.green,   // 0100 - South
            Color.blue     // 1000 - West
        };


        public void ClearTilemaps()
        {
            // init TileMaps
            _wallsTileMap.hideFlags = HideFlags.None;
            _wallsTileMap.ClearAllTiles();

            _groundTileMap.hideFlags = HideFlags.None;
            _groundTileMap.ClearAllTiles();

            // Init variables
            _roomHeight = _roomTex.height;
            _roomWidth = _roomTex.width;
        }


        public void CreateDungeon( Dictionary<Vector2Int , RoomCell> dungeonLayOutDict )
        {
            // Draw dungeon
            foreach ( var item in dungeonLayOutDict )
                DrawRoom( (Vector3Int)item.Key , item.Value );
        }



        private void DrawRoom( Vector3Int startRoomPos, RoomCell room )
        {
            startRoomPos *= new Vector3Int( _roomWidth , _roomHeight );

            for ( int x = 0; x < _roomWidth; x++ )
            {
                for ( int y = 0; y < _roomHeight; y++ )
                {
                    Vector3Int posToFill = startRoomPos + new Vector3Int( x, y );
                    DrawTile( posToFill , room , x , y );
                }
            }

            // Room Controller 
            Vector3 worldPos = startRoomPos + _roomOffset;

            GameObject roomControllerObj = Instantiate( _roomControllerPrefab , worldPos , Quaternion.identity );

            roomControllerObj.GetComponent<RoomController>().SetRoom( room );
        }



        private void DrawTile( Vector3Int posToFill , RoomCell room , int x , int y )
        {
            Color pixelColor = _roomTex.GetPixel( x , y );

            // Set Ground in all tiles
            _groundTileMap.SetTile( posToFill , _groundTile );

            // Set Walls
            if ( pixelColor.Equals( Color.black ) )
                _wallsTileMap.SetTile( posToFill , _wallTile );
            else
            {
                // Set Walls on possible doors place if needed
                for ( int i = 0; i < _doorSideColor.Length; i++ )
                    if ( pixelColor.Equals( _doorSideColor[i] ) )
                        if ( !room.CheckIfSideOpen( 1 << i ) )
                            _wallsTileMap.SetTile( posToFill , _wallTile );
            }
        }
    }
}