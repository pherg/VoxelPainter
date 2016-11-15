using UnityEngine;
using System.Collections;

public class Voxel : MonoBehaviour
{
    VoxelProxy currentVoxelProxy;
    public VoxelProxy CurrentVoxelProxy
    {
        get { return currentVoxelProxy; }
        set
        {
            currentVoxelProxy = value;
        }
    }

    Material myMaterial;

    void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        myMaterial = renderer.material;
    }

    public void UpdateValues()
    {
        myMaterial.color = myMaterial.color;
    }
}
