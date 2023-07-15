using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicTextManager : MonoBehaviour
{
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _text1;
    [SerializeField] private GameObject _text2;
    [SerializeField] private GameObject _text3;
    [SerializeField] private GameObject _text4;
    [SerializeField] private GameObject _text5;

    public bool PrologueTextDone;
    private float _seconds = 4f;

    public void CanStart()
    {
        StartCoroutine(nameof(PrologueText));
    }

    private IEnumerator PrologueText()
    {
        yield return new WaitForSeconds(0.5f);
        
        _text1.SetActive(true);
        
        yield return new WaitForSeconds(2.5f);
        
        _text1.SetActive(false);
        _text2.SetActive(true);
        
        yield return new WaitForSeconds(_seconds + 2f);
        
        _text1.SetActive(false);
        _text2.SetActive(false);
        _text3.SetActive(true);
        
        yield return new WaitForSeconds(_seconds + 2f);
        
        _text1.SetActive(false);
        _text2.SetActive(false);
        _text3.SetActive(false);
        _text4.SetActive(true);
        
        yield return new WaitForSeconds(_seconds + 3f);
        
        _text1.SetActive(false);
        _text2.SetActive(false);
        _text3.SetActive(false);
        _text4.SetActive(false);
        _text5.SetActive(true);
        
        yield return new WaitForSeconds(_seconds + 2f);
        PrologueTextDone = true;
        _background.SetActive(false);
        _text1.SetActive(false);
        _text2.SetActive(false);
        _text3.SetActive(false);
        _text4.SetActive(false);
        _text5.SetActive(false);
    }
}
