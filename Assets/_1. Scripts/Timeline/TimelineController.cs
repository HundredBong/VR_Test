using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Timeline
{
    public class TimelineController : MonoBehaviour
    {
        public PlayableDirector timeline;

        public void PlayTimeline()
        {
            Debug.Log("타임라인 실행");
            timeline.Play();
        }
    }
}

