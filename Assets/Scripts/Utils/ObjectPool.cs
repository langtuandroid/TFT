using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _objPrefab;
    [SerializeField] private int _poolCount;

    private List<GameObject> _pool = new List<GameObject>();

    private void Awake()
    {
        for ( int i = 0; i < _poolCount; i++ )
        {
            var go = Instantiate( _objPrefab , transform.position, Quaternion.identity );
            go.SetActive( false );
            _pool.Add( _objPrefab );
        }
    }

    public GameObject GetPooledObject()
    {
        foreach ( var go in _pool )
        {
            if ( !go.activeSelf )
            {
                go.SetActive( true );
                return go;
            }
        }
        var newGo = Instantiate( _objPrefab , transform.position, Quaternion.identity );
        _pool.Add( newGo );
        return newGo;
    }
}
