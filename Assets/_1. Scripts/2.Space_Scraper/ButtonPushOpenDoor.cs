using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonPushOpenDoor : MonoBehaviour
{
    public Animator anim;
    public string parameterName = "Open";


    private XRSimpleInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(ToggleDoorOpen);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(ToggleDoorOpen);
    }

    private void ToggleDoorOpen(SelectEnterEventArgs arg)
    {
        anim.SetBool(parameterName, !anim.GetBool(parameterName));
    }
}
