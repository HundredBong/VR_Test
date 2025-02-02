using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FireBulletOnActivate : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnPoint;
    public float fireSpeed;
    public AudioClip clip;

    private XRGrabInteractable grabbale;
    private AudioSource audioSource;

    private void Awake()
    {
        grabbale = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        grabbale.activated.AddListener(FireBullet);
    }

    private void OnDisable()
    {
        grabbale.activated.RemoveListener(FireBullet);
    }

    public void FireBullet(ActivateEventArgs arg = null)
    {
        GameObject spawnBullet = Instantiate(bullet);
        spawnBullet.transform.position = spawnPoint.position;
        spawnBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * fireSpeed;
        Destroy(spawnBullet, 3f);
        audioSource.PlayOneShot(clip);
    }

}
