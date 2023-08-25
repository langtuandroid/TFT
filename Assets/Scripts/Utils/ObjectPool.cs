using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject _objPrefab;
    private List<GameObject> _pool;

    public ObjectPool( GameObject objPrefab , int poolCount )
    {
        _objPrefab = objPrefab;
        _pool = new();
        for ( int i = 0; i < poolCount; i++ )
            CreateObject();
    }

    private void CreateObject()
    {
        var go = MonoBehaviour.Instantiate( _objPrefab );
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
