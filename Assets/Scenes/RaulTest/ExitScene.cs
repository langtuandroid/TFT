using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class ExitScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(Constants.TAG_PLAYER)) SceneManager.LoadScene(sceneName);
    }
}
