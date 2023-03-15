// ************ @autor: Álvaro Repiso Romero *************

namespace Procedural
{
    public static class Helpers
    {
        /// <returns>The binary format of myValue with bitblock as number of digits</returns>
        public static string ToBinaryString( int myValue , int bitblock = 4 )
        {
            string binVal = System.Convert.ToString(myValue, 2);
            int bits = 0;

            for ( int i = 0; i < binVal.Length; i += bitblock )
                bits += bitblock;

            return binVal.PadLeft( bits , '0' );
        }


        /// <returns>The binary format of myValue with bitblock as number of digits</returns>
        public static int RandomIntExcludingValue( int excludedValue , int maxValue )
        {
            int result = UnityEngine.Random.Range( 0, maxValue - 1 );

            if ( result >= excludedValue )
                result++;

            return result;
        }
    }
}