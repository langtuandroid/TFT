// ************ @autor: Álvaro Repiso Romero *************

namespace Procedural
{
    public class RoomCell
    {
        private int _openSidesMask;

        public int OpenSidesIntMask => _openSidesMask;

        public bool IsStartRoom { get; private set; }
        public bool IsMiniBossRoom { get; private set; }
        public bool IsBossRoom { get; private set; }

        public int BossDoorMask { get; set; }

        
        public void OpenRoomSide( int sideToOpenIntMask )
        {
            _openSidesMask |= sideToOpenIntMask;
        }
        
        
        public bool CheckIfSideOpen( int sideIntMask )
        {
            return ( _openSidesMask & sideIntMask ) > 0;
        }


        public bool IsDeadEnd()
        {
            int sidesOpenCount = 0;
            int numberOfSides = 4;

            for ( int i = 0; i < numberOfSides; i++ )
                if ( ( _openSidesMask & 1 << i ) > 0 )
                    sidesOpenCount++;

            return sidesOpenCount == 1;
        }


        public void SetAsStartRoom() => IsStartRoom = true;
        public void SetAsBossRoom() => IsBossRoom = true;
        public void SetAsMiniBossRoom() => IsMiniBossRoom = true;
    }
}