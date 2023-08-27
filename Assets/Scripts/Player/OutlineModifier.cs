using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class OutlineModifier
    {
        private List<Color> _powerColorList;
        private Material _material;

        public OutlineModifier( Material material , Scriptable.PowerPanelDataListScriptable powerPanelData )
        {
            _material = material;

            _powerColorList = new();
            foreach ( var powerDataList in powerPanelData.PowerPanelDataList )
                _powerColorList.Add( powerDataList.Color );
        }

        public void ChangeIntensity( float value )
        {

        }

        public void ChangeEmissionColor( int colorIndex )
        {
            _material.SetColor( "_OutlineColor" , _powerColorList[colorIndex] );
        }
    }
}