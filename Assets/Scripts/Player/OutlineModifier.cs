using UnityEngine;

namespace Player
{
    public class OutlineModifier : MonoBehaviour
    {
        private Material _material;

        private void Awake()
        {
            _material = GetComponent<SpriteRenderer>().material;
        }

        public void ChangeIntensity( float value )
        {

        }

        public void ChangeEmissionColor( Color color )
        {
            _material.SetColor( "_OutlineColor" , color );
        }
    }
}