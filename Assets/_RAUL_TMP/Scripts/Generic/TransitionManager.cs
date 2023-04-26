using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;

    [SerializeField]
    private GameObject _fadeIn;

    [SerializeField]
    private GameObject _fadeOut;

    private float _secondsToWait;

    private void Awake()
    {
        //Hacemos Singleton a la clase
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _secondsToWait = 3f;

        StartCoroutine(nameof(BeginScene));
    }

    public void CrossFade()
    {
        StartCoroutine(nameof(WaitTransition));
    }

    private IEnumerator BeginScene()
    {
        _fadeOut.SetActive(true);

        yield return new WaitForSeconds(_secondsToWait);

        _fadeOut.SetActive(false);
    }

    private IEnumerator WaitTransition()
    {
        _fadeIn.SetActive(true);

        yield return new WaitForSeconds(_secondsToWait);

        _fadeIn.SetActive(false);

        _fadeOut.SetActive(true);
    }
}