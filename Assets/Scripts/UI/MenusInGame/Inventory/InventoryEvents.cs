using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEvents
{

    #region Events

    public event Action<int> OnPrimarySkillChange;
    public event Action<int> OnSecondarySkillChange;

    #endregion

    #region Public methods

    public void ChangePrimarySkill(int skillIndex)
    {
        OnPrimarySkillChange?.Invoke(skillIndex);
    }

    public void ChangeSecondarySkill(int skillIndex)
    {
        OnSecondarySkillChange?.Invoke(skillIndex);
    }

    #endregion


}
