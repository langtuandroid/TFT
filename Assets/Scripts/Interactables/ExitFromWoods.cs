using System;
using System.Collections;
using System.Collections.Generic;
using Honeti;
using UnityEngine;

public class ExitFromWoods : MonoBehaviour
{
  private List<string> exitCode = new List<string>();
  public string wrongMessage = "^exitWoods_S14_Z0_4-1";
  public Switch buttonSun;
  public Switch buttonMoon;
  public Switch buttonStar;
  
  public void ActivateExit(string butonType)
  {
      exitCode.Add(butonType);
      CheckExit();
  }

  private void CheckExit()
  {
      if (exitCode.Count == 3)
      {
          if (exitCode[0] == "SUN" && exitCode[1] == "MOON" && exitCode[2] == "STAR")
          {
              gameObject.SetActive(false);
          }
          else
          {
              exitCode.Clear();
              buttonSun.ResetPuzle = true;
              buttonMoon.ResetPuzle = true;
              buttonStar.ResetPuzle = true;
              MyDialogueManager.Instance.Text(I18N.instance.getValue(wrongMessage));
          }
      }
  }
}
