using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MeteorPistol : MonoBehaviour
{
    private XRGrabInteractable interactable;
    private ParticleSystem wave;

    private bool rayActivate = false;

    public LayerMask layerMask;
    public Transform shootSource;
    public float distance = 10f;


    private void Awake()
    {
        interactable = GetComponent<XRGrabInteractable>();
        wave = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        interactable.activated.AddListener(StartShoot);
        interactable.deactivated.AddListener(StopShoot);
    }

    private void OnDisable()
    {
        interactable.activated.RemoveListener(StartShoot);
        interactable.deactivated.RemoveListener(StopShoot);
    }

    private void Update()
    {
        if (rayActivate)
        {
            RayCastCheck();
        }
    }

    private void StartShoot(ActivateEventArgs arg)
    {
        wave.Play();
        rayActivate = true;
    }

    private void StopShoot(DeactivateEventArgs arg)
    {
        wave.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        rayActivate = false;
    }

    private void RayCastCheck()
    {
        RaycastHit hit;
        bool hasHit = Physics.Raycast(shootSource.position, shootSource.forward, out hit, distance, layerMask);

        if (hasHit)
        {
            hit.transform.gameObject.SendMessage("Break", SendMessageOptions.DontRequireReceiver);
        }
    }
}
