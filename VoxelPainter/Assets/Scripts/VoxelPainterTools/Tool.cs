using UnityEngine;
using System.Collections;
using Valve.VR;

public abstract class Tool : MonoBehaviour
{
    Coroutine update; 
    public virtual void OnEnter()
    {
        Debug.Log("Starting coroutine.");
        update = StartCoroutine(Run());
    }

    public virtual void OnExit()
    {
        if (update != null)
        {
            StopCoroutine(update);
        }
    }

    protected abstract IEnumerator Run();
}
