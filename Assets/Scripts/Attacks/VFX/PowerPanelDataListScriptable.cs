using System.Collections.Generic;
using UnityEngine;


namespace Scriptable
{
    [CreateAssetMenu(fileName = "PowerPanelDataListScriptable", menuName = "PowerPanelDataList")]
    public class PowerPanelDataListScriptable : ScriptableObject
    {
        public List<PowerPanelData> PowerPanelDataList;
    }

}
