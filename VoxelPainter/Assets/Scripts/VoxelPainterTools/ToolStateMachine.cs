using UnityEngine;
using System.Collections;

public class ToolStateMachine : MonoBehaviour {

    public PaintTool PaintTool;

    Tool currentTool;
    public void EnterPaintTool()
    {
        ExitCurrentState();
        currentTool = PaintTool;
        currentTool.OnEnter();
    }

    void ExitCurrentState()
    {
        if (currentTool != null)
        {
            currentTool.OnExit();
        }
    }
}
