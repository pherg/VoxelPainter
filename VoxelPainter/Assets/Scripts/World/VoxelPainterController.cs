using UnityEngine;
using System.Collections;
using Valve.VR;

public class VoxelPainterController : MonoBehaviour
{
    static VoxelPainterController instance;
    public static VoxelPainterController Instance
    {
        get { return instance; }
    }

    public World World;

    public ToolStateMachine ToolStateMachine;

    void Start ()
    {
        VoxelPainterController.instance = this;
        StartCoroutine("WaitForCoreInitialization");
	}

    IEnumerator WaitForCoreInitialization()
    {
        while (!Core.Instance.Initialized)
        {
            yield return 0;
        }
        ToolStateMachine.EnterPaintTool();
    }
}
