using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Makes the children of this class into singletons and
// also makes them persist on load.
public class Manager<T> : MonoBehaviour where T : Manager<T>
{
    private static T _instance;

    public static T Instance
    {
        get => _instance;
        private set
        {
            if (_instance == null)
            {
                _instance = value;
                DontDestroyOnLoad(Instance.gameObject);
            }
            else
            {
                Destroy(value.gameObject);
                Debug.LogError("[" + value.name + "]" + "Trying to instantiate a second instance of singleton class.");
            }
        }
    }

    public static bool IsInitialized => _instance != null;

    protected virtual void Awake()
    {
        Instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}