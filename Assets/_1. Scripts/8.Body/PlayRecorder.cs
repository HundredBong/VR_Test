using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityAnimationRecorder))]
public class PlayRecorder : MonoBehaviour
{  
    public float duration = 5;
    public bool playOnStart = true;
    private UnityAnimationRecorder recorder;
    public Animation miniMe;
    public UnityEngine.Events.UnityEvent onRecordingEnd;
    public UnityEngine.UI.Slider slider;

    private void Awake()
    {
        recorder = GetComponent<UnityAnimationRecorder>();
    }

    
    private void Start()
    {

        if (playOnStart)
            StartCoroutine(RecordingCoroutine());
    }

    public void SetDuration(float _duration)
    {
        duration = _duration;
    }

    public void Play()
    {
        StartCoroutine(RecordingCoroutine());
    }

    private IEnumerator RecordingCoroutine()
    {
        recorder.StartRecording();
        float time = 0;

        while(time < duration)
        {
            slider.value = time / duration;
            time += Time.deltaTime;
            yield return null;
        }
        recorder.StopRecording();

        AnimationClip clip = recorder.lastMadeClip;
        clip.legacy = true;
        miniMe.clip = clip;
        miniMe.AddClip(clip, "default");
        miniMe.Play("default");

        onRecordingEnd.Invoke();
    }
}
