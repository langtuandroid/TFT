// ************ @autor: Álvaro Repiso Romero *************
using System;
using System.Collections.Generic;

[Serializable]
public class ZoneSave
{
    public bool IsCompleted;
    public List<bool> IsActivatedList = new();
}