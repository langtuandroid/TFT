using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEvents
{

    #region Events

    public Action<int> OnGotSoulsValue;

    #endregion

    #region Public methods

    public void GotSouls(int quantity)
    {
        OnGotSoulsValue.Invoke(quantity);
    }

    #endregion

}
