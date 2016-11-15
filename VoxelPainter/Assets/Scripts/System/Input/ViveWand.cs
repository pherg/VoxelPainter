using UnityEngine;
using System.Collections;
using Valve.VR;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class ViveWand : MonoBehaviour {

    SteamVR_Controller.Device device;
    SteamVR_TrackedObject trackedObject;

    void Start()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        if (trackedObject == null)
        {
            Debug.Log("Tracked object not found.");
        }
    }

    public bool GetButtonDownThisFrame(EVRButtonId button)
    {
        if (trackedObject == null)
        {
            return false;
        }
        device = SteamVR_Controller.Input((int)trackedObject.index);
        return device.GetPressDown(button);
    }

    public bool GetButtonUpThisFrame(EVRButtonId button)
    {
        if (trackedObject == null)
        {
            return false;
        }
        device = SteamVR_Controller.Input((int)trackedObject.index);
        return device.GetPressUp(button);
    }

    public bool GetButtonPressed(EVRButtonId button)
    {
        if (trackedObject == null)
        {
            return false;
        }
        device = SteamVR_Controller.Input((int)trackedObject.index);
        return device.GetPress(button);
    }

    public Vector2 GetAxis(EVRButtonId axis)
    {
        if (trackedObject == null)
        {
            return Vector2.zero;
        }
        device = SteamVR_Controller.Input((int)trackedObject.index);
        return device.GetAxis(axis);
    }
}
