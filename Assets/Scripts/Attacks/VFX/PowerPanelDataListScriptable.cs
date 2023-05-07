using System.Collections.Generic;
using UnityEngine;


namespace Scriptable
{
    [CreateAssetMenu(fileName = "PowerPanelDataListScriptable", menuName = "PowerPanelDataListScriptableSO")]
    public class PowerPanelDataListScriptable : ScriptableObject
    {
        public List<PowerPanelData> PowerPanelDataList;
    }

}
