using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    public Transform VoxelPrefab;
    public Transform VoxelContainer;
    public float GridSize = 0.01f;
    float snapToForGridSize;
    float inverseSnapToForGrid;

    Dictionary<Vector3, VoxelProxy> dirtyVoxelsToUpdate = new Dictionary<Vector3, VoxelProxy>();
    Dictionary<Vector3, VoxelProxy> dirtyVoxelsToAdd = new Dictionary<Vector3, VoxelProxy>();
    List<Vector3> dirtyVoxelsToRemove = new List<Vector3>();

    Dictionary<Vector3, Voxel> voxelGrid = new Dictionary<Vector3, Voxel>();


    void Awake()
    {
        snapToForGridSize = GridSize;
        inverseSnapToForGrid = 1 / GridSize;
    }

    void Update()
    {
        RenderVoxels();
    }

    void RenderVoxels()
    {
        Transform voxelTransform;
        Voxel voxel;
        // Add
        foreach (KeyValuePair<Vector3, VoxelProxy> kvp in dirtyVoxelsToAdd)
        {
            if ( voxelGrid.TryGetValue(kvp.Key, out voxel))
            {
                Debug.LogWarning("Trying to add a voxel to a index that already has a voxel in the grid: " + kvp.Key);
                continue;
            }
            voxelTransform = Instantiate(VoxelPrefab, VoxelContainer.TransformPoint(kvp.Key* GridSize), Quaternion.identity, VoxelContainer) as Transform;
            voxelTransform.localRotation = Quaternion.identity;
            voxel = voxelTransform.GetComponent<Voxel>();
            voxel.CurrentVoxelProxy = kvp.Value;
            voxelTransform.localScale *= GridSize;
            voxel.UpdateValues();
            voxelGrid.Add(kvp.Key, voxel);
        }

        // Update
        foreach(KeyValuePair<Vector3, VoxelProxy> kvp in dirtyVoxelsToUpdate)
        {
            if(voxelGrid.TryGetValue(kvp.Key, out voxel))
            {
                voxel.CurrentVoxelProxy = kvp.Value;
                voxel.UpdateValues();
            }
            else
            {
                Debug.LogWarning("Trying to update a voxel that doesn't exist in the voxel grid: " + kvp.Key);
            }
        }

        // Remove
        foreach (Vector3 voxelPosToRemove in dirtyVoxelsToRemove)
        {
            if (!voxelGrid.TryGetValue(voxelPosToRemove, out voxel))
            {
                Debug.LogWarning("Trying to remove voxel at position in grid that has no voxel: " + voxelPosToRemove);
                continue;
            }
            GameObject.Destroy(voxel.gameObject);
            voxelGrid.Remove(voxelPosToRemove);
        }

        dirtyVoxelsToAdd.Clear();
        dirtyVoxelsToUpdate.Clear();
        dirtyVoxelsToRemove.Clear();
    }

    public Vector3 GetGridNodeIndex(Vector3 worldPoint)
    {
        Vector3 localSpacePoint = VoxelContainer.InverseTransformPoint(worldPoint);
        return TransformUtils.SnapToGrid(localSpacePoint, snapToForGridSize, inverseSnapToForGrid) * inverseSnapToForGrid;
    }

    public void SetVoxelAtIndex(Vector3 index, VoxelProxy voxel)
    {
        Voxel currentVoxel;
        if (voxelGrid.TryGetValue(index, out currentVoxel))
        {
            dirtyVoxelsToUpdate.Add(index, voxel);
        }
        else
        {
            dirtyVoxelsToAdd.Add(index, voxel);
        }
    }

    public void SetVoxelAtWorldPosition(Vector3 worldPosition, VoxelProxy voxelProxy)
    {
        SetVoxelAtIndex(GetGridNodeIndex(worldPosition), voxelProxy);
    }

    public void ClearVoxelAtIndex(Vector3 index)
    {
        Voxel currentVoxel;
        if (voxelGrid.TryGetValue(index, out currentVoxel))
        {
            dirtyVoxelsToRemove.Add(index);
        }
        else
        {
            Debug.LogWarning("Trying to remove voxel that doesn't exist at: " + index);
        }
    }
}
