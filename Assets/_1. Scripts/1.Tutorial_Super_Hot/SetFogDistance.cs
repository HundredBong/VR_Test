using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFogDistance : MonoBehaviour
{
    private void Start()
    {
        RenderSettings.fogEndDistance = 5f;
    }

    private void Update()
    {
        RenderSettings.fogEndDistance = Mathf.Lerp(RenderSettings.fogEndDistance, 50f, 0.01f);
    }
}
