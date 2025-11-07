using System;
using UnityEngine;

public static class CameraMain
{
    static public GameObject mainCamera;

    static CameraMain()
    {
        GameObject[] cameraObjects = GameObject.FindGameObjectsWithTag("MainCamera");
        mainCamera = cameraObjects[0];
    }

    static public void ShowObj(RGScriptedObject o, Vector3 camOffset, Vector3 targetOffset, bool follow)
    {
        mainCamera.transform.SetParent(null);
        Vector3 objectPos = o.transform.position;
        mainCamera.transform.position = objectPos+camOffset;
        mainCamera.transform.LookAt(objectPos+targetOffset);

        Debug.Log($"ShowObj pos: {objectPos}+{camOffset} = {objectPos+camOffset}; LA:{objectPos}+{targetOffset} = {objectPos+targetOffset}");

        // if follow = true parent camera to object
        // might not be accurate, attr 45 camera_move is not set on cyrus, but camera follows him anyways
        if(follow)
        {
            mainCamera.transform.SetParent(o.transform);
        }

    }
}
