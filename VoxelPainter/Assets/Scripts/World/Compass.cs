using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Compass : MonoBehaviour
{
    public Vector3 ControllerOffset = new Vector3(0, 0.3f, -0.4f);
    public Transform positionCanvasTransform;
    public Text positionText;

    void Update()
    {
        if (Core.Instance.rightWand == null)
        {
            transform.position = new Vector3(0, -696969f, 0);
            return;
        }
        Transform worldTransform = VoxelPainterController.Instance.World.transform;
        transform.rotation = worldTransform.rotation;
        Vector3 positionOffset = Core.Instance.rightWand.transform.right * ControllerOffset.x + Core.Instance.rightWand.transform.up * ControllerOffset.y + Core.Instance.rightWand.transform.forward * ControllerOffset.z;
        //float xOffset = Core.Instance.rightWand.transform.right.x * ControllerOffset.x;
        //float yOffset = CoreCore.Instance.rightWand.transform * ControllerOffset.y;
        transform.position = Core.Instance.rightWand.transform.position + positionOffset;

        Vector3 cameraLookAt = transform.position - Camera.main.transform.position;
        positionCanvasTransform.rotation = Quaternion.LookRotation(cameraLookAt, Vector3.up);
        Vector3 logicalPos = VoxelPainterController.Instance.World.GetGridNodeIndex(Core.Instance.rightWand.transform.position);
        positionText.text = "(" + logicalPos.x + ", " + logicalPos.y + ", " + logicalPos.z + ")";
    }
}
