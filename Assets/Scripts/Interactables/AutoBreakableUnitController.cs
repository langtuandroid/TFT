using UnityEngine;

public class AutoBreakableUnitController : MonoBehaviour
{
    [SerializeField] private GameObject _ground;
    [SerializeField] private GameObject _hole;

    public void Break()
    {
        _ground.SetActive( false );
        _hole.SetActive( true );
    }
}
