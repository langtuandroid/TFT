// ************ @autor: �lvaro Repiso Romero *************
using System;
using System.Collections.Generic;

namespace SaveSystem
{
    [Serializable]
    public class ZoneSave
    {
        public bool IsCompleted;
        public List<bool> IsActivatedList;
    }
}