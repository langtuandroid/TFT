using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitFromWoods : MonoBehaviour
{
  private List<string> exitCode = new List<string>();
  
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
      }
  }
}
