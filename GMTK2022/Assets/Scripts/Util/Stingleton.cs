using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式父类
/// </summary>
/// <typeparam name="T">实际单例类</typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;

    public static T Instance
    {
        get => instance;
    }

    protected virtual void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = (T)this;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}