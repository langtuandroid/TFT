using UnityEngine;
using UnityEngine.Events;
using Utils;

public class Switch : MonoBehaviour
{
    [SerializeField] private Color activatedColor;
    [SerializeField] private Color deactivatedColor;
    [SerializeField] private GameObject[] objectsToActivate;
    
    [Header("Events on Activate Objects")]
    public UnityEvent OnFireActivation;
    
    private bool isActivated = false;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = deactivatedColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG_PLAYER))
        {
            ToggleSwitch();
        }
    }

    private void ToggleSwitch()
    {
        isActivated = !isActivated;
        spriteRenderer.color = isActivated ? activatedColor : deactivatedColor;

        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(isActivated);
            OnFireActivation?.Invoke();
        }
    }
}