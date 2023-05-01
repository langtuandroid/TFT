using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{

    [SerializeField]
    [Tooltip("Icono que aparece para indicar que puedes abrir el cofre.")]
    private GameObject _icon;

    private const string OPENED = "Opened";

    private bool _canBeOpened = true;
    private Animator _anim;

    public void Interact()
    {
        throw new System.NotImplementedException();
    }

    private void Awake()
    {
        _icon.SetActive(false);
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_canBeOpened && collision.CompareTag("Player"))
            _icon.SetActive(true);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _icon.SetActive(false);
    }

    private void Update()
    {
        if (_canBeOpened &&
            Input.GetKeyDown(KeyCode.E))
        {
            _canBeOpened = false;
            _icon.SetActive(false);
            _anim.SetBool(OPENED, true);
        }
    }
}



