using UnityEngine.Events;
using UnityEngine;
using Utils;

public class Chest : ActivableSceneObject, IInteractable
{
    #region SerializeFields

    [SerializeField]
    [Tooltip("Icono de exclamaci�n")]
    private GameObject _exclamationIcon;

    [SerializeField]
    [Tooltip("Prefab del objeto que aparece tras abrirlo")]
    private GameObject _givenObjectPrefab;
    #endregion

    public UnityEvent OnChestOpen;

    #region Private variables

    // COMPONENTS
    private Animator _anim;

    #endregion

    #region Unity methods

    private void Awake()
    {
        // Components
        _anim = GetComponent<Animator>();

        // Variables
        _hasBeenActivated = false;
    }

    #endregion

    #region Public methods

    public override void TriggerActivation()
    {
        base.TriggerActivation();
        _anim.SetBool(Constants.ANIM_CHEST_OPENED, true);
    }

    public void Interact(Vector2 lookDirection)
    {
        if (!_hasBeenActivated && lookDirection.y > 0)
        {
            _anim.SetBool(Constants.ANIM_CHEST_OPENED, true);
            ShowCanInteract(false);
            _hasBeenActivated = true;

            OnChestOpen?.Invoke();
            InstantiateObject();
        }
    }

    public void ShowCanInteract(bool show)
    {
        if (!_hasBeenActivated)
            _exclamationIcon.SetActive(show);
    }

    #endregion

    #region Private variables

    /// <summary>
    /// M�todo que instancia el objeto contenido en el cofre
    /// </summary>
    private void InstantiateObject()
    {
        // Calculamos la posici�n en la que aparecer� el objeto
        Vector2 outsidePos =
            Random.insideUnitCircle.normalized * 2 * .815f;

        // Aparece siempre en la mitad superior del cofre
        if (outsidePos.y < 0f)
            outsidePos = new Vector2(
                outsidePos.x, -outsidePos.y + .815f / 2
                );
        else
            outsidePos = new Vector2(
                outsidePos.x, outsidePos.y + .815f / 2
                );

        // Instanciamos el objeto
        Instantiate(
            _givenObjectPrefab, // prefab
            outsidePos, // posici�n
            Quaternion.identity // rotaci�n
            );
    }

    #endregion
}