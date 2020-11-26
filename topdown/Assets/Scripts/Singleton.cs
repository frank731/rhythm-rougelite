using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; } = null;
    protected virtual void Awake()
    {
        if (Instance == null)
        { // if the singleton instance has not yet been initialized
            Instance = GetComponent<T>();
        }
        else
        { // the singleton instance has already been initialized
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

}