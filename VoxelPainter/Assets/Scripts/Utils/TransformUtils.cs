using UnityEngine;
using System.Collections;

public class TransformUtils
{ 
    public static Vector3 SnapToGrid(Vector3 point, float gridSize, float inverseGridSize)
    {
        float xGridPos = Mathf.Round(point.x * inverseGridSize) * gridSize;
        float yGridPos = Mathf.Round(point.y * inverseGridSize) * gridSize;
        float zGridPos = Mathf.Round(point.z * inverseGridSize) * gridSize;

        return new Vector3(xGridPos, yGridPos, zGridPos);
    }

    public static void KillAllChildren(Transform parent)
    {
        Transform child;
        while (parent.childCount > 0)
        {
            child = parent.GetChild(0);
            child.SetParent(null);
            UnityEngine.MonoBehaviour.Destroy(child.gameObject);
        }
    }
}
