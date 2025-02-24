using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRTestGrab : XRGrabInteractable
{
   // public InputActionProperty rightGrabAction;

    protected override void OnHoverEntered(HoverEnterEventArgs arg)
    {
        Debug.Log($"{arg.interactableObject.isHovered}");
        Debug.Log($"{arg.interactorObject is XRRayInteractor}");
        Debug.Log((IXRSelectInteractor)arg.interactorObject);


        if (arg.interactableObject.isHovered && arg.interactorObject is XRRayInteractor)// && rightGrabAction.action.ReadValue<float>() > 0.1f)
        {
            interactionManager.SelectEnter((IXRSelectInteractor)arg.interactorObject, this);
        }

        base.OnHoverEntered(arg);
    }
}
