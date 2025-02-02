using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlaySteps : MonoBehaviour
{
    private PlayableDirector director;
    public List<Step> steps;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();    
    }

    public void PlayeStepIndex(int index)
    {
        Step step = steps[index];

        if (step.hasPlayed == false)
        {
            director.Stop();
            director.time = step.time;
            director.Play();
        }
    }

}

[System.Serializable]
public class Step
{
    public string name;
    public float time;
    public bool hasPlayed = false;
}