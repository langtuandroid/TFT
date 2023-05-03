using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Player;

public class Level1Manager : MonoBehaviour
{
    public static Level1Manager instance;

    #region Referencias

    [SerializeField]
    private Transform _destinationTransform;

    [SerializeField]
    private CinemachineVirtualCamera VCam1;

    [SerializeField]
    private CinemachineVirtualCamera VCam2;

    [SerializeField]
    private CinemachineVirtualCamera VCam3;

    [SerializeField]
    private CinemachineVirtualCamera VCam4;

    #endregion

    private float _secondsToChangeCam;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _secondsToChangeCam = 1f;
    }

    public void GotoCave()
    {
        StartCoroutine("ChangeCamera");
    }

    public void GoToLevelTest()
    {
        VCam3.enabled = true;
        VCam1.enabled = false;
        VCam2.enabled = false;
    }

    public void GoToLevelBoss()
    {
        VCam4.enabled = true;
        VCam1.enabled = false;
        VCam2.enabled = false;
        VCam3.enabled = false;
    }

    private void ResetTransition()
    {
        TransitionManager.instance.CrossFade();
    }

    private IEnumerator ChangeCamera()
    {
        ResetTransition();

        VCam2.enabled = true;

        yield return new WaitForSeconds(_secondsToChangeCam);

        PlayerMovement.Instance.ChangeWorldPosition(_destinationTransform);

        VCam1.enabled = false;
    }
}
