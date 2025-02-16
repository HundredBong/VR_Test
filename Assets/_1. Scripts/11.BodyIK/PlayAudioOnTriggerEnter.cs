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

    public bool randomizePitch = true;
    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            //source.PlayOneShot(clip);

            VelocityEstimator estimator = other.GetComponent<VelocityEstimator>();

            if (estimator && useVelocity)
            {
                float v = estimator.GetVelocityEstimate().magnitude;
                Debug.Log(estimator);
                Debug.Log(estimator.GetVelocityEstimate());
                Debug.Log(estimator.GetVelocityEstimate().magnitude);

                float volume = Mathf.InverseLerp(minVelocity, maxVelocity, v);
                Debug.Log(volume);

                if (randomizePitch)
                {
                    source.pitch = Random.Range(minPitch, maxPitch);
                }

                source.PlayOneShot(clip, volume);
            }
            else
            {
                if (randomizePitch)
                {
                    source.pitch = Random.Range(minPitch, maxPitch);
                }
                source.PlayOneShot(clip);
            }
            Debug.Log("클립 재생");
        }
    }
}
