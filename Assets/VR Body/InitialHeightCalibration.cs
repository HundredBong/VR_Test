using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class InitialHeightCalibration : MonoBehaviour
{
    public XROrigin origin;
    public Camera _camera;
    public float desiredHeight = 1.5f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1.4f);
        float actualCameraHeight = _camera.transform.position.y;
        float heightDifference = desiredHeight - actualCameraHeight;

        //origin.CameraYOffset += heightDifference;
        origin.transform.position += new Vector3(transform.position.x, heightDifference, transform.position.y);
    }
}
