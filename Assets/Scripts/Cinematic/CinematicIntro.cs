using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Utils;

public class CinematicIntro : MonoBehaviour
{
    [SerializeField] private GameObject _alex;
    [SerializeField] private CinemachineDollyCart _alexCam;
    [SerializeField] private GameObject _title;
    private Animator _anim;
    public bool IsCinematicFinished;

    private void Awake()
    {
        _anim = _alex.GetComponentInChildren<Animator>();
    }

    public void CanStart()
    {
        _alexCam.m_Speed = 3;
        StartCoroutine(CheckCinematicStatus());
    }
    
    private IEnumerator CheckCinematicStatus()
    {
        yield return new WaitForSeconds(0.1f);

        while (_alexCam.m_Position < _alexCam.m_Path.PathLength)
        {
            yield return null;
        }

        IsCinematicFinished = true;
        _anim.SetBool(Constants.ANIM_PLAYER_WALKING, false);
        _title.SetActive(true);
    }
}