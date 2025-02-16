using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnTriggerEnter : MonoBehaviour
{
    private AudioSource source;
    public AudioClip clip;
    public string targetTag;

    public bool useVelocity = true;
    public float minVelocity = 0;
    public float maxVelocity = 2;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            VelocityEstimator estimator = other.GetComponent<VelocityEstimator>();

            if (estimator && useVelocity)
            {
                float v = estimator.GetVelocityEstimate().magnitude;
                float volume = Mathf.InverseLerp(minVelocity, minVelocity, v);
                source.PlayOneShot(clip, volume);
            }
            else
            {
                source.PlayOneShot(clip);
            }
            Debug.Log("클립 재생");
        }
    }
}
