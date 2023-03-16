using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static FMOD.Studio.EventInstance _musicEventInstance;

    [SerializeField] private FMODUnity.EventReference music;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _musicEventInstance = FMODUnity.RuntimeManager.CreateInstance(music);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_musicEventInstance, transform);
        _musicEventInstance.start();
    }
}
