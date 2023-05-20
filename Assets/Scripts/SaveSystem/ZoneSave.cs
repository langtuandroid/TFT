using System;
using System.Collections.Generic;

namespace SaveSystem
{
    [Serializable]
    public class ZoneSave
    {
        public bool IsCompleted;
        public List<bool> IsActivatedActivableObjectList;
    }
}