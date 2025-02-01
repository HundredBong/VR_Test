using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivateTeleportationRay : MonoBehaviour
{
    public GameObject leftTeleportationRay;
    public GameObject rightTeleportationRay;

    public InputActionProperty leftTrigger;
    public InputActionProperty rightTrigger;

    public InputActionProperty leftGrab;
    public InputActionProperty rightGrab;

    public XRRayInteractor leftRay;
    public XRRayInteractor rightRay;

    private void Update()
    {
        bool isLeftRayHovering = leftRay.TryGetHitInfo(out Vector3 leftPos, out Vector3 leftNormal, out int leftNumber, out bool leftValid);
        bool isRightRayHovering = rightRay.TryGetHitInfo(out Vector3 rightPos, out Vector3 rightNormal, out int rightNumber, out bool rightValid);

        leftTeleportationRay.SetActive(!isLeftRayHovering && leftGrab.action.ReadValue<float>() == 0 && leftTrigger.action.ReadValue<float>() > 0.1f);
        rightTeleportationRay.SetActive(!isRightRayHovering && rightGrab.action.ReadValue<float>() == 0 && rightTrigger.action.ReadValue<float>() > 0.1f);
    }
}
