using System;
using UnityEngine;

namespace Scriptable
{
    [Serializable]
    public class PowerPanelData
    {
        public string PowerName;
        public Color Color;

        public PowerPanelData(string name, Color color) 
        { 
            PowerName = name;
            Color = color;
        }

    }

}
