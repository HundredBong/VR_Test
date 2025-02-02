using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableGrabbingHandModel : MonoBehaviour
{
    public GameObject leftHandModel;
    public GameObject rightHandModel;

    private XRGrabInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(HideGrabbingHand);
        interactable.selectExited.AddListener(ShowGrabbingHand);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(HideGrabbingHand);
        interactable.selectExited.RemoveListener(ShowGrabbingHand);
    }

    private void HideGrabbingHand(SelectEnterEventArgs arg)
    {
        if (arg.interactorObject.transform.CompareTag("Left Hand"))
        {
            leftHandModel.SetActive(false);
        }
        else if (arg.interactorObject.transform.CompareTag("Right Hand"))
        {
            rightHandModel.SetActive(false);
        }
    }

    private void ShowGrabbingHand(SelectExitEventArgs arg)
    {
        if (arg.interactorObject.transform.CompareTag("Left Hand"))
        {
            leftHandModel.SetActive(true);
        }
        else if (arg.interactorObject.transform.CompareTag("Right Hand"))
        {
            rightHandModel.SetActive(true);
        }
    }
}
