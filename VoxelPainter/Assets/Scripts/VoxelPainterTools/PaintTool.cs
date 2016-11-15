using UnityEngine;
using System.Collections;

public class PaintTool : Tool
{
    public float distanceScaleSensitivity = 3;

    Coroutine currentInputCoroutine;
    protected override IEnumerator Run()
    {
        while (true)
        {
            ViveWand toolWand = Core.Instance.rightWand;
            Vector3 logicalPosToolWand = new Vector3(0, -69696969, 0);
            if (toolWand)
            {
                logicalPosToolWand = VoxelPainterController.Instance.World.GetGridNodeIndex(toolWand.transform.position);
            }
            
            ViveWand paletteWand = Core.Instance.leftWand;
            Vector3 logicalPosPaletteHand = new Vector3(0, -69696969, 0);
            if (paletteWand)
            {
                logicalPosPaletteHand = VoxelPainterController.Instance.World.GetGridNodeIndex(paletteWand.transform.position);
            }

            // If we are currently running an input exit the loop.
            if (currentInputCoroutine == null)
            {
                // Grip Gestures for TRS of world:
                if (paletteWand.GetButtonPressed(Valve.VR.EVRButtonId.k_EButton_Grip) && toolWand.GetButtonDownThisFrame(Valve.VR.EVRButtonId.k_EButton_Grip))
                {
                    currentInputCoroutine = StartCoroutine(MoveAndScale());
                }
                // Paint.
                else
                {
                    if (toolWand.GetButtonPressed(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
                    {
                        VoxelPainterController.Instance.World.SetVoxelAtIndex(logicalPosToolWand, new VoxelProxy(Color.white));
                    }
                    else if (toolWand.GetButtonPressed(Valve.VR.EVRButtonId.k_EButton_Grip))
                    {
                        VoxelPainterController.Instance.World.ClearVoxelAtIndex(logicalPosToolWand);
                    }
                }
            }

            yield return 0;
        }
    }

    IEnumerator MoveAndScale()
    {
        yield return 0;
        ViveWand toolWand = Core.Instance.rightWand;
        ViveWand paletteWand = Core.Instance.leftWand;

        /// Scale Vars
        float initialScale = VoxelPainterController.Instance.World.transform.localScale.x;
        float initDistance = Vector3.Distance(toolWand.transform.position, paletteWand.transform.position);
        float currentDistance;
        float scaleChange = 0;
        //Debug.Log("Most initial scale: " + initialScale + " VoxelPainterController.Instance.World.transform.localScale: " + VoxelPainterController.Instance.World.transform.localScale);


        /// Pos Vars
        Vector3 initWorldPosition = VoxelPainterController.Instance.World.transform.position;
        Vector3 initCenter = (toolWand.transform.position + paletteWand.transform.position) * 0.5f;
        Vector3 currentCenter = initCenter;
        Vector3 positionDelta = Vector3.zero;
        while (toolWand.GetButtonPressed(Valve.VR.EVRButtonId.k_EButton_Grip) && paletteWand.GetButtonPressed(Valve.VR.EVRButtonId.k_EButton_Grip))
        {
            ///
            /// Handle Scale
            /// 
            currentDistance = Vector3.Distance(toolWand.transform.position, paletteWand.transform.position);
            //Debug.Log("Scaling.  InitDist: " + initDistance + " CurrDist: " + currentDistance);
            currentDistance -= initDistance;
            scaleChange = initialScale + currentDistance * distanceScaleSensitivity;
            //Debug.Log("Scale change: " + scaleChange + " Init Scale: " + initialScale);
            VoxelPainterController.Instance.World.transform.localScale = Vector3.one * scaleChange;

            ///
            /// Handle Position
            /// 
            currentCenter = (toolWand.transform.position + paletteWand.transform.position) * 0.5f;
            positionDelta = (currentCenter - initCenter) * 3f;
            VoxelPainterController.Instance.World.transform.position = initWorldPosition + positionDelta;
            yield return 0;
        }

        currentInputCoroutine = null;
    }
}
