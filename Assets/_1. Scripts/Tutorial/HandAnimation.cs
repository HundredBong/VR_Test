using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimation : MonoBehaviour
{
    public InputActionProperty triggerAction;
    public InputActionProperty grapAction;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetFloat("Grip", grapAction.action.ReadValue<float>());
        anim.SetFloat("Trigger", triggerAction.action.ReadValue<float>());
    }

}
