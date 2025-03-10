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

    private void Start()
    {
        recorder = GetComponent<UnityAnimationRecorder>();
        if (playOnStart)
            StartCoroutine(RecordingRoutine());
    }

    public void SetDuration(float _duration)
    {
        duration = _duration;
    }

    public void Play()
    {
        StartCoroutine(RecordingRoutine());
    }

    private IEnumerator RecordingRoutine()
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
