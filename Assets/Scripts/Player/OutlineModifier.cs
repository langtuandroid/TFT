using UnityEngine;

namespace Player
{
    public class OutlineModifier : MonoBehaviour
    {
        [SerializeField] private ColorArraySO _colorArraySO;
        [SerializeField] private Scriptable.PowerPanelDataListScriptable _powerColorDataSO;

        private Material _material;

        private void Awake()
        {
            _material = GetComponent<SpriteRenderer>().material;
        }

        public void ChangeIntensity( float value )
        {

        }

        public void ChangeEmissionColor( int colorIndex )
        {
            //_material.SetColor( "_OutlineColor" , _colorArraySO.ColorArray[colorIndex] );
            _material.SetColor( "_OutlineColor" , _powerColorDataSO.PowerPanelDataList[colorIndex].Color );
        }
    }
}