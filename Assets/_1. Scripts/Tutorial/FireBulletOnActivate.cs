using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBulletOnActivate : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnPoint;
    public float fireSpeed;

    private XRGrabInteractable grabbale;

    private void Awake()
    {
        grabbale = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grabbale.activated.AddListener(FireBullet);
    }

    private void OnDisable()
    {
        grabbale.activated.RemoveListener(FireBullet);
    }

    private void FireBullet(ActivateEventArgs arg)
    {
        GameObject spawnBullet = Instantiate(bullet);
        spawnBullet.transform.position = spawnPoint.position;
        spawnBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * fireSpeed;
        Destroy(spawnBullet, 3f);
    }

}
