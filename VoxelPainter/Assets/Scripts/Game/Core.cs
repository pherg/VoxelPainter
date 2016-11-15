using UnityEngine;
using System.Collections;

public class Core : MonoBehaviour {
    static Core instance;
    public static Core Instance
    {
        get { return instance; }
    }

    bool initialized = false;
    public bool Initialized
    {
        get { return initialized; }
    }

    public ViveWand leftWand;
    public ViveWand rightWand;

    void Awake()
    {
        Core.instance = this;
    }

    void Start()
    {
        StartCoroutine("Bootstrap");
    }

    IEnumerator Bootstrap()
    {
        while(!IsInitialized())
        {
            yield return 0;
        }

        initialized = true;
    }

    bool IsInitialized()
    {
        return true;
    }
}
