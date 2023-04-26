// ************ @autor: Álvaro Repiso Romero *************
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

public static class ServiceLocator
{
    private static Dictionary<Type, object> serviceContainer = new();

    public static bool IsInitialized = false;

    public static void AddService<T>( T instance )
    {
        Type type = typeof( T );
        Assert.IsFalse( serviceContainer.ContainsKey( type ) );
        serviceContainer.Add( type , instance );
    }

    public static T GetService<T>()
    {
        Type type = typeof( T );
        Assert.IsTrue( serviceContainer.ContainsKey( type ) );
        return ( T )serviceContainer[type];
    }
}
