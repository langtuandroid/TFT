using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicDirector : MonoBehaviour
{
    [SerializeField] private CinematicTextManager _cinematicTextManager;
    [SerializeField] private CinematicIntro _cinematicIntro;

    void Start()
    {
        _cinematicTextManager.CanStart();
        StartCoroutine(nameof(NextSequence));
    }

    private IEnumerator NextSequence()
    {
        yield return new WaitUntil(() => _cinematicTextManager.PrologueTextDone);
        _cinematicIntro.CanStart();
    }
    
}
