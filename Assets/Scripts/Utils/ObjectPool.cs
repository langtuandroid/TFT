using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject _objPrefab;
    [SerializeField] private int _poolCount;

    private List<GameObject> _pool;

    private void Awake()
    {
        _pool = new();
        for ( int i = 0; i < _poolCount; i++ )
            CreateObject();
    }

    private void CreateObject()
    {
        var go = Instantiate( _objPrefab , transform.position, Quaternion.identity );
        go.SetActive( false );
        _pool.Add( go );
    }

    public GameObject GetPooledObject()
    {
        var go = _pool.Find( obj => !obj.activeSelf );
        if ( go == null )
        {
            CreateObject();
            go = _pool[^1];
        }
        go.SetActive( true );
        return go;
    }
}
