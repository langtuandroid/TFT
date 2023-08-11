using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorExit : MonoBehaviour
{
    private int cont = 0;


    public void IncreaseCount()
    {
        cont++;
        if(cont == 4) gameObject.SetActive(false);
    }
}
